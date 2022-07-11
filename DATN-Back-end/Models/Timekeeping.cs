using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class Timekeeping : BaseModel
    {
        public DateTime? CheckinTime { get; set; }

        public DateTime? CheckoutTime { get; set; }

        public bool IsPunished { get; set; }

        public int PunishedTime { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public User User { get; set; }
    }
}
