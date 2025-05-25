using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client.Extensibility;
using Shop.Application.Extentions;
using Shop.Application.Generator;
using Shop.Application.Services.Interfaces;
using Shop.Application.StaticTools;
using Shop.Application.Utils;
using Shop.Domain.Interfaces;
using Shop.Domain.Models.Site;
using Shop.Domain.ViewModels.Site.Sliders;
using Shop.Infra.Data.Context;

namespace Shop.Application.Services.Implementations
{
    public class SiteSettingService:ISiteSettingService
    {
        #region Constructor

        private readonly ISiteSettingRepository _siteSettingRepository;

        public SiteSettingService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        #endregion
        #region Slider

        public async Task<FilterSlidersViewModel> FilterSliders(FilterSlidersViewModel filter)
        {
            return await _siteSettingRepository.FilterSliders(filter);
        }

        public async Task<CreateSliderResult> CreateSlider(CreateSliderViewModel createSlider, IFormFile imageFile)
        {
            var newSlider = new Slider()
            {
                SliderTitle = createSlider.SliderTitle,
                SliderHref = createSlider.SliderHref,
                SliderTextButton = createSlider.SliderTextButton,
                SliderPrice = createSlider.SliderPrice,
                SliderText = createSlider.SliderText
            };

            if (imageFile != null && imageFile.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(imageFile.FileName);
                imageFile.AddImageToServer(imageName, PathsExtensions.ImageSliderOriginServer, 150, 150, PathsExtensions.ImageSliderThumbServer);

                newSlider.SliderImageName = imageName;
            }
            else
            {
               return CreateSliderResult.ImageNotFound;
            }

            await _siteSettingRepository.AddSlider(newSlider);

            await _siteSettingRepository.SaveChange();

            return CreateSliderResult.Success;

        }

        public async Task<EditSliderViewModel> GetEditSlider(long sliderId)
        {
            var currentSlider = await _siteSettingRepository.GetSliderById(sliderId);

            if (currentSlider == null)
            {
                return null;
            }

            return new EditSliderViewModel()
            {
                SliderImageName = currentSlider.SliderImageName,
                SliderTitle = currentSlider.SliderTitle,
                SliderText = currentSlider.SliderText,
                SliderPrice = currentSlider.SliderPrice,
                SliderHref = currentSlider.SliderHref,
                SliderTextButton = currentSlider.SliderTextButton,
                SliderId = currentSlider.Id
            };
        }

        public async Task<EditSliderResult> EditSlider(EditSliderViewModel editSlider, IFormFile imageFile)
        {
            var slider = await _siteSettingRepository.GetSliderById(editSlider.SliderId);

            if (slider == null)
            {
                return EditSliderResult.NotFound;
            }

            slider.SliderHref = editSlider.SliderHref;
            slider.SliderPrice = editSlider.SliderPrice;
            slider.SliderText = editSlider.SliderText;
            slider.SliderTextButton = editSlider.SliderTextButton;
            slider.SliderTitle = editSlider.SliderTitle;

            if (imageFile != null && imageFile.IsImage())
            {
                var imageName = CodeGenerator.GenerateFileName(imageFile.FileName);
                imageFile.AddImageToServer(imageName, PathsExtensions.ImageSliderOriginServer, 150, 150, PathsExtensions.ImageSliderThumbServer,slider.SliderImageName);

                slider.SliderImageName = imageName;
            }

            _siteSettingRepository.UpdateSlider(slider);
            await _siteSettingRepository.SaveChange();

            return EditSliderResult.Success;

        }

        #endregion


    }
}
