using DATN_Back_end.Dto.DtoStatus;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/formstatuses")]
    public class FormstatusesController : ControllerBase
    {
        private IFormStatusRepository formStatusRepository;

        public FormstatusesController(IFormStatusRepository formStatusRepository)
        {
            this.formStatusRepository = formStatusRepository;
        }

        [HttpGet]
        public async Task<List<FormStatusItem>> Get() => await formStatusRepository.Get();

        [HttpGet("{id}")]
        public async Task<FormStatusDetail> Get(int id) => await formStatusRepository.Get(id);

        [HttpPost]
        public async Task Create([FromBody] FormStatusForm formStatusForm) => await formStatusRepository.Create(formStatusForm);

        [HttpPut("{id}")]
        public async Task Update(int id, [FromBody] FormStatusForm formStatusForm) => await formStatusRepository.Update(id, formStatusForm);
    }
}
