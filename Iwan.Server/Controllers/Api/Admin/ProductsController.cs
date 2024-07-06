using Iwan.Server.Commands.Products.Admin;
using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Events.Products;
using Iwan.Server.Queries.Products.Admin;
using Iwan.Shared;
using Iwan.Shared.Constants;
using Iwan.Shared.Dtos.Products;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Options.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Controllers.Api.Admin
{
    public class ProductsController : BaseAdminApiController
    {
        private IUnitOfWork _context;

        public ProductsController(IMediator mediator, IStringLocalizer<Localization> stringLocalizer, IUnitOfWork context)
            : base(mediator, stringLocalizer) { _context = context; }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsOptions options, CancellationToken token = default)
        {
            var products = await _mediator.Send(new GetAllProducts.Request(options), token);

            return Ok(products);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.BASE)]
        public async Task<IActionResult> Get([FromQuery]AdminGetProductsOptions options, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var products = await _mediator.Send(new GetProducts.Request(options), token);

            return Ok(products);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.GetProduct)]
        public async Task<IActionResult> Get(string id, CancellationToken token = default)
        {
            var product = await _mediator.Send(new GetProduct.Request(id), token);

            return Ok(product);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.GetProductCategories)]
        public async Task<IActionResult> GetProductCategories(string id, CancellationToken token = default)
        {
            var productCategories = await _mediator.Send(new GetProductCategories.Request(id), token);

            return Ok(productCategories);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.GetStates)]
        public async Task<IActionResult> GetProductStates(string id, CancellationToken token = default)
        {
            var states = await _mediator.Send(new GetStates.Request(id), token);

            return Ok(states);
        }

        [HttpGet]
        [Route(Routes.Api.Admin.Products.GetImages)]
        public async Task<IActionResult> GetImages(string id, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var images = await _mediator.Send(new GetProductImages.Request(id), token);

            return Ok(images);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Products.BASE)]
        public async Task<IActionResult> AddProduct(AddProductDto product, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var addedProduct = await _mediator.Send(new AddProduct.Request(product), token);

            await _mediator.Publish(new ProductAddedEvent(addedProduct.Id), token);

            return base.Ok(addedProduct);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Products.AddViaRarFile)]
        public async Task<IActionResult> AddViaRarFile([FromForm]AddProductViaRarFileDto file, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var addedProduct = await _mediator.Send(new AddProductViaRarFile.Request(file.RarFile), token);

            await _mediator.Publish(new ProductAddedEvent(addedProduct.Id), token);

            return base.Ok(addedProduct);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Products.BASE_IMAGES)]
        public async Task<IActionResult> AddProductImage(AddProductImageDto image, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var productImage = await _mediator.Send(new AddProductImage.Request(image), token);

            await _mediator.Publish(new ProductImageAddedEvent(productImage.Id), token);

            return Ok(productImage);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Products.BASE_CATEGORIES)]
        public async Task<IActionResult> AddProductCategory(AddProductCategoryDto productCategory, CancellationToken token = default)
        {
            var addedProductCategory = await _mediator.Send(new AddProductToCategory.Request(productCategory.ProductId, productCategory.CategoryId), token);

            await _mediator.Publish(new ProductAddedToCategoryEvent(addedProductCategory.Id), token);

            return base.Ok(addedProductCategory);
        }

        [HttpPost]
        [Route(Routes.Api.Admin.Products.BASE_STATES)]
        public async Task<IActionResult> AddProductState(AddProductStateDto state, CancellationToken token = default)
        {
            var addedState = await _mediator.Send(new AddState.Request(state), token);

            await _mediator.Publish(new ProductStateAddedEvent(addedState.Id), token);

            return Ok(addedState);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.BASE)]
        public async Task<IActionResult> EditProduct(EditProductDto product, CancellationToken cancellationToken = default)
        {
            var editedProduct = await _mediator.Send(new EditProduct.Request(product), cancellationToken);

            await _mediator.Publish(new ProductEditedEvent(editedProduct.Id), cancellationToken);

            return base.Ok(editedProduct);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.ChangeMainImage)]
        public async Task<IActionResult> ChangeMainImage(ChangeProductMainImageDto image, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            var product = await _mediator.Send(new ChangeProductMainImage.Request(image), token);

            await _mediator.Publish(new ProductMainImageSetEvent(product.Id), token);

            return Ok(product);
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.ResizeImage)]
        public async Task<IActionResult> ResizeImage(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new ResizeProductImage.Request(id), cancellationToken);

            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.WatermarkImage)]
        public async Task<IActionResult> WatermarkImage(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new WatermarkProductImage.Request(id), cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.UnWatermarkImage)]
        public async Task<IActionResult> UnWatermarkImage(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new UnWatermarkProductImage.Request(id), cancellationToken);
            return Ok();
        }

        [HttpPut]
        [Route(Routes.Api.Admin.Products.FlipVisibility)]
        public async Task<IActionResult> FlipVisibility(string id, CancellationToken cancellationToken = default)
        {
            await _mediator.Send(new FlipProductVisibility.Request(id), cancellationToken);
            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Products.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(string id, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            await _mediator.Send(new DeleteProduct.Request(id), token);

            await _mediator.Publish(new ProductDeletedEvent(id), token);

            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Products.DeleteProductCategory)]
        public async Task<IActionResult> DeleteProductCategory(string id, CancellationToken token = default)
        {
            (var productId, var categoryId) = await _mediator.Send(new RemoveProductCategory.Request(id), token);

            await _mediator.Publish(new ProductRemovedFromCategoryEvent(productId, categoryId), token);

            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Products.DeleteState)]
        public async Task<IActionResult> DeleteProductState(string id, CancellationToken token = default)
        {
            await _mediator.Send(new DeleteState.Request(id), token);

            await _mediator.Publish(new ProductStateDeletedEvent(id), token);

            return Ok();
        }

        [HttpDelete]
        [Route(Routes.Api.Admin.Products.DeleteProductImage)]
        public async Task<IActionResult> DeleteImage(string id, CancellationToken token = default)
        {
            if (ServerState.WorkingOnProducts)
                throw new ServiceUnavailableException(Responses.General.WorkingOnProducts);

            await _mediator.Send(new DeleteProductImage.Request(id), token);

            await _mediator.Publish(new ProductImageDeletedEvent(id), token);

            return Ok();
        }
    }
}
