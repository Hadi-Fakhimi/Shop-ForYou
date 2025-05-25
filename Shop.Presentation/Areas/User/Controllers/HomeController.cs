using Microsoft.AspNetCore.Mvc;

namespace Shop.Presentation.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
