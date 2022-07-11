using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class Comment : BaseModel
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public User CommentedUser { get; set; }

        public Guid ReportId { get; set; }

        [ForeignKey(nameof(ReportId))]
        public Report Report { get; set; }

        public string Content { get; set; }

        public DateTime CommentedTime { get; set; }
    }
}
