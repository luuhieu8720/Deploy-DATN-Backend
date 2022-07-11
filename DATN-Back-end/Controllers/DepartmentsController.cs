using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATN_Back_end.Dto.DtoDepartment;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DATN_Back_end.Controllers
{
    [Route("api/departments")]
    public class DepartmentsController : ControllerBase
    {
        private IRepository<Department> repository;

        private IDepartmentRepository departmentRepository;

        public DepartmentsController(IRepository<Department> repository, 
            IDepartmentRepository departmentRepository)
        {
            this.repository = repository;
            this.departmentRepository = departmentRepository;
        }

        [HttpGet]
        public async Task<List<DepartmentItem>> Get() => await departmentRepository.Get();

        [HttpGet("{id}")]
        public async Task<DepartmentDetail> Get(Guid id) => await repository.Get<DepartmentDetail>(id);

        [HttpPost]
        public async Task Create([FromBody] DepartmentForm departmentForm) => await repository.Create(departmentForm);

        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] DepartmentForm departmentForm) => await repository.Update(id, departmentForm);
    }
}
