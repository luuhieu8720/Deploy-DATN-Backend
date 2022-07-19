using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoTimeKeeping
{
    public class TimeKeepingItem
    {
        public Guid Id { get; set; }
        public DateTime? CheckinTime { get; set; }

        public DateTime? CheckoutTime { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public bool IsPunished { get; set; }

        public int PunishedTime { get; set; }
    }
}
