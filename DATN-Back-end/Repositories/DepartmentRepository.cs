using DATN_Back_end.Dto.DtoDepartment;
using DATN_Back_end.Exceptions;
using DATN_Back_end.Models;
using DATN_Back_end.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly DataContext dataContext;

        public DepartmentRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<DepartmentItem>> Get()
        {
            return await dataContext.Departments
                .Include(b => b.Manager)
                .Select(x => x.ConvertTo<DepartmentItem>())
                .ToListAsync();
        }
    }
}
