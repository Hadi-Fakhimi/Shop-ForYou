using Microsoft.AspNetCore.Mvc;
using Shop.Application.Services.Interfaces;
using Shop.Presentation.Extensions;

namespace Shop.Presentation.Areas.User.ViewComponents
{
    public class UserSideBarViewComponent : ViewComponent
    {
        #region Constroctor
        private readonly IUserService _userService;
        public UserSideBarViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserById(User.GetUserId());

                return View("UserSideBar", user);
            }
            return View("UserSideBar");
        }
    }
    public class UserInformationViewComponent : ViewComponent
    {
        #region Constroctor
        private readonly IUserService _userService;
        public UserInformationViewComponent(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetUserById(User.GetUserId());

                return View("UserInformation", user);
            }
            return View("UserInformation");
        }
    }
}
