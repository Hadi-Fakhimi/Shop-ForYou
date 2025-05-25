using Microsoft.AspNetCore.Http;
using Shop.Application.Extentions;
using Shop.Application.Generator;
using Shop.Application.Services.Interfaces;
using Shop.Application.StaticTools;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Constructor
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        public ProductService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }


        #endregion

        #region Product - Admin

        #region Product Categories
        public async Task<CreateProductCategoryResult> CreateProductCategory(CreateProductCategoryViewModel createProductCategory, IFormFile image)
        {
            if (await _productRepository.CheckUrlNameCategories(createProductCategory.UrlName)) return CreateProductCategoryResult.IsExistUrlName;

            var newCategory = new ProductCategory()
            {
                Title = createProductCategory.Title,
                UrlName = createProductCategory.UrlName,
                ParentId = null,
                IsDelete = false
            };
            if(image != null && image.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(image.FileName);
                image.AddImageToServer(imageName, PathsExtensions.ImageCategoryOriginServer, 150, 150, PathsExtensions.ImageCategoryThumbServer);

                newCategory.ImageName = imageName;
            }

            await _productRepository.AddProductCategory(newCategory);
            await _productRepository.SaveChange();
            return CreateProductCategoryResult.Success;
        }

        public async Task<EditProductCategoryResult> EditProductCategory(EditProductCategoryViewModel editProductCategory, IFormFile image)
        {
            var productCategory = await _productRepository.GetProductCategoryById(editProductCategory.ProductCategoryId);
            if(productCategory == null)
            {
                return EditProductCategoryResult.NotFound;
            }

            if(await _productRepository.CheckUrlNameCategories(editProductCategory.UrlName, editProductCategory.ProductCategoryId))
            {
                return EditProductCategoryResult.IsExistUrlName;
            }
            productCategory.UrlName = editProductCategory.UrlName;
            productCategory.Title = editProductCategory.Title;

            if (image != null && image.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(image.FileName);
                image.AddImageToServer(imageName, PathsExtensions.ImageCategoryOriginServer, 150, 150, PathsExtensions.ImageCategoryThumbServer,productCategory.ImageName);

                productCategory.ImageName = imageName;
            }

            _productRepository.UpdateProductCategory(productCategory);
            await _productRepository.SaveChange();

            return EditProductCategoryResult.Success;

        }

        public async Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filterProductCategories)
        {
            return await _productRepository.FilterProductCategories(filterProductCategories);
        }



        public async Task<EditProductCategoryViewModel> GetEditProductCategory(long categoryId)
        {
            var productCategory = await _productRepository.GetProductCategoryById(categoryId);
            if (productCategory != null)
            {
                return new EditProductCategoryViewModel()
                {
                    ImageName = productCategory.ImageName,
                    ProductCategoryId = productCategory.Id,
                    Title = productCategory.Title,
                    UrlName = productCategory.UrlName
                };
            }
            return null;
        }

        public async Task<List<ProductCategory>> GetAllProductCategory()
        {
            return await _productRepository.GetAllProductCategory();
        }
        #endregion

        #region Product
        public async Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filterProducts)
        {
            return await _productRepository.FilterProducts(filterProducts);    
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductViewModel createProduct,IFormFile image)
        {
            var newProduct = new Product()
            {
                Description = createProduct.Description,
                ProductName = createProduct.ProductName,
                Price = createProduct.Price,
                IsActive = createProduct.IsActive,
                ShortDescription = createProduct.ShortDescription

            };

            if(image != null && image.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(image.FileName);
                image.AddImageToServer(imageName, PathsExtensions.ImageProductOriginServer, 150, 150, PathsExtensions.ImageProductThumbServer);

                newProduct.ProductImageName = imageName;
            }
            else
            {
                return CreateProductResult.NotImage;
            }

            await _productRepository.AddProduct(newProduct);
            await _productRepository.SaveChange();

            await _productRepository.AddProductSelectedCategories(createProduct.ProductSelectedCategory,newProduct.Id);


            return CreateProductResult.Success;

        }

        public async Task<EditProductViewModel> GetEditProduct(long productId)
        {
            var currentProduct = await _productRepository.GetProductById(productId);
            if(currentProduct!= null)
            {
                return new EditProductViewModel()
                {
                    ProductName = currentProduct.ProductName,
                    Description = currentProduct.Description,
                    ImageName = currentProduct.ProductImageName,
                    IsActive = currentProduct.IsActive,
                    ShortDescription = currentProduct.ShortDescription,
                    Price = currentProduct.Price,
                    ProductId = productId,
                    ProductSelectedCategory = await _productRepository.GetAllProductCategoryId(productId)
                };


            }

            return null;
        }

        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct, IFormFile image)
        {
            var currentProduct = await _productRepository.GetProductById(editProduct.ProductId);
            if (currentProduct == null) return EditProductResult.NotFuond;
            if (editProduct.ProductSelectedCategory == null) return EditProductResult.ProductSelectedCategoryHasNull;

            #region edit product
            currentProduct.IsActive = editProduct.IsActive;
            currentProduct.Description = editProduct.Description;
            currentProduct.Price = editProduct.Price;
            currentProduct.ShortDescription = editProduct.ShortDescription;
            currentProduct.ProductName = editProduct.ProductName;
            #endregion

            if (image != null && image.IsImage())
            {

                var imageName = CodeGenerator.GenerateFileName(image.FileName);
                image.AddImageToServer(imageName, PathsExtensions.ImageProductOriginServer, 150, 150, PathsExtensions.ImageProductThumbServer, currentProduct.ProductImageName);

                currentProduct.ProductImageName = imageName;
            }
            _productRepository.UpdateProduct(currentProduct);
            await _productRepository.SaveChange();



            await _productRepository.RemoveProductSelectedCategories(editProduct.ProductId);
            await _productRepository.AddProductSelectedCategories(editProduct.ProductSelectedCategory,editProduct.ProductId);
            return EditProductResult.Success;
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }

        public async Task<bool> RestoreProduct(long productId)
        {
            return await _productRepository.RestoreProduct(productId);
        }




        #endregion

        #region Product Gallery
        public async Task<bool> AddProductGallery(long productId, List<IFormFile> images)
        {
            if(!await _productRepository.CheckProduct(productId)) return false;
            var productGalleries = new List<ProductGalleries>();
            if (images != null && images.Any())
            {
                foreach(var image in images)
                {
                    if (image.IsImage())
                    {
                        var imageName = CodeGenerator.GenerateFileName(image.FileName);
                        image.AddImageToServer(imageName, PathsExtensions.ImageProductOriginServer, 150, 150, PathsExtensions.ImageProductThumbServer);

                        productGalleries.Add(new ProductGalleries()
                        {
                            ImageName = imageName,
                            ProductId = productId
                        });

                    }

                }

                await _productRepository.AddProductGalleries(productGalleries);

                return true;
            }

            return false;
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _productRepository.GetAllProductGalleries(productId);
        }

        public async Task DeleteImage(long galleryId)
        {
            var productGallery = await _productRepository.GetProductGalleriesById(galleryId);
            if (productGallery != null)
            {

                UploadImageExtension.DeleteImage(productGallery.ImageName, PathsExtensions.ImageProductOriginServer, PathsExtensions.ImageProductThumbServer);
                await _productRepository.DeleteProductGalleries(productGallery);


            }
        }


        #endregion

        #region Product Featuers
        public async Task<CreateProductFeatuersResult> CreateProductFeatuers(CreateProductFeatuersViewModel createProductFeatuers)
        {
            if(!await _productRepository.CheckProduct(createProductFeatuers.ProductId))
            {
                return CreateProductFeatuersResult.Error;
            }
            var newFearures = new ProductFeature()
            {
                FeatureTitle = createProductFeatuers.Title,
                FeatureValue = createProductFeatuers.Value,
                ProductId = createProductFeatuers.ProductId
            };

            await _productRepository.AddProductFeatures(newFearures);
            await _productRepository.SaveChange();
            return CreateProductFeatuersResult.Success;
        }
        public async Task<List<ProductFeature>> GetAllProductFeature(long productId)
        {
            return await _productRepository.GetAllProductFeature(productId);
        }

        public async Task DeleteProductFeatures(long id)
        {
            await _productRepository.DeleteProductFeatures(id);
        }

        #endregion


        #endregion

        #region Product Site

        #region Product Item

        public async Task<List<ProductItemViewModel>> ShowAllProductInSlider()
        {
            return await _productRepository.ShowAllProductInSlider();
        }

        public async Task<List<ProductItemViewModel>> ShowLastProductInSlider()
        {
            return await _productRepository.ShowLastProductInSlider();
        }

        #endregion

        #region Product Detail

        public async Task<ProductDetailViewModel> ShowProductDetailById(long productId)
        {
            return await _productRepository.ShowProductDetailById(productId);
        }

        #endregion

        #region ProductComment

        public async Task<CreateProductCommentResult> CreateProductComment(
            CreateProductCommentViewModel createProductComment,
            long userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return CreateProductCommentResult.CheckUser;
            }

            if (!await _productRepository.CheckProduct(createProductComment.ProductId))
            {
                return CreateProductCommentResult.CheckProduct;
            }

            var createComment = new ProductComment()
            {
                ProductId = createProductComment.ProductId,
                UserId = userId,
                CommentText = createProductComment.CommentText,
                Subject = createProductComment.Subject,
                BuildQuality = createProductComment.BuildQuality,
                EaseOfUse = createProductComment.EaseOfUse,
                FeaturesAndCapabilities = createProductComment.FeaturesAndCapabilities,
                Innovation = createProductComment.Innovation,
                ValueForMoney = createProductComment.ValueForMoney
            };

            await _productRepository.AddProductComment(createComment);
            await _productRepository.SaveChange();
            return CreateProductCommentResult.Success;
        }

        #endregion


        #endregion
    }
}
