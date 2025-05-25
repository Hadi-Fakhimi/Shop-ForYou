using Shop.Domain.ViewModels.Site.Sliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Shop.Application.Services.Interfaces
{
    public interface ISiteSettingService
    {
        Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filter);
        Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider , IFormFile imageFile);
        Task<EditSliderViewModel> GetEditSlider(long sliderId);
        Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile imageFile);




    }
}
