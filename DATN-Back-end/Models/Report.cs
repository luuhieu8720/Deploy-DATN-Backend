using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN_Back_end.Models
{
    public class Report : BaseModel
    {
        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public string? UploadFileLink { get; set; }

        public string Content { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public User User { get; set; }

        public List<Comment>? Comments { get; set; }
    }
}
