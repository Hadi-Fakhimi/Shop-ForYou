using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Pageing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.ViewModels.Wallet
{
    public class FilterWalletViewModel:BasePageing
    {
        #region Properties
        public long? UserId { get; set; }
        public List<UserWallet> UserWallets { get; set; }
        #endregion
        #region Methods
        public FilterWalletViewModel SetWallets(List<UserWallet> userWallets)
        {
            this.UserWallets = userWallets;
            return this;
        }
        public FilterWalletViewModel SetPageing(BasePageing pageing)
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
