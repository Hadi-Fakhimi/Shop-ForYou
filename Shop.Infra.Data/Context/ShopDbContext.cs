using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.Models.Site;
using Shop.Domain.Models.Wallet;

namespace Shop.Infra.Data.Context
{
    public class ShopDbContext:DbContext
    {
        #region Constructor
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {

        }
        #endregion

        #region User
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        #endregion

        #region Wallet
        public DbSet<UserWallet> UserWallets { get; set; }
        #endregion

        #region Product
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductFeature> ProductFeatures { get; set; }
        public DbSet<ProductGalleries> ProductGalleries { get; set; }
        public DbSet<ProductSelectedCategories> ProductSelectedCategories { get; set; }
        public DbSet<ProductComment> ProductComments { get; set; }
        public DbSet<ProductCommentPoint> ProductCommentPoints { get; set; }

        #endregion

        #region Slider

        public DbSet<Slider> Sliders { get; set; }

        #endregion
    }
}
