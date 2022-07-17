using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DATN_Back_end.Dto.DtoDepartment;
using DATN_Back_end.Dto.DtoRequestType;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DATN_Back_end.Controllers
{
    [Route("api/requesttype")]
    public class RequestTypeController : ControllerBase
    {

        private IRequestTypeRepository requestTypeRepository;

        public RequestTypeController(IRequestTypeRepository requestTypeRepository)
        {
            this.requestTypeRepository = requestTypeRepository;
        }

        [HttpGet]
        public async Task<List<RequestTypeItem>> Get() => await requestTypeRepository.Get();

        [HttpPost]
        public async Task Create([FromBody] RequestTypeForm departmentForm) => await requestTypeRepository.Create(departmentForm);

        [HttpPut("{id}")]
        public async Task Update(int id, [FromBody] RequestTypeForm departmentForm) => await requestTypeRepository.Update(id, departmentForm);
    }
}
