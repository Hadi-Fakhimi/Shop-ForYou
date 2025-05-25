using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.ProductEntities
{
    public class ProductSelectedCategories:BaseEntity
    {
        #region Properties
        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }
        #endregion
        #region Relations
        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }
        #endregion
    }
}
