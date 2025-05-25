using Microsoft.AspNetCore.Http;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Application.Services.Interfaces
{
    public interface IProductService
    {
        #region Product - Admin

        #region Product Categories
        Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createProductCategory, IFormFile image);
        Task<EditProductCategoryViewModel> GetEditProductCategory(long categoryId);
        Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image);
        Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filterProductCategories);
        Task<List<ProductCategory>> GetAllProductCategory();
        #endregion

        #region Product
        Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filterProducts);
        Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct, IFormFile image);
        Task<EditProductViewModel> GetEditProduct(long productId);
        Task<EditProductResult> EditProduct(EditProductViewModel editProduct, IFormFile image);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RestoreProduct(long productId);
        #endregion

        #region Product Gallery
        Task<bool> AddProductGallery(long productId, List<IFormFile> images);
        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task DeleteImage(long galleryId);
        #endregion

        #region Product Fetuers
        Task<CreateProductFeatuersResult> CreateProductFeatuers(CreateProductFeatuersViewModel createProductFeatuers);
        Task<List<ProductFeature>> GetAllProductFeature(long productId);
        Task DeleteProductFeatures(long id);

        #endregion


        #endregion

        #region Product - Site

        #region Product Item

        Task<List<ProductItemViewModel>> ShowAllProductInSlider();
        Task<List<ProductItemViewModel>> ShowLastProductInSlider();

        #endregion
        #region Product Detail

        Task<ProductDetailViewModel> ShowProductDetailById(long productId);

        #endregion

        #region ProductComment

        Task<CreateProductCommentResult> CreateProductComment(CreateProductCommentViewModel createProductComment,
            long userId);

        #endregion

        #endregion
    }
}
