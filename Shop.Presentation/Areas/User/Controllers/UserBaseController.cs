using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    [Route("User")]
    public class UserBaseController : Controller
    {
        protected readonly string SuccessMessage = "SuccessMessage";
        protected readonly string ErrorMessage = "ErrorMessage";
        protected readonly string WarningMessage = "WarningMessage";
        protected readonly string InfoMessage = "InfoMessage";
    }
}
