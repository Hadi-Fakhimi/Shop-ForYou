using Microsoft.AspNetCore.Mvc;
using Shop.Presentation.Models;
using System.Diagnostics;

namespace Shop.Presentation.Controllers
{
    public class HomeController : SiteBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
