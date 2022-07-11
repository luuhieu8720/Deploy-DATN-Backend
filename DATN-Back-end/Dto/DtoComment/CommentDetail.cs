using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoComment
{
    public class CommentDetail
    {
        public Guid ReportId { get; set; }

        public string Content { get; set; }

        public DateTime CommentedTime { get; set; }

        public Guid UserId { get; set; }

        public User CommentedUser { get; set; }
    }
}
