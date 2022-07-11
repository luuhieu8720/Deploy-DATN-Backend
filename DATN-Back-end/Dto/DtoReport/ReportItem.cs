using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoReport
{
    public class ReportItem
    {
        public Guid Id { get; set; }
        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string UploadFileLink { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }

        public User User { get; set; }

        public List<Comment> Comments { get; set; }
    }
}
