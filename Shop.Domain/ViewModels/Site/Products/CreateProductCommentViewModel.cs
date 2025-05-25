using Shop.Domain.Models.ProductEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Site.Products
{
    public class CreateProductCommentViewModel
    {
        public long ProductId { get; set; }
        [Display(Name = "عنوان نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Subject { get; set; }
        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string CommentText { get; set; }
        [Display(Name = "کیفیت ساخت")]
        public ProductRating BuildQuality { get; set; } = ProductRating.Excellent;
        [Display(Name = "ارزش خرید به نسبت قیمت")]
        public ProductRating ValueForMoney { get; set; } = ProductRating.Excellent;
        [Display(Name = "نوآوری")]
        public ProductRating Innovation { get; set; } = ProductRating.Excellent;
        [Display(Name = "امکانات و قابلیت ها")]
        public ProductRating FeaturesAndCapabilities { get; set; } = ProductRating.Excellent;
        [Display(Name = "سهولت استفاده")]
        public ProductRating EaseOfUse { get; set; } = ProductRating.Excellent;
    }

    public enum CreateProductCommentResult
    {
        CheckUser,
        CheckProduct,
        Success
    }
}
