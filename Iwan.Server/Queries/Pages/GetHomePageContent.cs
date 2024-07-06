using Iwan.Server.Extensions;
using Iwan.Server.Models.Pages;
using Iwan.Server.Services.Accounts;
using Iwan.Server.Services.Catalog;
using Iwan.Server.Services.Pages;
using Iwan.Server.Services.Sliders;
using Iwan.Shared.Options.SliderImages;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Queries.Pages
{
    public class GetHomePageContent
    {
        public record Request : IRequest<HomePageContentViewModel>;

        public class Handler : IRequestHandler<Request, HomePageContentViewModel>
        {
            protected readonly IPagesQueryService _homeQueryService;
            protected readonly ILoggedInUserProvider _loggedInUser;
            protected readonly IQueryCategoryService _queryCategoryService;
            protected readonly IQuerySliderService _querySliderService;

            public Handler(IPagesQueryService homeQueryService, ILoggedInUserProvider loggedInUser,
                IQueryCategoryService queryCategoryService, IQuerySliderService querySliderService)
            {
                _homeQueryService = homeQueryService;
                _loggedInUser = loggedInUser;
                _queryCategoryService = queryCategoryService;
                _querySliderService = querySliderService;
            }

            public async Task<HomePageContentViewModel> Handle(Request request, CancellationToken cancellationToken)
            {
                var dto = await _homeQueryService.GetHomePageContentDetailsAsync(cancellationToken);

                var header = dto.HeaderSection;
                var aboutUs = dto.AboutUsSection;
                var interior = dto.InteriorDesignSection;
                var contactUs = dto.ContactUsSection;
                var services = dto.ServicesSection;

                var headerModel = new HeaderSectionViewModel();
                var aboutUsModel = new AboutUsSectionViewModel();
                
                var interiorModel = new InteriorDesignSectionViewModel
                {
                    Url = interior.Url
                };
                
                var contactUsModel = new ContactUsSectionViewModel
                {
                    Email = contactUs.Email,
                    PhoneNumber = contactUs.PhoneNumber,
                    FacebookUrl = contactUs.FacebookUrl,
                    InstagramUrl = contactUs.InstagramUrl,
                    Location = contactUs.Location
                };

                var servicesModel = new ServicesSectionViewModel();

                if (_loggedInUser.IsCultureArabic)
                {
                    headerModel.Title = header.ArabicTitle;
                    headerModel.Subtitle1 = header.ArabicSubtitle1;
                    headerModel.Subtitle2 = header.ArabicSubtitle2;

                    interiorModel.Text = interior.ArabicText;

                    aboutUsModel.Text = aboutUs.ArabicText;

                    servicesModel.Service1Title = services.Service1ArabicTitle;
                    servicesModel.Service2Title = services.Service2ArabicTitle;
                    servicesModel.Service1Text = services.Service1ArabicText;
                    servicesModel.Service2Text = services.Service2ArabicText;
                }
                else
                {
                    headerModel.Title = header.EnglishTitle;
                    headerModel.Subtitle1 = header.EnglishSubtitle1;
                    headerModel.Subtitle2 = header.EnglishSubtitle2;

                    interiorModel.Text = interior.EnglishText;

                    aboutUsModel.Text = aboutUs.EnglishText;

                    servicesModel.Service1Title = services.Service1EnglishTitle;
                    servicesModel.Service2Title = services.Service2EnglishTitle;
                    servicesModel.Service1Text = services.Service1EnglishText;
                    servicesModel.Service2Text = services.Service2EnglishText;
                }

                // Interior images
                if (interior.Image != null)
                {
                    if (interior.Image.MainImage != null)
                        interiorModel.MediumImage = interior.Image.MainImage.ToViewModel();

                    if (interior.Image.MobileImage != null)
                        interiorModel.MobileImage = interior.Image.MobileImage.ToViewModel();
                }

                if (aboutUs.Images?.Any() ?? false)
                    aboutUsModel.Images = aboutUs.Images.Select(i => new AboutUsSectionImageViewModel
                    {
                        OriginalImage = i.OriginalImage.ToViewModel(),
                        MediumImage = i.MediumImage.ToViewModel(),
                        MobileImage = i.MobileImage.ToViewModel()
                    }).ToList();

                var categories = await _queryCategoryService.GetParentCategoriesForPublicAsync(cancellationToken);

                var sliderImagesViewModels = new List<SliderImageViewModel>();

                var sliderOptions = new GetSliderImagesOptions
                {
                    CurrentPage = 0,
                    PageSize = 30
                };

                var sliderImages = (await _querySliderService.GetSliderImagesDetailsAsync(sliderOptions, cancellationToken)).Data;

                foreach (var image in sliderImages)
                {
                    sliderImagesViewModels.Add(new SliderImageViewModel
                    {
                        MediumImage = image.MediumImage.ToViewModel(),
                        MobileImage = image.MobileImage.ToViewModel()
                    });
                }

                return new HomePageContentViewModel
                {
                    HeadSection = headerModel,
                    AboutUsSection = aboutUsModel,
                    ContactUsSection = contactUsModel,
                    InteriorDesignSection= interiorModel,
                    ServicesSection = servicesModel,
                    Categories = categories.ToList(),
                    SliderImages = sliderImagesViewModels,
                    IsCultureArabic = _loggedInUser.IsCultureArabic
                };
            }
        }
    }
}
