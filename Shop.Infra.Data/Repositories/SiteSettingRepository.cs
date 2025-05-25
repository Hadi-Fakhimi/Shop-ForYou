using Shop.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site.Sliders;
using Shop.Infra.Data.Context;
using Shop.Domain.ViewModels.Pageing;

namespace Shop.Infra.Data.Repositories
{
    public class SiteSettingRepository:ISiteSettingRepository
    {
        #region Constructor

        private readonly ShopDbContext _context;

        public SiteSettingRepository(ShopDbContext context)
        {
            _context = context;
        }

        #endregion
        public async Task SaveChange()
        {
            await _context.SaveChangesAsync();
        }

        #region Slider

        public async Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filter)
        {
            var query = _context.Sliders.AsQueryable();

            #region Filter

            if (string.IsNullOrEmpty(filter.SliderTitle))
            {
                query = query.Where(c => EF.Functions.Like(c.SliderTitle, $"%{filter.SliderTitle}"));
            }

            #endregion

            #region Pageing
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.CountForShowBeforeAndAfter);

            var allData = await query.Pageing(pager).ToListAsync();
            #endregion

            return filter.SetPageing(pager).SetSliders(allData);
        }

        public async Task AddSlider(Slider slider)
        {
            await _context.Sliders.AddAsync(slider);
        }

        public async Task<Slider> GetSliderById(long sliderId)
        {
            return await _context.Sliders.AsQueryable().SingleOrDefaultAsync(s => s.Id == sliderId);
        }

        public void UpdateSlider(Slider slider)
        {
            _context.Sliders.Update(slider);
        }

        #endregion
    }
}
