using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Controllers
{
    public class SiteBaseController : Controller
    {
        protected readonly string SuccessMessage = "SuccessMessage";
        protected readonly string ErrorMessage = "ErrorMessage";
        protected readonly string WarningMessage = "WarningMessage";
        protected readonly string InfoMessage = "InfoMessage";
    }
}
