using DATN_Back_end.Dto.DtoUser;
using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Dto.DtoDepartment
{
    public class DepartmentItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? ManagerId { get; set; }

        public UserShorted? Manager { get; set; }
    }
}
