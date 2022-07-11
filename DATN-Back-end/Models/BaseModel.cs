using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; }
    }
}
