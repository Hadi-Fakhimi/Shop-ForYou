using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminBaseController : Controller
    {
        protected readonly string SuccessMessage = "SuccessMessage";
        protected readonly string ErrorMessage = "ErrorMessage";
        protected readonly string WarningMessage = "WarningMessage";
        protected readonly string InfoMessage = "InfoMessage";

    }
}
