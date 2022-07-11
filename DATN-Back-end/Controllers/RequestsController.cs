using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoFormRequest;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/request")]
    public class RequestsController : ControllerBase
    {
        private IFormRequestRepository formRequestRepository;

        private IRepository<FormRequest> repository;

        public RequestsController(IFormRequestRepository formRequestRepository,
            IRepository<FormRequest> repository)
        {
            this.formRequestRepository = formRequestRepository;
            this.repository = repository;
        }

        [Authorize]
        [HttpGet]
        public async Task<List<FormRequestItem>> Get() => await formRequestRepository.Get();

        [Authorize]
        [HttpGet("{id}")]
        public async Task<FormRequestDetail> Get(Guid id) => await formRequestRepository.Get(id);

        [Authorize]
        [HttpPost("filter")]
        public async Task<List<FormRequestDetail>> Filter([FromBody]RequestsFilter requestsFilter) => await formRequestRepository.FilterRequest(requestsFilter);

        [Authorize]
        [HttpPost("filter/user")]
        public async Task<List<FormRequestDetail>> FilterForUser([FromBody] RequestsFilter requestsFilter) => await formRequestRepository.FilterRequestForUser(requestsFilter);

        [Authorize]
        [HttpPost]
        public async Task Create([FromBody] FormRequestForm requestForm) => await formRequestRepository.Create(requestForm);

        [Authorize]
        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] FormRequestForm requestForm) => await formRequestRepository.Update(id, requestForm);

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("comfirm/{id}")]
        public async Task ConfirmRequest(Guid id, [FromBody] FormRequestConfirm formRequestConfirm) => await formRequestRepository.ConfirmRequest(id, formRequestConfirm);
    }
}
