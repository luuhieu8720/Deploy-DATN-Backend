using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class FormRequest : BaseModel
    {
        public string Content { get; set; }

        public string Reason { get; set; }

        public DateTime SubmittedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }

        public int? Hours { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]

        public User User { get; set; }

        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]

        public FormStatus FormStatus { get; set; }

        public DateTime RequestDate { get; set; }

        public int RequestTypeId { get; set; }
        [ForeignKey(nameof(RequestTypeId))]

        public RequestType RequestType { get; set; }
    }
}
