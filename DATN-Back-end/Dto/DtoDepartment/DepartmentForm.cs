using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoDepartment
{
    public class DepartmentForm
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên phòng ban không được để trống")]
        public string Name { get; set; }

        public Guid? ManagerId { get; set; }
    }
}
