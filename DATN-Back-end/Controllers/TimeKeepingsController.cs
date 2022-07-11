using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoFormRequest;
using DATN_Back_end.Dto.DtoTimeKeeping;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/timekeeping")]
    public class TimeKeepingsController : ControllerBase
    {
        private readonly IRepository<Timekeeping> repository;

        private readonly ITimeKeepingRepository timeKeepingRepository;

        public TimeKeepingsController(IRepository<Timekeeping> repository,
            ITimeKeepingRepository timeKeepingRepository)
        {
            this.repository = repository;
            this.timeKeepingRepository = timeKeepingRepository;
        }

        [Authorize]
        [HttpPost]
        public async Task<Guid> Checkin([FromBody] TimeKeepingForm timeKeepingForm) => await timeKeepingRepository.CheckIn(timeKeepingForm);

        [Authorize]
        [HttpGet]
        public async Task<List<TimeKeepingItem>> Get() => await repository.Get<TimeKeepingItem>();

        [Authorize]
        [HttpGet("getbyid/{id}")]
        public async Task<TimeKeepingDetail> Get(Guid id) => await repository.Get<TimeKeepingDetail>(id);

        [HttpPost("filter")]
        public async Task<List<TimeKeepingItem>> Filter([FromBody] TimeKeepingFilter timekeepingFilter) => await timeKeepingRepository.FilterTimeKeeping(timekeepingFilter);

        [Authorize]
        [HttpPut]
        public async Task Checkout([FromBody] TimeKeepingForm timeKeepingForm) => await timeKeepingRepository.CheckOut(timeKeepingForm);

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<List<TimeKeepingItem>> GetTimeKeepingInfos(Guid userId) => await timeKeepingRepository.GetTimeKeepingInfos(userId);

        [Authorize]
        [HttpGet("validate")]
        public async Task<TimeKeepingDetail> ValidateCheckinToday(Guid userId) => await timeKeepingRepository.ValidateCheckinToday(userId);

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("punish/delete")]
        public async Task DeletePunish(Guid id, [FromBody] FormRequestForm formRequestForm) => await timeKeepingRepository.DeletePunish(id, formRequestForm);
    }
}
