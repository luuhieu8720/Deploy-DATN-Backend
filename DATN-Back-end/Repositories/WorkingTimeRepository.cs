using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoWorkingTime;
using DATN_Back_end.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public class WorkingTimeRepository : IWorkingTimeRepository
    {
        private readonly DataContext dataContext;

        private readonly IAuthenticationService authenticationService;

        public WorkingTimeRepository(DataContext dataContext, IAuthenticationService authenticationService)
        {
            this.dataContext = dataContext;
            this.authenticationService = authenticationService;
        }

        public async Task<List<WorkingTimeItem>> GetAllUserWorkingTime(DateTime dateTime)
        {
            var entry = (await dataContext.Timekeepings
                .Include(x => x.User)
                .Where(x => x.CheckinTime.Value.Year == dateTime.Year
                && x.CheckinTime.Value.Month == dateTime.Month)
                .ToListAsync())
                .GroupBy(x => x.UserId)
                .Select(x => x)
                .ToList();

            return entry.Select(x => new WorkingTimeItem()
            {
                Time = x.Sum(c => (c.CheckoutTime.Value - c.CheckinTime.Value).TotalHours),
                PunishedTime = x.Sum(c => c.PunishedTime),
                UserId = x.Select(c => c.UserId).FirstOrDefault(),
                User = x.Select(c => c.User).FirstOrDefault()
            })
            .ToList();
        }

        public async Task<List<WorkingTimeItem>> FilterUserWorkingTime(WorkingTimeFilter workingTimeFilter)
        {
            var entry = (await dataContext.Timekeepings
                .Include(x => x.User)
                .Where(x => (workingTimeFilter.DepartmentId.HasValue ? x.User.DepartmentId == workingTimeFilter.DepartmentId.Value
                : x != null))
                .Where(x => workingTimeFilter.UserId.HasValue ? x.UserId == workingTimeFilter.UserId.Value
                : x != null)
                .Where(x => workingTimeFilter.DateTime.HasValue ?
                (x.CheckinTime.Value.Month == workingTimeFilter.DateTime.Value.Month
                 && x.CheckinTime.Value.Year == workingTimeFilter.DateTime.Value.Year)
                : x != null)
                .ToListAsync())
                .GroupBy(x => x.UserId)
                .Select(x => x)
                .ToList();

            return entry.Select(x => new WorkingTimeItem()
                    {
                        UserId = x.Select(c => c.UserId).FirstOrDefault(),
                        Time = x.Sum(c => c.CheckoutTime.HasValue ? (c.CheckoutTime.Value - c.CheckinTime.Value).TotalHours : 0),
                        PunishedTime = x.Sum(c => c.PunishedTime),
                        User = x.Select(c => c.User).FirstOrDefault()
                    })
                    .ToList();
        }

        public async Task<double> GetWorkingTime(DateTime dateTime)
        {
            var currentUserId = authenticationService.CurrentUserId;

            var entry = await dataContext.Timekeepings.Where(x => x.UserId == currentUserId
               && x.CheckinTime.Value.Year == dateTime.Year
               && x.CheckinTime.Value.Month == dateTime.Month).Select(x => x).ToListAsync();
                
            return entry.Sum(x => (x.CheckoutTime.Value - x.CheckinTime.Value).TotalHours);
        }
    }
}
