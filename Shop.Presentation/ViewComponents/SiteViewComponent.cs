using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Domain.ViewModels.Admin.Products;
using Shop.Domain.ViewModels.Site.Sliders;

namespace Shop.Presentation.ViewComponents
{
    #region Header
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        public SiteHeaderViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.User = await _userService.GetUserByPhoneNumber(User.Identity.Name);
            }

            return View("SiteHeader");
        }
    }
    #endregion

    #region Footer
    public class SiteFooterViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SiteFooter");
        }
    }
    #endregion

    #region Slider - Home

    public class SliderHomeViewComponent : ViewComponent
    {
        #region Constructor

        private readonly ISiteSettingService _siteSettingService;

        public SliderHomeViewComponent(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterSlidersViewModel()
            {
                TakeEntity = 10
            };
            var data = await _siteSettingService.FilterSliders(filter);


            return View("SliderHome",data);
        }
    }

    #endregion

    #region Popular Category - Home

    public class PopularCategoryHomeViewComponent : ViewComponent
    {
        #region Constructor

        private readonly IProductService _productService;

        public PopularCategoryHomeViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterProductCategoriesViewModel()
            {
                TakeEntity = 6
            };
            var data = await _productService.FilterProductCategories(filter);


            return View("PopularCategoryHome", data);
        }
    }

    #endregion

    #region Category Sidebar

    public class SidebarCategoryHomeViewComponent : ViewComponent
    {
        #region Constructor

        private readonly IProductService _productService;

        public SidebarCategoryHomeViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterProductCategoriesViewModel();
            var data = await _productService.FilterProductCategories(filter);


            return View("SidebarCategoryHome", data);
        }
    }

    #endregion
    #region Category Sidebar mobile
    public class SidebarCategoryMobileHomeViewComponent : ViewComponent
    {
        #region Constructor

        private readonly IProductService _productService;

        public SidebarCategoryMobileHomeViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterProductCategoriesViewModel();
            var data = await _productService.FilterProductCategories(filter);

            return View("SidebarCategoryMobileHome", data );
        }
    }
    #endregion

    #region All Product Slider

    public class AllProductInSliderViewComponent : ViewComponent
    {
        #region Constructor

        private readonly IProductService _productService;

        public AllProductInSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var data = await _productService.ShowAllProductInSlider();

            return View("AllProductInSlider", data);
        }
    }

    #region Last Product Slider

    public class LastProductInSliderViewComponent : ViewComponent
    {
        #region Constructor

        private readonly IProductService _productService;

        public LastProductInSliderViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var data = await _productService.ShowLastProductInSlider();

            return View("LastProductInSlider", data);
        }
    }

    #endregion
    #endregion
}
