using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shop.Application.Services.Interfaces;

namespace Shop.Presentation.Premission
{
    public class PremissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        #region Constructor
        private  IUserService _userService;
        private  long _premissionId = 0;
        public PremissionCheckerAttribute(long premissionId)
        {
            _premissionId = premissionId;
        }
        #endregion

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _userService = (IUserService)context.HttpContext.RequestServices.GetService(typeof(IUserService));

            if (context.HttpContext.User.Identity.IsAuthenticated )
            {
                var phoneNumber = context.HttpContext.User.Identity.Name;

                if(!_userService.CheckPermission(_premissionId, phoneNumber))
                {
                    context.Result = new RedirectResult("/Login");
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login");
            }

        }
    }
}
