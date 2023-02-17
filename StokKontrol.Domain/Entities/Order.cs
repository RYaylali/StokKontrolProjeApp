using StokKontrol.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Domain.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            SiparisDetaylari = new List<OrderDetails>();//Liste boş geldiğinde hataya düşmemek için constructer metot açtık
        }

        [ForeignKey("Kullanici")]
        public int UserID { get; set; }
        public Status Status { get; set; }

        //Navigation Property

        //Bir sipairşin ibr kullanıccısı olur
        //Bir siparişin birden fazla sipariş detayı olur

        public virtual UserRole? Kullanici { get; set; }
        public virtual List<OrderDetails> SiparisDetaylari { get; set; }
    }
}
