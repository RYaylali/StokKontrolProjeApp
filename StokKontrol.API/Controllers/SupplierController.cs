using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Service.Abstract;

namespace StokKontrol.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SupplierController : ControllerBase
	{
		private readonly IGenericService<Supplier> _service;

		public SupplierController(IGenericService<Supplier> service)
		{
			_service = service;
		}
		[HttpGet]//Almak için kullanılır.
		public ActionResult TumTedarikcileriGetir()
		{
			//return Ok("Başarılı");//burada IActionResult döndürmem gerekiyor eğer string yazsaydı string döndürmem gerekirdi.

			return Ok(_service.GetAll());
		}

		[HttpGet]
		public IActionResult AktifTedarikcileriGetir()
		{
			return Ok(_service.GetActive());
		}
		[HttpGet("{id}")]
		public IActionResult IdyeGoreTedarikciGetir(int id)
		{
			return Ok(_service.GetByID(id));
		}

		[HttpPost]
		public IActionResult TedarikciEkle(Supplier supplier)
		{
			_service.Add(supplier);
			//return Ok("Başarılı");
			return CreatedAtAction("IdyeGoreKategoriGetir", new { id = supplier.Id }, supplier);
		}
		[HttpPut]
		public IActionResult TedarikciGüncelle(int id, Supplier supplier)
		{
			if (id != supplier.Id)
			{
				return BadRequest();
			}
			try
			{
				_service.Update(supplier);
				return Ok(supplier);
			}
			catch (DbUpdateConcurrencyException)//beklenmeyen sayıda işlem yapılırsa bu hata çıkar
			{

				if (!TedarikciVarMi(id))
					return NotFound();

				return NoContent();//hata kodu 204
			}
		}
		private bool TedarikciVarMi(int id)
		{
			return _service.Any(cat => cat.Id == id);//idleri karşılaştırmak için metot 
		}
		[HttpDelete("{id}")]
		public IActionResult TedarikciSil(int id)
		{
			var supplier = _service.GetByID(id);
			if (supplier == null)
			{
				return NotFound();
			}
			try
			{
				_service.Remove((Supplier)supplier);//hataya bak 
				return Ok("İşlem başarılı");
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
		[HttpGet("{id}")]
		public IActionResult TedarikciAktiflestirme(int id)
		{
			var supplier = _service.GetByID(id);
			if (supplier == null)
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
