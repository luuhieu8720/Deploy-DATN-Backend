using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoWorkingTime
{
    public class WorkingTimeItem
    {
        public double Time { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public int PunishedTime { get; set; }
    }
}
