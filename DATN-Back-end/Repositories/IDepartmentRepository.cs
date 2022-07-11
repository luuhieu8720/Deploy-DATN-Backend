using DATN_Back_end.Dto.DtoDepartment;
using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Task<List<DepartmentItem>> Get();
    }
}
