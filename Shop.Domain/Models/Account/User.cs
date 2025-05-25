using Shop.Domain.Models.BaseEntities;
using Shop.Domain.Models.Wallet;
using System.ComponentModel.DataAnnotations;
using Shop.Domain.Models.ProductEntities;

namespace Shop.Domain.Models.Account
{
    public class User:BaseEntity
    {
        #region Properties

        [Display(Name = "آواتار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string UserAvatar { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string LastName { get; set; }
        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(15, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string PhoneNumber { get; set; }
        [Display(Name = "مسدود شده")]
        public bool IsBloacked { get; set; }
        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Password { get; set; }

        [Display(Name = "کد فعالسازی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string MobileActiveCode { get; set; }
        [Display(Name = "وضعیت فعالسازی")]
        public bool IsMobileActive { get; set; }


        [Display(Name = "جنسیت")]
        public UserGender UserGender { get; set; }



        #endregion

        #region Relations
        public ICollection<UserWallet> UserWallets { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        #endregion
    }
    public enum UserGender
    {
        [Display(Name = "آقا")]
        Mail,
        [Display(Name = "خانوم")]
        Femail,
        [Display(Name = "نامشخص")]
        Unknown
    }
}
