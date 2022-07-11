using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoReport
{
    public class ReportForm
    {
        public DateTime CreatedTime { get; set; }

        public DateTime UpdatedTime { get; set; }

        public IFormFile File { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }
    }
}
