using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Presentation.Areas.Admin.Controllers
{
    public class SiteController : AdminBaseController
    {
        #region Constructor

        private readonly ISiteSettingService _siteSettingService;

        public SiteController(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        #endregion

        #region Filter Slider

        public async Task<IActionResult> FilterSlider(FilterSlidersViewModel filter)
        {
            return View(await _siteSettingService.FilterSliders(filter));
        }

        #endregion

        #region Create - Slider
        [HttpGet]
        public IActionResult CreateSlider()
        {
            return View();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider(CreateSliderViewModel createSliderViewModel , IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteSettingService.CreateSlider(createSliderViewModel, imageFile);

                switch (result)
                {
                    case CreateSliderResult.ImageNotFound:
                        TempData[ErrorMessage] = "تصویر اسلایدر را بارگذاری کنید";
                        break;

                    case CreateSliderResult.Success:
                        TempData[SuccessMessage] = "عملیات ایجاد با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterSlider));
                }
            }


            return View(createSliderViewModel);
        }

        #endregion

        #region Edit - Slider
        [HttpGet]
        public async Task<IActionResult> EditSlider(long sliderId)
        {
            var data = await _siteSettingService.GetEditSlider(sliderId);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlider(EditSliderViewModel editSliderViewModel , IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                var result = await _siteSettingService.EditSlider(editSliderViewModel, imageFile);
                switch (result)
                {
                    case EditSliderResult.NotFound:
                        TempData[ErrorMessage] = "با شناسه مورد نظر یافت نشد";
                        break;
                    case EditSliderResult.Success:
                        TempData[SuccessMessage] = "عملیات ویرایش با موفقیت انجام شد";
                        return RedirectToAction(nameof(FilterSlider));
                }
            }


            return View();
        }

        #endregion

    }
}
