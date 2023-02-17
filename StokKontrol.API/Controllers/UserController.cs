using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Service.Abstract;

namespace StokKontrol.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IGenericService<User> _service;

		public UserController(IGenericService<User> service)
		{
			_service = service;
		}
		[HttpGet]//Almak için kullanılır.
		public ActionResult TumSiparisleriGetir()
		{
			//return Ok("Başarılı");//burada IActionResult döndürmem gerekiyor eğer string yazsaydı string döndürmem gerekirdi.

			return Ok(_service.GetAll());
		}

		[HttpGet]
		public IActionResult AktifKullaniciGetir()
		{
			return Ok(_service.GetActive());
		}
		[HttpGet("{id}")]
		public IActionResult IdyeGoreKulaniciGetir(int id)
		{
			return Ok(_service.GetByID(id));
		}

		[HttpPost]
		public IActionResult KullaniciEkle(User user)
		{
			_service.Add(user);
			//return Ok("Başarılı");
			return CreatedAtAction("IdyeGoreKategoriGetir", new { id = user.Id }, user);
		}
		[HttpPut]
		public IActionResult KullaniciGüncelle(int id, User user)
		{
			if (id != user.Id)
			{
				return BadRequest();
			}
			try
			{
				_service.Update(user);
				return Ok(user);
			}
			catch (DbUpdateConcurrencyException)//beklenmeyen sayıda işlem yapılırsa bu hata çıkar
			{

				if (!KullaniciVarMi(id))
					return NotFound();

				return NoContent();//hata kodu 204
			}
		}
		private bool KullaniciVarMi(int id)
		{
			return _service.Any(cat => cat.Id == id);//idleri karşılaştırmak için metot 
		}
		[HttpDelete("{id}")]
		public IActionResult KullaniciSil(int id)
		{
			var user = _service.GetByID(id);
			if (user == null)
			{
				return NotFound();
			}
			try
			{
				_service.Remove((User)user);//hataya bak 
				return Ok("İşlem başarılı");
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
		[HttpGet("{id}")]
		public IActionResult KullaniciAktiflestirme(int id)
		{
			var user = _service.GetByID(id);
			if (user == null)
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
		[HttpGet]
		public IActionResult Login(string email, string password)
		{
			if (_service.Any(user => user.Email == email && user.Password == password))
			{
				User login = _service.GetByDefault(x => x.Email == email && x.Password == password);
				return Ok(login);
			}
			else
				return BadRequest();
		}
	}
}
