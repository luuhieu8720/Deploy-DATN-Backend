using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoFilter
{
    public class RequestsFilter
    {
        public Guid? DepartmentId { get; set; }

        public Guid? UserId { get; set; }

        public int? FormStatusId { get; set; }

        public int? TypeId { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
