using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StokKontrol.Domain.Entities;
using StokKontrol.Domain.Enums;
using StokKontrol.Service.Abstract;

namespace StokKontrol.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		private readonly IGenericService<Order> _orderService;
		private readonly IGenericService<User> _userService;
		private readonly IGenericService<OrderDetails> _odService;
		private readonly IGenericService<Product> _productService;

		

        public OrderController(IGenericService<Order> orderService, IGenericService<User> userService, IGenericService<OrderDetails> odService, IGenericService<Product> productService)
        {
            _orderService = orderService;
            _userService = userService;
            _odService = odService;
            _productService = productService;
        }

        [HttpGet]//Almak için kullanılır.
		public ActionResult TumSiparisleriGetir()
		{
			//return Ok("Başarılı");//burada IActionResult döndürmem gerekiyor eğer string yazsaydı string döndürmem gerekirdi.

			return Ok(_orderService.GetAll(t0=>t0.SiparisDetaylari,t1=>t1.Kullanici));
		}

		[HttpGet]
		public IActionResult AktifSiparisleriGetir()
		{
			return Ok(_orderService.GetActive(t0 => t0.SiparisDetaylari, t1 => t1.Kullanici));
		}
		[HttpGet("{id}")]
		public IActionResult IdyeGoreSiparisleriGetir(int id)
		{
			return Ok(_orderService.GetByID(id,t0 => t0.SiparisDetaylari, t1 => t1.Kullanici));
		}
		[HttpGet]
		public IActionResult BekleyenSiparisleriGetir()
		{
			return Ok(_orderService.GetDefault(x => x.Status == Status.Pending));
		}
        [HttpGet]
        public IActionResult ReddedilenSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Cancelled));
        }
        [HttpGet]
        public IActionResult OnaylananSiparisleriGetir()
        {
            return Ok(_orderService.GetDefault(x => x.Status == Status.Confirmed));
        }
        [HttpPost]
		public IActionResult SiparisEkle(int userID, [FromQuery] int[] productsIDs, [FromQuery] short[] quantities)
		{
			Order yeniSiparis = new Order();
			yeniSiparis.UserID = userID;
			yeniSiparis.Status=Status.Pending;//Eklenen sipariş bekliyor durumunda eklenecek
			yeniSiparis.IsActive= true;//sipariş onlanır yada reddelirse false çekilecek
			_orderService.Add(yeniSiparis);//db ye eklendiğinde yeni bir order satırı ekleniyor ve ıd oluşuyor
			for (int i = 0; i < productsIDs.Length; i++)
			{
				OrderDetails yeniDetay = new OrderDetails();
				yeniDetay.OrderID=yeniSiparis.UserID;
				yeniDetay.ProductID = productsIDs[i];
				yeniDetay.Quantity = quantities[i];
				yeniDetay.UnitPrice= _productService.GetById(productsIDs[i]).UnitPrice;
				yeniDetay.IsActive = true;
				//_odService.Add(yeniDetay);
			}
            return CreatedAtAction("IdyeGoreSiparisGetir", new { id = yeniSiparis.Id, yeniSiparis });

        }
		[HttpGet]
		public  IActionResult SiparisOnayla(int id)
		{
			var onaylananSiparis = _orderService.GetById(id);
			if (onaylananSiparis==null)
			{
				return NotFound();
			}
			else
			{
				List<OrderDetails> details = _odService.GetDefault(x => x.OrderID == onaylananSiparis.Id).ToList();
				foreach (OrderDetails item in details)
				{
					Product siparistekiUrun = _productService.GetById(item.ProductID);
					 siparistekiUrun.Stock -= item.Quantity;
					_productService.Update(siparistekiUrun);
					item.IsActive = false;
					_odService.Update(item);
				}
				onaylananSiparis.Status = Status.Confirmed;
				onaylananSiparis.IsActive = false;
				_orderService.Update(onaylananSiparis);
			}
			

			return Ok(onaylananSiparis);
		}
        [HttpGet]
        public IActionResult SiparisReddet(int id)
        {
            var rededilenSiparis = _orderService.GetById(id);
            if (rededilenSiparis == null)
            {
                return NotFound();
            }
            else
            {
                List<OrderDetails> details = _odService.GetDefault(x => x.OrderID == rededilenSiparis.Id).ToList();
                foreach (OrderDetails item in details)
                {
                    //Product siparistekiUrun = _productService.GetById(item.ProductID);
                    //siparistekiUrun.Stock -= item.Quantity;
                    //_productService.Update(siparistekiUrun);
                    item.IsActive = false;
                    _odService.Update(item);
                }
                rededilenSiparis.Status = Status.Cancelled;
                rededilenSiparis.IsActive = false;
                _orderService.Update(rededilenSiparis);
            }


            return Ok(rededilenSiparis);
        }
    }
}
