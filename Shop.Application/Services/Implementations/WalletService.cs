using Shop.Application.Services.Interfaces;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;

namespace Shop.Application.Services.Implementations
{
    public class WalletService : IWalletService
    {
        #region Constructor
        private readonly IWalletRepositorry _walletRepository;
        private readonly IUserRepository _userRepository;
        public WalletService(IWalletRepositorry walletRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _walletRepository = walletRepository;
        }
        #endregion

        public async Task<long> ChargeWallet(long userId, ChargeWalletViewModel chargeWallet, string description)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return 0;
            }
            var wallet = new UserWallet
            {
                UserId = userId,
                Amount = chargeWallet.Amount,
                Description = description,
                IsDelete = false,
                IsPay = false,
                WalletType = WalletType.Variz

            };
            await _walletRepository.CreateWallet(wallet);
            await _walletRepository.SaveChange();
            return wallet.Id;
        }

        public async Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter)
        {
            return await _walletRepository.FilterWallet(filter);
        }

        public async Task<UserWallet> GetUserWalletById(long walletId)
        {
            return await _walletRepository.GetUserWalletById(walletId);
        }

        public async Task<bool> UpdateWalletForCharge(UserWallet wallet)
        {
            if (wallet != null)
            {
                wallet.IsPay = true;
                _walletRepository.UpdateWallet(wallet);
                await _walletRepository.SaveChange();
                return true;

            }
            return false;
        }
    }
}
