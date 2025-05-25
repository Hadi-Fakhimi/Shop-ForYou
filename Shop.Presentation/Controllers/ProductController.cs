using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;
using Shop.Presentation.Extensions;

namespace Shop.Presentation.Controllers
{
    public class ProductController : SiteBaseController
    {

        #region Constructor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region Product Filter

        [HttpGet("Products")]
        public async Task<IActionResult> Products(FilterProductViewModel filter)
        {
            filter.TakeEntity = 16;
            filter.ProductBox = ProductBox.ItemBoxInSite;
            ViewData["ProductCategories"] = await _productService.GetAllProductCategory();
            var products = await _productService.FilterProducts(filter);

            return View(products);
        }

        #endregion

        #region Product Detail
        [HttpGet("ProductDetail/{productId}")]
        public async Task<IActionResult> ProductDetail(long productId)
        {
            var data = await _productService.ShowProductDetailById(productId);
            var filter = new FilterProductViewModel();
            filter.TakeEntity = 8;
            filter.FilterByCategory = data.ProductCategory.UrlName;

            ViewData["Products"] = await _productService.FilterProducts(filter);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        #endregion

        #region Create Product Comment
        [HttpPost("Add-Comment")]
        public async Task<IActionResult> CreateProductComment(CreateProductCommentViewModel createProductComment)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductComment(createProductComment, User.GetUserId());
                switch (result)
                {
                    case CreateProductCommentResult.CheckProduct:
                        TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case CreateProductCommentResult.CheckUser:
                        TempData[WarningMessage] = "کاربر مورد نظر یافت نشد";
                        break;
                    case CreateProductCommentResult.Success:
                        TempData[SuccessMessage] = "نظر شما با موفقیت ثبت شد";
                        return RedirectToAction(nameof(ProductDetail),new {ProductId = createProductComment.ProductId});
                }
            }

            return View(createProductComment);
        }

        #endregion
    }
}
