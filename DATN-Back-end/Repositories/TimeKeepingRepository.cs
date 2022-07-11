using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoFormRequest;
using DATN_Back_end.Dto.DtoTimeKeeping;
using DATN_Back_end.Exceptions;
using DATN_Back_end.Models;
using DATN_Back_end.Services;
using Microsoft.EntityFrameworkCore;

namespace DATN_Back_end.Repositories
{
    public class TimeKeepingRepository : Repository<Timekeeping>, ITimeKeepingRepository
    {
        private readonly DataContext dataContext;

        public TimeKeepingRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<Guid> CheckIn(TimeKeepingForm timeKeepingForm)
        {
            timeKeepingForm.CheckinTime = DateTime.Parse(timeKeepingForm.CheckinTime.ToString()).ToLocalTime();

            var formRequest = await dataContext.FormRequests
                .Include(x => x.FormStatus)
                .Where(x => x.UserId == timeKeepingForm.UserId
                && x.RequestDate.Day == timeKeepingForm.CheckinTime.Value.Day
                && x.RequestDate.Month == timeKeepingForm.CheckinTime.Value.Month
                && x.RequestDate.Year == timeKeepingForm.CheckinTime.Value.Year
                && x.RequestTypeId == 5)
                .FirstOrDefaultAsync();

            if (formRequest != null)
            {
                if (timeKeepingForm.CheckinTime.Value.Hour > 9 && formRequest.FormStatus.Status != "Approved")
                {
                    timeKeepingForm.IsPunished = true;
                    timeKeepingForm.PunishedTime += 1;
                }
            }
            else
            {
                timeKeepingForm.IsPunished = true;
                timeKeepingForm.PunishedTime += 1;
            }

            var timeKeeping = timeKeepingForm.ConvertTo<Timekeeping>();
            await dataContext.Timekeepings.AddAsync(timeKeeping);

            await dataContext.SaveChangesAsync();

            return timeKeeping.Id;
        }

        public async Task CheckOut(TimeKeepingForm timeKeepingForm)
        {
            var entry = await dataContext.Timekeepings
                .Where(x => x.UserId == timeKeepingForm.UserId &&
                DateTime.Compare(timeKeepingForm.CheckinTime.Value.Date, x.CheckinTime.Value.Date) == 0)
                .FirstOrDefaultAsync();

            var formRequest = await dataContext.FormRequests
                .Include(x => x.FormStatus)
                .Where(x => x.UserId == timeKeepingForm.UserId
                && x.RequestDate.Day == timeKeepingForm.CheckinTime.Value.Day
                && x.RequestDate.Month == timeKeepingForm.CheckinTime.Value.Month
                && x.RequestDate.Year == timeKeepingForm.CheckinTime.Value.Year
                && x.RequestTypeId == 6)
                .FirstOrDefaultAsync();

            timeKeepingForm.CheckoutTime = DateTime.Parse(timeKeepingForm.CheckoutTime.ToString()).ToLocalTime();

            timeKeepingForm.CheckinTime = DateTime.Parse(timeKeepingForm.CheckinTime.ToString()).ToLocalTime();
            
            var checkAmOrPm = timeKeepingForm.CheckoutTime.Value.ToString("tt", CultureInfo.InvariantCulture);

            if (formRequest != null)
            {
                if (((timeKeepingForm.CheckoutTime.Value.Hour < 18 && checkAmOrPm == "PM")
                || checkAmOrPm == "AM") && formRequest.FormStatus.Status != "Approved")
                {
                    timeKeepingForm.IsPunished = true;
                    timeKeepingForm.PunishedTime = entry.PunishedTime;
                    timeKeepingForm.PunishedTime++;
                }
            }
            
            else
            {
                timeKeepingForm.IsPunished = true;
                timeKeepingForm.PunishedTime = entry.PunishedTime;
                timeKeepingForm.PunishedTime++;
            }

            timeKeepingForm.CopyTo(entry);

            dataContext.Entry(entry).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();
        }

        public async Task<List<TimeKeepingItem>> GetTimeKeepingInfos(Guid userId)
        {
            return await dataContext.Timekeepings.Where(x => x.UserId == userId)
                .Select(x => x.ConvertTo<TimeKeepingItem>())
                .ToListAsync();
        }

        public async Task<TimeKeepingDetail> ValidateCheckinToday(Guid userId)
        {
            var today = DateTime.Now.Date;
            var entry = await dataContext.Timekeepings.Where(x => x.UserId == userId && DateTime.Compare(today,x.CheckinTime.Value.Date) == 0)
                .FirstOrDefaultAsync();
            var test = await dataContext.Timekeepings.Select(x => x.CheckinTime.Value.Date).FirstOrDefaultAsync();
            var check = DateTime.Compare(test, today);
            if (entry == null)
            {
                throw new NotFoundException("This user didnt checkin today");
            }

            else
            {
                var des = entry.ConvertTo<TimeKeepingDetail>();
                return des;
            }
        }

        public async Task<List<TimeKeepingItem>> FilterTimeKeeping(TimeKeepingFilter timeKeepingFilter)
        {
            return (await dataContext.Timekeepings
                .Include(x => x.User)
                .Where(x => (timeKeepingFilter.DepartmentId.HasValue ? x.User.DepartmentId == timeKeepingFilter.DepartmentId.Value
                : x != null))
                .Where(x => timeKeepingFilter.UserId.HasValue ? x.UserId == timeKeepingFilter.UserId.Value
                : x != null)
                .Where(x => timeKeepingFilter.DateTime.HasValue ?
                (x.CheckinTime.Value.Month == timeKeepingFilter.DateTime.Value.Month
                 && x.CheckinTime.Value.Year == timeKeepingFilter.DateTime.Value.Year)
                : x != null)
                .Select(x => x.ConvertTo<TimeKeepingItem>())
                .ToListAsync());
        }

        public async Task DeletePunish(Guid id, FormRequestForm formRequestForm)
        {
            var entry = await dataContext.FormRequests.FindAsync(id) ??
                throw new NotFoundException("This request cannot be found");

            var timekeeping = await dataContext.Timekeepings
                .Where(x => x.UserId == formRequestForm.UserId
                && x.CheckinTime.Value.Day == formRequestForm.RequestDate.Day
                && x.CheckinTime.Value.Month == formRequestForm.RequestDate.Month
                && x.CheckinTime.Value.Year == formRequestForm.RequestDate.Year)
                .FirstOrDefaultAsync();

            if (timekeeping.PunishedTime < 1 && formRequestForm.RequestTypeId != 5)
            {
                throw new BadRequestException("This user didnt be punished this day");
            }

            if (formRequestForm.StatusId == 2 && formRequestForm.RequestTypeId == 5)
            {
                timekeeping.PunishedTime = timekeeping.PunishedTime - 1;
            }

            await base.Update(timekeeping.Id, timekeeping);
        }
    }
}
