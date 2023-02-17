using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Service.Abstract;

namespace StokKontrol.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase //Api bizim ugyulamımzın dışarıya açılan penceresi 
    {
        //localhost:PortNo/api/Category/TumKategorileriGetir
        private readonly IGenericService<Category> _service;//şimdi tüm metotlar kategori için çalışır oldu
        public CategoryController(IGenericService<Category> service)
        {
            _service= service;
        }

        [HttpGet]//Almak için kullanılır.
        public ActionResult TumKategorileriGetir()
        {
            //return Ok("Başarılı");//burada IActionResult döndürmem gerekiyor eğer string yazsaydı string döndürmem gerekirdi.

            return Ok(_service.GetAll());
        }

        [HttpGet]
        public IActionResult AktifKategorileriGetir()
        {
            return Ok(_service.GetActive());
        }
        [HttpGet("{id}")]
        public IActionResult IdyeGoreKategoriGetir(int id)
        {
            return Ok(_service.GetByID(id));
        }

        [HttpPost]
        public IActionResult KategoriEkle(Category category)
        {
            _service.Add(category);
            //return Ok("Başarılı");
            return CreatedAtAction("IdyeGoreKategoriGetir", new { id = category.Id }, category);
        }
        [HttpPut]
        public IActionResult KategoriGüncelle(int id,Category category)
        {
            if (id!=category.Id)
            {
                return BadRequest();
            }
            try
            {
               _service.Update(category);
                return Ok(category);
            }
            catch (DbUpdateConcurrencyException)//beklenmeyen sayıda işlem yapılırsa bu hata çıkar
            {

                if (!KategoriVarMi(id))
                    return NotFound();
               
                return NoContent();//hata kodu 204
            }
        }
        private bool KategoriVarMi(int id)
        {
            return _service.Any(cat=>cat.Id==id);//idleri karşılaştırmak için metot 
        }
        [HttpDelete("{id}")]
        public IActionResult KategoriSil(int id)
        {
            var category = _service.GetByID(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _service.Remove((Category)category);//hataya bak 
                return Ok("İşlem başarılı");
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public IActionResult KategoriActtiflestirme(int id)
        {
			var category = _service.GetByID(id);
			if (category == null)
			{
				return NotFound();
			}
			try
			{
				_service.Activate(id);
				return Ok(_service.GetById(id));
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
    }
}
