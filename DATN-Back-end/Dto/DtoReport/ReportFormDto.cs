using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoReport
{
    public class ReportFormDto
    {
        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string UploadFileLink { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }
    }
}
