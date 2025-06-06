﻿using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Services.Interfaces
{
    public interface IWalletService
    {
        Task<long> ChargeWallet(long userId, ChargeWalletViewModel chargeWallet, string description);
        Task<UserWallet> GetUserWalletById(long walletId);
        Task<bool> UpdateWalletForCharge(UserWallet wallet);
        Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter);
    }
}
