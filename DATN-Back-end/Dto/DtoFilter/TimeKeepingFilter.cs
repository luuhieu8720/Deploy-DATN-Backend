using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoFilter
{
    public class TimeKeepingFilter
    {
        public DateTime? DateTime { get; set; }

        public Guid? UserId { get; set; }

        public Guid? DepartmentId { get; set; }
    }
}
