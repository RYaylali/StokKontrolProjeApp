using Microsoft.AspNetCore.Mvc;

namespace StokKontrol.WEBUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
