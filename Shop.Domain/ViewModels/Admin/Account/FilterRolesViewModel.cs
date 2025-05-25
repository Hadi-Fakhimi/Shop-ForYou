using Shop.Domain.Models.Account;
using Shop.Domain.Models.BaseEntities;
using Shop.Domain.ViewModels.Pageing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Admin.Account
{
    public class FilterRolesViewModel:BasePageing
    {
        #region Properties
        public string RoleName { get; set; }
        public List<Role> Roles { get; set; }
        #endregion


        #region Methods
        public FilterRolesViewModel SetRoles(List<Role> roles)
        {
            this.Roles = roles;
            return this;
        }
        public FilterRolesViewModel SetPageing(BasePageing pageing)
        {
            this.PageId = pageing.PageId;
            this.EndPage = pageing.EndPage;
            this.StartPage = pageing.StartPage;
            this.AllEntityCount = pageing.AllEntityCount;
            this.CountForShowBeforeAndAfter = pageing.CountForShowBeforeAndAfter;
            this.TakeEntity = pageing.TakeEntity;
            this.SkipEntity = pageing.SkipEntity;
            this.PageCount = pageing.PageCount;
            return this;
        }
        #endregion

    }
}
