using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntities
{
    public class ProductComment:BaseEntity
    {
        #region Properties

        public long ProductId { get; set; }
        public long UserId { get; set; }
        [Display(Name = "عنوان نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Subject { get; set; }
        [Display(Name = "متن نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string CommentText { get; set; }
        [Display(Name = "کیفیت ساخت")]
        public ProductRating BuildQuality { get; set; }
        [Display(Name = "ارزش خرید به نسبت قیمت")]
        public ProductRating ValueForMoney { get; set; }
        [Display(Name = "نوآوری")]
        public ProductRating Innovation { get; set; }
        [Display(Name = "امکانات و قابلیت ها")]
        public ProductRating FeaturesAndCapabilities { get; set; }
        [Display(Name = "سهولت استفاده")]
        public ProductRating EaseOfUse { get; set; }

        #endregion

        #region Relations

        public Product Product { get; set; }
        public User User { get; set; }

        public ICollection<ProductCommentPoint> ProductCommentPoints { get; set; }


        #endregion

    }

    public enum ProductRating
    {
        [Display(Name = "خیلی بد")]
        VeryBad,

        [Display(Name = "بد")]
        Bad,

        [Display(Name = "معمولی")]
        Average,

        [Display(Name = "خوب")]
        Good,

        [Display(Name = "عالی")]
        Excellent
    }
}
