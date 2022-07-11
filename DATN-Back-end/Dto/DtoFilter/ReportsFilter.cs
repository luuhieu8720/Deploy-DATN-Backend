using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoFilter
{
    public class ReportsFilter
    {
        public Guid? DepartmentId { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
