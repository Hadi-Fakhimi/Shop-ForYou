using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Domain.Interfaces
{
    public interface ISiteSettingRepository
    {

        Task SaveChange();

        #region Slider

        Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filter);
        Task AddSlider(Slider slider);
        Task<Slider> GetSliderById(long sliderId);
        void UpdateSlider(Slider slider);

        #endregion
    }
}
