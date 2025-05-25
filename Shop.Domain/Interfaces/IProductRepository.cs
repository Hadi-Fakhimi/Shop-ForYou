using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IProductRepository
    {
        #region Product - Admin

        #region Product Category
        Task<bool> CheckUrlNameCategories(string urlName);
        Task<bool> CheckUrlNameCategories(string urlName, long categoryId);
        Task AddProductCategory(ProductCategory productCategory);
        Task<ProductCategory> GetProductCategoryById(long productCategoryId);
        void UpdateProductCategory(ProductCategory productCategory);
        Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filterProductCategories);
        Task<List<ProductCategory>> GetAllProductCategory();
        Task<List<long>> GetAllProductCategoryId(long productId);
        #endregion

        #region Product
        Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filterProducts);
        Task AddProduct(Product product);
        Task RemoveProductSelectedCategories(long productId);
        Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId);
        Task<Product> GetProductById(long productId);
        void UpdateProduct(Product product);
        Task<bool> DeleteProduct(long productId);
        Task<bool> RestoreProduct(long productId);
        Task<bool> CheckProduct(long productId);
        #endregion

        #region Product Gallery
        Task AddProductGalleries(List<ProductGalleries> productGalleries);
        Task<List<ProductGalleries>> GetAllProductGalleries(long productId);
        Task<ProductGalleries> GetProductGalleriesById(long galleryId);
        Task DeleteProductGalleries(ProductGalleries productGalleries);

        #endregion

        #region Product Features
        Task AddProductFeatures(ProductFeature feature);
        Task<List<ProductFeature>> GetAllProductFeature(long productId);
        Task DeleteProductFeatures(long id);
        #endregion

        Task SaveChange();
        #endregion

        #region Product - Site

        #region Product Item

        Task<List<ProductItemViewModel>> ShowAllProductInSlider();
        Task<List<ProductItemViewModel>> ShowLastProductInSlider();

        #endregion

        #region Product Detail

        Task<ProductDetailViewModel> ShowProductDetailById(long productId);

        #endregion

        #region Product Comment

        Task AddProductComment(ProductComment productComment);

        #endregion

        #endregion
    }
}
