using Hangfire.Server;
using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Media;
using Iwan.Server.Hubs;
using Iwan.Server.Services.Media;
using Iwan.Server.Services.Settings;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.RealTime;
using Iwan.Shared.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Iwan.Server.Services.RealTime
{
    [Injected(ServiceLifetime.Scoped, typeof(IRealTimeNotifierImagesManipulatorService))]
    public class RealTimeNotifierImagesManipulatorService : IRealTimeNotifierImagesManipulatorService
    {
        protected readonly IHubContext<AdminHub> _hubContext;
        protected readonly IUnitOfWork _context;
        protected readonly IImageManipulatorService _imageManipulatorService;
        protected readonly IQuerySettingService _querySettingService;

        public RealTimeNotifierImagesManipulatorService(IUnitOfWork context, IHubContext<AdminHub> hubContext,
            IImageManipulatorService imageManipulatorService, IQuerySettingService querySettingService)
        {
            _context = context;
            _hubContext = hubContext;
            _imageManipulatorService = imageManipulatorService;
            _querySettingService = querySettingService;
        }

        public async Task AddWatermarkToImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var productsCount = await _context.ProductsRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            // var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);
            var jobDetails = await _context.JobDetailsRepository.Table
                .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.Watermarking && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.WatermarkProgress)
                    .SendAsync(ServerMessages.WatermarkingJobStarted, new EntityImagesManipulationInitializationDto
                    {
                        NumberOfEntitiesToProcess = productsCount,
                    }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedProducts = 0;

            var watermarkSettings = await _querySettingService.GetWatermarkImageSettingsAsync(cancellationToken);

            var watermarkImageBytes = await _imageManipulatorService.GetBytesAsync(watermarkSettings.WatermarkImage, cancellationToken);

            while(true)
            {
                var product = await _context.ProductsRepository.Table
                    .OrderBy(p => p.Number).Skip(processedProducts)
                    .FirstOrDefaultAsync(cancellationToken);

                if (product == null) break;

                var productMainImage = await _context.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(product.Id, cancellationToken);
                var productImages = await _context.ProductImagesRepository.GetImagesWithImageModelsAsync(product.Id, cancellationToken);

                var images = new List<Image>();

                if (productMainImage != null)
                {
                    images.AddRange(new List<Image>() { productMainImage.MediumImage, productMainImage.SmallImage, productMainImage.MobileImage });
                }

                foreach (var subImage in productImages)
                    images.AddRange(new List<Image> { subImage.MediumImage, subImage.SmallImage, subImage.MobileImage });

                // Add watermark to the images
                await _imageManipulatorService.AddWatermarkToImagesAsync(images, watermarkImageBytes, watermarkSettings.Opacity, cancellationToken);

                // Notify that we processed an image
                processedProducts++;
                await _hubContext.Clients.Group(SignalRGroups.WatermarkProgress)
                    .SendAsync(ServerMessages.WatermarkingJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedProducts,
                        EntitiesLeft = productsCount - processedProducts,
                        NumberOfEntitiesToProcess = productsCount
                    }.ToJson(), cancellationToken);

                await Task.Delay(500);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.WatermarkProgress)
                .SendAsync(ServerMessages.WatermarkingJobEnded, cancellationToken);

            ServerState.WorkingOnProducts = false;
        }

        public async Task RemoveWatermarkFromImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var productsCount = await _context.ProductsRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            // var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);

            var jobDetails = await _context.JobDetailsRepository.Table
                .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.UnWatermarking && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.UnWatermarkProgress)
                    .SendAsync(ServerMessages.UnWatermarkingJobStarted, new EntityImagesManipulationInitializationDto
                    {
                        NumberOfEntitiesToProcess = productsCount
                    }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedProducts = 0;

            var imagesSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var product = await _context.ProductsRepository.Table
                    .OrderBy(p => p.Number).Skip(processedProducts)
                    .FirstOrDefaultAsync(cancellationToken);

                if (product == null) break;

                var productMainImage = await _context.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(product.Id, cancellationToken);
                var originalMainImageBytes = await _imageManipulatorService.GetBytesAsync(productMainImage.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalMainImageBytes, cancellationToken);

                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalMainImageBytes, cancellationToken);
                
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalMainImageBytes, cancellationToken);

                var productImages = await _context.ProductImagesRepository.GetImagesWithImageModelsAsync(product.Id, cancellationToken);

                // Resize sub-images
                foreach (var subImage in productImages)
                {
                    var originalSubImageBytes = await _imageManipulatorService.GetBytesAsync(subImage.OriginalImage, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalSubImageBytes, cancellationToken);
                }

                // Notify that we processed an image
                processedProducts++;
                await _hubContext.Clients.Group(SignalRGroups.UnWatermarkProgress)
                    .SendAsync(ServerMessages.UnWatermarkingJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedProducts,
                        EntitiesLeft = productsCount - processedProducts,
                        NumberOfEntitiesToProcess = productsCount
                    }.ToJson(), cancellationToken);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.UnWatermarkProgress)
                .SendAsync(ServerMessages.UnWatermarkingJobEnded, cancellationToken);

            ServerState.WorkingOnProducts = false;
        }

        public async Task ResizeProductsImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var productsCount = await _context.ProductsRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(Thread.CurrentThread.ManagedThreadId.ToString(), cancellationToken);

            //var jobDetails = await _context.JobDetailsRepository.Table
            //    .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.ResizingProductsImages && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.ProductsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingProductsImagesJobStarted, new EntityImagesManipulationInitializationDto
                {
                    NumberOfEntitiesToProcess = productsCount
                }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedProducts = 0;

            var imagesSettings = await _querySettingService.GetProductsImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var product = await _context.ProductsRepository
                    .Table.OrderBy(p => p.Number).Skip(processedProducts)
                    .FirstOrDefaultAsync(cancellationToken);

                if (product == null) break;

                var productMainImage = await _context.ProductMainImagesRepository.GetByProductIdAndIncludeImagesAsync(product.Id, cancellationToken);
                var originalMainImageBytes = await _imageManipulatorService.GetBytesAsync(productMainImage.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalMainImageBytes, cancellationToken);
                

                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalMainImageBytes, cancellationToken);
                
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(productMainImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalMainImageBytes, cancellationToken);

                var productImages = await _context.ProductImagesRepository.GetImagesWithImageModelsAsync(product.Id, cancellationToken);

                // Resize sub-images
                foreach (var subImage in productImages)
                {
                    var originalSubImageBytes = await _imageManipulatorService.GetBytesAsync(subImage.OriginalImage, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.SmallImage, imagesSettings.SmallImageWidth, imagesSettings.SmallImageHeight, originalSubImageBytes, cancellationToken);
                    await _imageManipulatorService.ResizeFromOriginalAndReplaceImageAsync(subImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, originalSubImageBytes, cancellationToken);
                }

                // Notify that we processed an image
                processedProducts++;
                await _hubContext.Clients.Group(SignalRGroups.ProductsImagesResizeProgress)
                    .SendAsync(ServerMessages.ResizingProductsImagesJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedProducts,
                        EntitiesLeft = productsCount - processedProducts,
                        NumberOfEntitiesToProcess = productsCount,
                    }.ToJson(), cancellationToken);

                await Task.Delay(500);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.ProductsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingProductsImagesJobEnded, cancellationToken);
        }

        public async Task ResizeCategoriesImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var categoriesCount = await _context.CategoriesRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);
            //var jobDetails = await _context.JobDetailsRepository.Table
            //    .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.ResizingCategoriesImages && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.CategoriesImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingCategoriesImagesJobStarted, new EntityImagesManipulationInitializationDto
                {
                    NumberOfEntitiesToProcess = categoriesCount
                }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedCategories = 0;

            var imagesSettings = await _querySettingService.GetCategoriesImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var category = await _context.CategoriesRepository.Table
                    .OrderBy(c => c.Id).Skip(processedCategories)
                    .FirstOrDefaultAsync(cancellationToken);

                if (category == null) break;

                var categoryImage = await _context.CategoryImagesRepository.GetByCategoryIncludingImagesAsync(category.Id, cancellationToken);
                var imageBytes = await _imageManipulatorService.GetBytesAsync(categoryImage.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(categoryImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, imageBytes, cancellationToken);
                

                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(categoryImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, imageBytes, cancellationToken);

                // Notify that we processed an image
                processedCategories++;
                await _hubContext.Clients.Group(SignalRGroups.CategoriesImagesResizeProgress)
                    .SendAsync(ServerMessages.ResizingCategoriesImagesJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedCategories,
                        EntitiesLeft = categoriesCount - processedCategories,
                        NumberOfEntitiesToProcess = categoriesCount
                    }.ToJson(), cancellationToken);

                await Task.Delay(500);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.CategoriesImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingCategoriesImagesJobEnded, cancellationToken);
        }

        public async Task ResizeCompositionsImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var compositionsCount = await _context.CompositionsRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);
            //var jobDetails = await _context.JobDetailsRepository.Table
            //    .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.ResizingCompositionsImages && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.CompositionsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingCompositionsImagesJobStarted, new EntityImagesManipulationInitializationDto
                {
                    NumberOfEntitiesToProcess = compositionsCount
                }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedCompositions = 0;

            var imagesSettings = await _querySettingService.GetCompositionsImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var composition = await _context.CompositionsRepository
                    .Table.OrderBy(c => c.Id).Skip(processedCompositions).FirstOrDefaultAsync(cancellationToken);

                if (composition == null) break;

                var compositionImage = await _context.CompositionImagesRepository.GetByCompositionIncludingImagesAsync(composition.Id, cancellationToken);
                var imageBytes = await _imageManipulatorService.GetBytesAsync(compositionImage.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(compositionImage.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, imageBytes, cancellationToken);
                
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(compositionImage.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, imageBytes, cancellationToken);

                // Notify that we processed an image
                processedCompositions++;
                await _hubContext.Clients.Group(SignalRGroups.CompositionsImagesResizeProgress)
                    .SendAsync(ServerMessages.ResizingCompositionsImagesJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedCompositions,
                        EntitiesLeft = compositionsCount - processedCompositions,
                        NumberOfEntitiesToProcess = compositionsCount,
                    }.ToJson(), cancellationToken);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.CompositionsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingCompositionsImagesJobEnded, cancellationToken);
        }

        public async Task ResizeAboutUsImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var aboutUsImagesCount = await _context.AboutUsSectionImagesRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);

            //var jobDetails = await _context.JobDetailsRepository.Table
            //    .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.ResizingAboutUsImages && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.AboutUsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingAboutUsImagesJobStarted, new EntityImagesManipulationInitializationDto
                {
                    NumberOfEntitiesToProcess = aboutUsImagesCount
                }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedImages = 0;

            var imagesSettings = await _querySettingService.GetAboutUsSectionImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var image = await _context.AboutUsSectionImagesRepository.Table
                    .OrderBy(i => i.Id).Skip(processedImages).FirstOrDefaultAsync(cancellationToken);

                if (image == null) break;

                var imageBytes = await _imageManipulatorService.GetBytesAsync(image.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(image.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, imageBytes, cancellationToken);
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(image.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, imageBytes, cancellationToken);

                // Notify that we processed an image
                processedImages++;
                await _hubContext.Clients.Group(SignalRGroups.AboutUsImagesResizeProgress)
                    .SendAsync(ServerMessages.ResizingAboutUsImagesJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedImages,
                        EntitiesLeft = aboutUsImagesCount - processedImages,
                        NumberOfEntitiesToProcess = aboutUsImagesCount
                    }.ToJson(), cancellationToken);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.AboutUsImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingAboutUsImagesJobEnded, cancellationToken);
        }

        public async Task ResizeSliderImagesAsync(PerformContext context, CancellationToken cancellationToken = default)
        {
            var sliderImagesCount = await _context.SliderImagesRepository.Table.CountAsync(cancellationToken);

            // Get the job details
            var jobDetails = await _context.JobDetailsRepository.GetByJobAsync(context.BackgroundJob.Id, cancellationToken);

            //var jobDetails = await _context.JobDetailsRepository.Table
            //    .FirstOrDefaultAsync(j => j.JobTypeId == (int)JobType.ResizingSliderImages && j.JobStatusId == (int)JobStatus.Pending, cancellationToken);

            // Notify that we started processing 
            await _hubContext.Clients.Group(SignalRGroups.SliderImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingSliderImagesJobStarted, new EntityImagesManipulationInitializationDto
                {
                    NumberOfEntitiesToProcess = sliderImagesCount
                }.ToJson(), cancellationToken);

            // Might be processing when images are processed and the server got down and restarted again
            if (jobDetails.Status != JobStatus.Processing)
            {
                jobDetails.Status = JobStatus.Processing;
                await _context.SaveChangesAsync(cancellationToken);
            }

            var processedImages = 0;

            var imagesSettings = await _querySettingService.GetSlidersImagesSettingsAsync(cancellationToken);

            while (true)
            {
                var image = await _context.SliderImagesRepository.Table
                    .OrderBy(s => s.Id).Skip(processedImages).FirstOrDefaultAsync(cancellationToken);

                if (image == null) break;

                var imageBytes = await _imageManipulatorService.GetBytesAsync(image.OriginalImage, cancellationToken);

                // Resize main image
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(image.MediumImage, imagesSettings.MediumImageWidth, imagesSettings.MediumImageHeight, imageBytes, cancellationToken);
                await _imageManipulatorService
                    .ResizeFromOriginalAndReplaceImageAsync(image.MobileImage, imagesSettings.MobileImageWidth, imagesSettings.MobileImageHeight, imageBytes, cancellationToken);

                // Notify that we processed an image
                processedImages++;
                await _hubContext.Clients.Group(SignalRGroups.SliderImagesResizeProgress)
                    .SendAsync(ServerMessages.ResizingSliderImagesJobProgress, new EntityImagesManipulationProgressDto
                    {
                        DoneEntities = processedImages,
                        EntitiesLeft = sliderImagesCount - processedImages,
                        NumberOfEntitiesToProcess = sliderImagesCount
                    }.ToJson(), cancellationToken);
            }

            _context.JobDetailsRepository.Delete(jobDetails);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients.Group(SignalRGroups.SliderImagesResizeProgress)
                .SendAsync(ServerMessages.ResizingSliderImagesJobEnded, cancellationToken);
        }
    }
}
