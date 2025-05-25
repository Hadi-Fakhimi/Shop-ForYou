using Microsoft.EntityFrameworkCore;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Wallet;
using Shop.Domain.ViewModels.Pageing;
using Shop.Domain.ViewModels.Wallet;
using Shop.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infra.Data.Repositories
{
    public class WalletRepository : IWalletRepositorry
    {
        #region Constructor
        private readonly ShopDbContext _context;
        public WalletRepository(ShopDbContext context)
        {
            _context = context;
        }
        #endregion

        public async Task CreateWallet(UserWallet wallet)
        {
            await _context.AddAsync(wallet);
        }

        public async Task<FilterWalletViewModel> FilterWallet(FilterWalletViewModel filter)
        {
            var query =  _context.UserWallets.AsQueryable();
            #region Filter
            if (filter.UserId != 0 && filter.UserId != null)
            {
                query = query.Where(c => c.UserId == filter.UserId);    
            }
            #endregion

            #region Pageing
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filter.SetPageing(pager).SetWallets(allData);
        }

        public async Task<UserWallet> GetUserWalletById(long walletId)
        {
            return await _context.UserWallets.AsQueryable().SingleOrDefaultAsync(uw => uw.Id == walletId);
        }

        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateWallet(UserWallet wallet)
        {
            _context.UserWallets.Update(wallet);
        }
    }
}
