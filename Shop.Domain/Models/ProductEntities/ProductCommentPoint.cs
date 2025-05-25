using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.BaseEntities;

namespace Shop.Domain.Models.ProductEntities
{
    public class ProductCommentPoint:BaseEntity
    {

        #region Properties
        public long ProductCommentId { get; set; }
        [Display(Name = "عنوان نقاط قوت یا ضعف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }
        [Display(Name = "قوت/ ضعف")]
        public PointType Type { get; set; }

        #endregion

        #region Relations

        public ProductComment ProductComment { get; set; }

        #endregion

    }

    public enum PointType
    {
        [Display(Name = "نقاط قوت")]
        Strength,
        [Display(Name = "نقاط ضعف")]
        Weakness
    }
}
