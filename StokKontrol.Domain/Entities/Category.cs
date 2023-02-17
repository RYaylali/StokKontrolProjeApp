using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Domain.Entities
{
    public class Category:BaseEntity
    {
        public Category()
        {
            Urunler = new List<Product>();//Liste boş geldiğinde hataya düşmemek için constructer metot açtık
        }
        public string CategorrName { get; set; }
        public string Description { get; set; }

        //Navigation Property
        //Bir kategorinin birden fazla ürünü olabilir.

        public virtual List<Product> Urunler { get; set; }
    }
}
