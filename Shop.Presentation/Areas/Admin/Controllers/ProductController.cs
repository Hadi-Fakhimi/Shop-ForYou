using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Admin.Products;

namespace Shop.Presentation.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        #region Filter - Product
        public async Task<IActionResult> FilterProduct(FilterProductViewModel filterProduct)
        {
            return View(await _productService.FilterProducts(filterProduct));
        }
        #endregion

        #region Create Product
        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            ViewData["Categories"] = await _productService.GetAllProductCategory();

            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductViewModel createProduct,IFormFile productImage)
        {
            ViewData["Categories"] = await _productService.GetAllProductCategory();

            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProduct(createProduct, productImage);

                switch (result)
                {
                    case CreateProductResult.NotImage:
                        TempData[ErrorMessage] = "تصویر برای محصول قرار داده نشده است";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = "عملیات ایجاد محصول با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterProduct));
                }
            }


            return View(createProduct);
        }
        #endregion

        #region Edit Product
        [HttpGet]
        public async Task<IActionResult> EditProduct(long productId)
        {
            ViewData["Categories"] = await _productService.GetAllProductCategory();
            var data = await _productService.GetEditProduct(productId);
            if (data == null) return NotFound();
            return View(data);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductViewModel editProduct , IFormFile? productImage)
        {
            ViewData["Categories"] = await _productService.GetAllProductCategory();
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProduct(editProduct, productImage);
                switch (result)
                {
                    case EditProductResult.NotFuond:
                        TempData[ErrorMessage] = "محصول مورد نظر یافت نشد";
                        break;
                    case EditProductResult.ProductSelectedCategoryHasNull:
                        TempData[WarningMessage] = "لطفا دسته بندی محصول را انتخاب کنید";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterProduct));

                }

            }
            return View(editProduct);
        }
        #endregion

        #region Delete Product
        public async Task<IActionResult> DeleteProduct(long productId)
        {
            var result = await _productService.DeleteProduct(productId);
            if (result)
            {
                TempData[SuccessMessage] = "عملیات حذف محصول با موفقیت انجام شد";
            }
            else
            {
                TempData[ErrorMessage] = "در انجام عملیات حذف خطایی رخ داده است";
            }
            return RedirectToAction(nameof(FilterProduct));
        }
        #endregion

        #region Restore Product
        public async Task<IActionResult> RestoreProduct(long productId)
        {
            var result = await _productService.RestoreProduct(productId);
            if (result)
            {
                TempData[SuccessMessage] = "عملیات بازگردانی محصول با موفقیت انجام شد";
            }
            else
            {
                TempData[ErrorMessage] = "در انجام عملیات بازگردانی خطایی رخ داده است";
            }
            return RedirectToAction(nameof(FilterProduct));
        }
        #endregion

        #region Create Product Galleries
        public IActionResult GalleryProduct(long productId)
        {
            ViewBag.ProductId = productId;  
            return View();
        }
        public async Task<IActionResult> AddImageToProduct(List<IFormFile> images , long productId)
        {
            var result = await _productService.AddProductGallery(productId, images);
            if (result)
            {
                return new JsonResult(new { status = "Success" });
            }
            else
            {
                return new JsonResult(new { status = "Error" });
            }
            return RedirectToAction(nameof(FilterProduct));
        }
        #endregion

        #region Gat Product Gallery List
        public async Task<IActionResult> ProductGalleries(long productId)
        {
            var data = await _productService.GetAllProductGalleries(productId);
            if (data == null) return NotFound();
            return View(data);


        }
        #endregion

        #region Delete Product Galleries
        public async Task<IActionResult> DeleteImageGallery(long galleryId)
        {
            await _productService.DeleteImage(galleryId);
            TempData[SuccessMessage] = "عملیات حذف تصویر با موفقیت انجام شد";
            return RedirectToAction(nameof(FilterProduct));
        }

        #endregion

        #region Filter Product Category
        public async Task<IActionResult> FilterProductCategories(FilterProductCategoriesViewModel filterProductCategories)
        {
            return View(await _productService.FilterProductCategories(filterProductCategories));
        }
        #endregion

        #region Create Product Category
        [HttpGet]
        public IActionResult CreateProductCategory()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductCategory(CreateProductCategoryViewModel createProductCategory ,IFormFile image)
        {
  
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductCategory(createProductCategory , image);

                switch (result)
                {
                    case CreateProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "عنوان url تکراری است";
                        break;
                    case CreateProductCategoryResult.Success:
                        TempData[SuccessMessage] = "عملیات ایجاد دسته بندی با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterProductCategories));

                }
            }

            return View(createProductCategory);
        }
        #endregion

        #region Edit Product Category
        [HttpGet]
        public async Task<IActionResult> EditProductCategory(long productCategoryId)
        {
            var productCategory = await _productService.GetEditProductCategory(productCategoryId);
            if(productCategory == null)
            {
                return NotFound();
            }
            return View(productCategory);

        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProductCategory(EditProductCategoryViewModel editProductCategory,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.EditProductCategory(editProductCategory,image);
                switch (result)
                {
                    case EditProductCategoryResult.IsExistUrlName:
                        TempData[WarningMessage] = "عنوان url تکراری است";
                        break;
                    case EditProductCategoryResult.NotFound:
                        TempData[ErrorMessage] = "دسته بندی با مشخصات مورد نظر یافت نشد";
                        break;
                    case EditProductCategoryResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterProductCategories));
                }
            }
            return View(editProductCategory);
        }
        #endregion

        #region Create Product Featuers
        [HttpGet]
        public IActionResult CreateProductFeatuer(long productId)
        {
            var createProductFeatuers = new CreateProductFeatuersViewModel()
            {
                ProductId = productId
            };

            return View(createProductFeatuers);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProductFeatuer(CreateProductFeatuersViewModel createProductFeatuers)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductFeatuers(createProductFeatuers);
                switch (result)
                {
                    case CreateProductFeatuersResult.Error:
                        TempData[SuccessMessage] = "عملیات افزودن با شکست مواجه شد";
                        break;
                    case CreateProductFeatuersResult.Success:
                        TempData[SuccessMessage] = "عملیات افزودن با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterProduct));
                }
            }
            return View(createProductFeatuers);
        }
        #endregion

        #region Product Featuers
        public async Task<IActionResult> ProductFeatuers(long productId)
        {
            var data = await _productService.GetAllProductFeature(productId);
            if (data == null) return NotFound();
            return View(data);
        }
        #endregion

        #region Delete Product Featuer
        public async Task<IActionResult> DeleteProductFeatuer(long productFeatuerId)
        {
            await _productService.DeleteProductFeatures(productFeatuerId);
            TempData[SuccessMessage] = "عملیات حذف ویژگی با موفقیت انجام شد";
            return RedirectToAction(nameof(FilterProduct));
        }
        #endregion
    }
}
