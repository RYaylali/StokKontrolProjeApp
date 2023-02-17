using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol.Domain.Entities
{
    public class BaseEntity
    {
        [Column(Order =1)]//bu kolonun(id)'nin tüm tablolar için ilk sırada gelmesini istiyoruz burada
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}
