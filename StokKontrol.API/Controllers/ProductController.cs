using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Service.Abstract;

namespace StokKontrol.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IGenericService<Product> _service;

		public ProductController(IGenericService<Product> service)
		{
			_service = service;
		}
		[HttpGet]//Almak için kullanılır.
		public ActionResult TumUrunleriGetir()
		{
			//return Ok("Başarılı");//burada IActionResult döndürmem gerekiyor eğer string yazsaydı string döndürmem gerekirdi.

			return Ok(_service.GetAll());
		}

		[HttpGet]
		public IActionResult AktifUrunleriGetir()
		{
			return Ok(_service.GetActive());
		}
		[HttpGet("{id}")]
		public IActionResult IdyeGoreUrunGetir(int id)
		{
			return Ok(_service.GetByID(id));
		}

		[HttpPost]
		public IActionResult UrunEkle(Product product)
		{
			_service.Add(product);
			//return Ok("Başarılı");
			return CreatedAtAction("IdyeGoreKategoriGetir", new { id = product.Id }, product);
		}
		[HttpPut]
		public IActionResult UrunGüncelle(int id, Product product)
		{
			if (id != product.Id)
			{
				return BadRequest();
			}
			try
			{
				_service.Update(product);
				return Ok(product);
			}
			catch (DbUpdateConcurrencyException)//beklenmeyen sayıda işlem yapılırsa bu hata çıkar
			{

				if (!UrunVarMi(id))
					return NotFound();

				return NoContent();//hata kodu 204
			}
		}
		private bool UrunVarMi(int id)
		{
			return _service.Any(cat => cat.Id == id);//idleri karşılaştırmak için metot 
		}
		[HttpDelete("{id}")]
		public IActionResult UrunSil(int id)
		{
			var product = _service.GetByID(id);
			if (product == null)
			{
				return NotFound();
			}
			try
			{
				_service.Remove((Product)product);//hataya bak 
				return Ok("İşlem başarılı");
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
		[HttpGet("{id}")]
		public IActionResult UrunAktiflestirme(int id)
		{
			var product = _service.GetByID(id);
			if (product == null)
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
