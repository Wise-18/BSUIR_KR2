using Microsoft.AspNetCore.Mvc;

namespace BSUIR_KR1.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Title"] = "Контрольная работа №1";
            return View();
        }
    }
}