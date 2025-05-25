using Shop.Domain.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Models.Account
{
    public class RolePermission:BaseEntity
    {
        #region Properties
        public long RoleId { get; set; }
        public long PermissionId { get; set; }
        #endregion

        #region Relations
        public Role Role { get; set; }
        public Permission Permission { get; set; }
        #endregion
    }
}
