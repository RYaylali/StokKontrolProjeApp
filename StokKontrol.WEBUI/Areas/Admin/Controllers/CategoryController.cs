using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StokKontrol.Domain.Entities;

namespace StokKontrol.WEBUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        string uri = "https://localhost:7299";
        public async Task< IActionResult> Index()
        {
            List<Category> kategoriler = new List<Category>();
            using (var client = new HttpClient())
            {
                using(var cevap = await client.GetAsync($"{uri}/api/category/tümkategorilerigetir"))
                {
                    string apiCevap = await cevap.Content.ReadAsStringAsync();
                    kategoriler=JsonConvert.DeserializeObject<List<Category>>(apiCevap);
                }
            }
            return View(kategoriler);
        }
    }
}
