using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Pageing;
using Shop.Domain.ViewModels.Site.Products;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Constructor
        private readonly ShopDbContext _context;
        public ProductRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Product - Admin

        #region Product Category

        public async Task<bool> CheckUrlNameCategories(string urlName)
        {
            return await _context.ProductCategories.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName);
        }
        public async Task AddProductCategory(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
        }
        public Task<ProductCategory> GetProductCategoryById(long productCategoryId)
        {
            return _context.ProductCategories.AsQueryable().SingleOrDefaultAsync(c => c.Id == productCategoryId);
        }

        public async Task<bool> CheckUrlNameCategories(string urlName, long categoryId)
        {
            return await _context.ProductCategories.AsQueryable()
                .AnyAsync(c => c.UrlName == urlName && c.Id != categoryId);
        }

        public void UpdateProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Update(productCategory);
        }

        public async Task<FilterProductCategoriesViewModel> FilterProductCategories(FilterProductCategoriesViewModel filterProductCategories)
        {
            var query = _context.ProductCategories.AsQueryable();

            #region Filter
            if (!string.IsNullOrEmpty(filterProductCategories.Title))
            {
                query = query.Where(c => EF.Functions.Like(c.Title, $"%{filterProductCategories.Title}%"));
            }
            #endregion

            #region Pageing
            var pager = Pager.Build(filterProductCategories.PageId, await query.CountAsync(), filterProductCategories.TakeEntity, filterProductCategories.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filterProductCategories.SetPageing(pager).SetProductCategories(allData);


        }


        public async Task<List<ProductCategory>> GetAllProductCategory()
        {
            return await _context.ProductCategories.AsQueryable().Where(c => !c.IsDelete).ToListAsync();
        }

        public async Task<List<long>> GetAllProductCategoryId(long productId)
        {
            return await _context.ProductSelectedCategories.AsQueryable()
                .Where(c => !c.IsDelete && c.ProductId == productId).Select(c => c.Id).ToListAsync();
        }
        #endregion

        #region Product
        public async Task<FilterProductViewModel> FilterProducts(FilterProductViewModel filterProducts)
        {

            var query = _context.Products.Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory).AsQueryable();

            #region Filter
            if (!string.IsNullOrEmpty(filterProducts.ProductName))
            {
                query = query.Where(c => EF.Functions.Like(c.ProductName, $"%{filterProducts.ProductName}%"));
            }
            if (!string.IsNullOrEmpty(filterProducts.FilterByCategory))
            {
                query = query.Where(c => c.ProductSelectedCategories.Any(s => s.ProductCategory.UrlName == filterProducts.FilterByCategory));
            }
            #endregion

            #region States
            switch (filterProducts.ProductState)
            {
                case ProductState.All:
                    break;
                case ProductState.IsActive:
                    query = query.Where(p => p.IsActive);
                    break;
                case ProductState.Delete:
                    query = query.Where(p => p.IsDelete);
                    break;

            }

            #endregion

            #region Order

            switch (filterProducts.ProductOrder)
            {
                case ProductOrder.All:
                    break;
                case ProductOrder.ProductNewss:
                    query = query.Where(p => p.IsActive).OrderByDescending(p => p.CreateDate);
                    break;
                case ProductOrder.ProductExpensive:
                    query = query.Where(p => p.IsActive).OrderByDescending(p => p.Price);
                    break;
                case ProductOrder.ProductCheep:
                    query = query.Where(p => p.IsActive).OrderBy(p => p.Price);
                    break;

            }

            #region Product Box

            switch (filterProducts.ProductBox)
            {
                case ProductBox.Default:
                    break;
                case ProductBox.ItemBoxInSite:
                    var pagerBox = Pager.Build(filterProducts.PageId, await query.CountAsync(), filterProducts.TakeEntity, filterProducts.CountForShowBeforeAndAfter);

                    var allDataBox = await query.Pageing(pagerBox).Select(c => new ProductItemViewModel()
                    {
                        ProductCategory = c.ProductSelectedCategories.Select(b => b.ProductCategory).First(),
                        CountComment = 0,
                        Price = c.Price,
                        ProductId = c.Id,
                        ProductImageName = c.ProductImageName,
                        ProductName = c.ProductName
                    }).ToListAsync();

                    return filterProducts.SetPageing(pagerBox).SetProductBox(allDataBox);
            }

            #endregion

            #endregion

            #region Pageing
            var pager = Pager.Build(filterProducts.PageId, await query.CountAsync(), filterProducts.TakeEntity, filterProducts.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filterProducts.SetPageing(pager).SetProduct(allData);
        }

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task RemoveProductSelectedCategories(long productId)
        {
            var allProductSelectedCategories = await _context.ProductSelectedCategories.AsQueryable()
                .Where(p => p.ProductId == productId).ToListAsync();

            if (allProductSelectedCategories != null && allProductSelectedCategories.Any())
            {

                _context.ProductSelectedCategories.RemoveRange(allProductSelectedCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddProductSelectedCategories(List<long> productSelectedCategories, long productId)
        {
            if (productSelectedCategories != null && productSelectedCategories.Any())
            {
                var newProductSelectedCategories = new List<ProductSelectedCategories>();

                foreach (var CategoryId in productSelectedCategories)
                {
                    newProductSelectedCategories.Add(new ProductSelectedCategories()
                    {
                        ProductCategoryId = CategoryId,
                        ProductId = productId
                    });

                }

                await _context.ProductSelectedCategories.AddRangeAsync(newProductSelectedCategories);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<Product> GetProductById(long productId)
        {
            return await _context.Products.AsQueryable().SingleOrDefaultAsync(p => p.Id == productId);
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
        }

        public async Task<bool> DeleteProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable()
                .Where(p => p.Id == productId).FirstOrDefaultAsync();

            if (currentProduct != null)
            {
                currentProduct.IsDelete = true;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> RestoreProduct(long productId)
        {
            var currentProduct = await _context.Products.AsQueryable()
                .Where(p => p.Id == productId).FirstOrDefaultAsync();

            if (currentProduct != null)
            {
                currentProduct.IsDelete = false;
                _context.Products.Update(currentProduct);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> CheckProduct(long productId)
        {
            return await _context.Products.AsQueryable()
                .AnyAsync(p => p.Id == productId);
        }



        #endregion

        #region Product Gallery
        public async Task AddProductGalleries(List<ProductGalleries> productGalleries)
        {
            await _context.ProductGalleries.AddRangeAsync(productGalleries);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductGalleries>> GetAllProductGalleries(long productId)
        {
            return await _context.ProductGalleries.AsQueryable()
                .Where(p => p.ProductId == productId && !p.IsDelete).ToListAsync();
        }

        public async Task<ProductGalleries> GetProductGalleriesById(long galleryId)
        {
            return await _context.ProductGalleries.AsQueryable()
                .Where(c => c.Id == galleryId).FirstOrDefaultAsync();
        }

        public async Task DeleteProductGalleries(ProductGalleries productGalleries)
        {
            if (productGalleries != null)
            {
                productGalleries.IsDelete = true;

                _context.ProductGalleries.Update(productGalleries);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

        #region Product Features
        public async Task AddProductFeatures(ProductFeature feature)
        {
            await _context.ProductFeatures.AddAsync(feature);
        }
        public async Task<List<ProductFeature>> GetAllProductFeature(long productId)
        {
            return await _context.ProductFeatures.AsQueryable()
                .Where(p => p.ProductId == productId && !p.IsDelete).ToListAsync();
        }
        public async Task DeleteProductFeatures(long id)
        {
            var currentFeatuer = await _context.ProductFeatures.AsQueryable()
                .Where(p => p.Id == id).SingleOrDefaultAsync();

            if (currentFeatuer != null)
            {
                currentFeatuer.IsDelete = true;
                _context.ProductFeatures.Update(currentFeatuer);
                await _context.SaveChangesAsync();
            }
        }
        #endregion
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region Product - Site

        #region Product Item

        public async Task<List<ProductItemViewModel>> ShowAllProductInSlider()
        {
            return await _context.Products.Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .AsQueryable()
                .Select(c => new ProductItemViewModel()
                {
                    ProductCategory = c.ProductSelectedCategories.Select(b => b.ProductCategory).First(),
                    CountComment = 0,
                    Price = c.Price,
                    ProductId = c.Id,
                    ProductImageName = c.ProductImageName,
                    ProductName = c.ProductName
                }).ToListAsync();
        }

        public async Task<List<ProductItemViewModel>> ShowLastProductInSlider()
        {
            return await _context.Products.Include(c => c.ProductSelectedCategories)
                .ThenInclude(c => c.ProductCategory)
                .AsQueryable().OrderByDescending(c => c.CreateDate)
                .Select(c => new ProductItemViewModel()
                {
                    ProductCategory = c.ProductSelectedCategories.Select(b => b.ProductCategory).First(),
                    CountComment = 0,
                    Price = c.Price,
                    ProductId = c.Id,
                    ProductImageName = c.ProductImageName,
                    ProductName = c.ProductName
                }).Take(8).ToListAsync();
        }

        #endregion

        #region Product Detail

        public async Task<ProductDetailViewModel> ShowProductDetailById(long productId)
        {
            return await _context.Products.Include(c => c.ProductSelectedCategories).ThenInclude(c => c.ProductCategory)
                .Include(c => c.ProductFeatures).Include(c => c.ProductGalleries)
                .Where(c => c.Id.Equals(productId))
                .AsQueryable().Select(c => new ProductDetailViewModel()
                {
                    ProductImageName = c.ProductImageName,
                    ProductName = c.ProductName,
                    Price = c.Price,
                    ProductCategory = c.ProductSelectedCategories.Select(b => b.ProductCategory).First(),
                    Description = c.Description,
                    ProductComment = 1,
                    ProductFeatures = c.ProductFeatures.ToList(),
                    ProductId = c.Id,
                    ProductImages = c.ProductGalleries.Where(c => !c.IsDelete).Select(b => b.ImageName).ToList(),
                    ShortDescription = c.ShortDescription,
                    IsActive = c.IsActive

                }).FirstOrDefaultAsync();
        }

        #endregion

        #region Product Comment

        public async Task AddProductComment(ProductComment productComment)
        {
            await _context.ProductComments.AddAsync(productComment);
        }

        #endregion

        #endregion
    }
}
