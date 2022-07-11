using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoWorkingTime;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/workingtime")]
    public class WorkingTimesController : ControllerBase
    {
        private readonly IWorkingTimeRepository workingTimeRepository;

        public WorkingTimesController(IWorkingTimeRepository workingTimeRepository)
        {
            this.workingTimeRepository = workingTimeRepository;
        }

        [HttpPost("filter")]
        public async Task<List<WorkingTimeItem>> FIlterFilterUserWorkingTime([FromBody]WorkingTimeFilter workingTimeFilter)
            => await workingTimeRepository.FilterUserWorkingTime(workingTimeFilter);

        [HttpGet("all")]
        public async Task<List<WorkingTimeItem>> GetAllUserWorkingTime(DateTime dateTime)
            => await workingTimeRepository.GetAllUserWorkingTime(dateTime);

        [HttpGet]
        public async Task<double> GetWorkingTime(DateTime dateTime)
            => await workingTimeRepository.GetWorkingTime(dateTime);
    }
}
