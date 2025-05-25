using Shop.Domain.Models.Account;
using Shop.Domain.ViewModels.Pageing;

namespace Shop.Domain.ViewModels.Admin.Account
{
    public class FilterUserViewModel:BasePageing
    {
        #region Properties
        public string PhoneNumber { get; set; }
        public List<User> Users { get; set; }
        #endregion

        #region Methods
        public FilterUserViewModel SetUsers(List<User> users)
        {
            this.Users = users;
            return this;
        }
        public FilterUserViewModel SetPageing(BasePageing pageing)
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
