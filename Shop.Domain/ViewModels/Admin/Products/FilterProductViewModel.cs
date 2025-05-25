using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Pageing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.ViewModels.Site.Products;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class FilterProductViewModel:BasePageing
    {
        #region Properties
        public string ProductName { get; set; }
        public string FilterByCategory { get; set; }
        public ProductState ProductState { get; set; }
        public ProductOrder ProductOrder { get; set; }
        public ProductBox ProductBox { get; set; }
        public List<Product> Products { get; set; }
        public List<ProductItemViewModel> ProductItem { get; set; }
        #endregion


        #region Methods
        public FilterProductViewModel SetProduct(List<Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductViewModel SetProductBox(List<ProductItemViewModel> productItem)
        {
            this.ProductItem = productItem;
            return this;
        }
        public FilterProductViewModel SetPageing(BasePageing pageing)
        {
            this.PageId = pageing.PageId;
            this.EndPage = pageing.EndPage;
            this.StartPage = pageing.StartPage;
            this.AllEntityCount = pageing.AllEntityCount;
            this.CountForShowBeforeAndAfter = pageing.CountForShowBeforeAndAfter;
            this.TakeEntity = pageing.TakeEntity;
            this.SkipEntity = pageing.SkipEntity;
            this.PageCount = pageing.PageCount;
            return this;
        }
        #endregion
    }
    public enum ProductState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        IsActive,
        [Display(Name = "حذف شده")]
        Delete
    }
    public enum ProductOrder
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "جدید")]
        ProductNewss,
        [Display(Name = "گران ترین")]
        ProductExpensive,
        [Display(Name = "ارزان ترین")]
        ProductCheep
    }

    public enum ProductBox
    {
        Default,
        ItemBoxInSite
    }

}
