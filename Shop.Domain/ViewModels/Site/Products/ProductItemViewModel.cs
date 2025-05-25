using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.ProductEntities;

namespace Shop.Domain.ViewModels.Site.Products
{
    public class ProductItemViewModel
    {
        #region Properies

        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }
        public string ProductImageName { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public int CountComment { get; set; }


        #endregion
    }
}
