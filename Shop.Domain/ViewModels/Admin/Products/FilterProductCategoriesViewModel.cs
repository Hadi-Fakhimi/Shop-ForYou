using Shop.Domain.Models.Account;
using Shop.Domain.Models.ProductEntities;
using Shop.Domain.ViewModels.Admin.Account;
using Shop.Domain.ViewModels.Pageing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Products
{
    public class FilterProductCategoriesViewModel:BasePageing
    {
        #region Properties
        public string Title { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        #endregion


        #region Methods
        public FilterProductCategoriesViewModel SetProductCategories(List<ProductCategory> productCategory)
        {
            this.ProductCategories = productCategory;
            return this;
        }
        public FilterProductCategoriesViewModel SetPageing(BasePageing pageing)
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
}
