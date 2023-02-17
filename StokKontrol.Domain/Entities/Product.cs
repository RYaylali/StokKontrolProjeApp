using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Domain.Entities
{
    public class Product:BaseEntity
    {
        public Product()
        {
            SiparisDetaylari = new List<OrderDetails>();//Liste boş geldiğinde hataya düşmemek için constructer metot açtık
        }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }

        //Navigation Property-------------------------------------------------

        //Bir ürünün bir kategorisi olur

        [ForeignKey("Kategori")]
        public int CategoryID { get; set; }
        public virtual Category? Kategori { get; set; }

        //Bir ürünün bir tedarikçisi olur

        [ForeignKey("Tedarikci")]
        public int SupplierID { get; set; }
        public virtual Supplier? Tedarikci { get; set; }//virtual olma nedeni Lazy Loading çeşidini uygulayabilmek için virtual yazdık.


        //Bir üün bireden fazla sipariş detayındad olabilir.

        public virtual List<OrderDetails> SiparisDetaylari { get; set; }


    }
}
