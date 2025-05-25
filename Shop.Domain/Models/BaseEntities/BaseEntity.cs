using System.ComponentModel.DataAnnotations;

namespace Shop.Domain.Models.BaseEntities
{
    public class BaseEntity
    {
        #region Properties
        [Key]
        public long Id { get; set; }
        [Display(Name = "وضعیت حذف")]
        public bool IsDelete { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        #endregion

    }
}
