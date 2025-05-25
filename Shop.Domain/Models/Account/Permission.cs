using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Account
{
    public class Permission:BaseEntity
    {
        #region Properties
        [Display(Name = "عنوان دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }
        public long? ParentId { get; set; }

        #endregion
        #region Relations
        [ForeignKey("ParentId")]
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

        #endregion
    }
}
