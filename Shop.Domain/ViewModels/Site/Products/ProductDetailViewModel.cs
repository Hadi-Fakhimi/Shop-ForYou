using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.ProductEntities;

namespace Shop.Domain.ViewModels.Site.Products
{
    public class ProductDetailViewModel
    {
        #region Properties

        public long ProductId { get; set; }
        [Display(Name = "نام محصول")]
        public string ProductName { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "قیمت")]
        public int Price { get; set; }

        [Display(Name = "تصویر محصول")]
        public string ProductImageName { get; set; }
        [Display(Name = "نظرات محصول")]
        public int ProductComment { get; set; }
        [Display(Name = "گالری محصول")]
        public List<string> ProductImages { get; set; }
        [Display(Name = "دسته بندی محصول")]
        public ProductCategory ProductCategory { get; set; }
        [Display(Name = "ویژگی های محصول")]
        public List<ProductFeature> ProductFeatures { get; set; }

        [Display(Name = "فعال / غیر فعال")]
        public bool IsActive { get; set; }

        #endregion
    }
}
