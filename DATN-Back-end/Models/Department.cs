using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Models
{
    public class Department : BaseModel
    {
        public string Name { get; set; }

        public Guid? ManagerId { get; set; }
        [ForeignKey(nameof(ManagerId))]

        public User? Manager { get; set; }
    }
}
