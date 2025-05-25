using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Domain.Interfaces
{
    public interface IWalletRepositorry
    {
        Task CreateWallet(UserWallet wallet);
        Task<UserWallet> GetUserWalletById(long walletId);
        Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter);
        void UpdateWallet(UserWallet wallet);       
        Task SaveChange();
    }
}
