using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        #region Dashboard

        public IActionResult Index()
        {
            return View();
        }
        #endregion
    }
}
