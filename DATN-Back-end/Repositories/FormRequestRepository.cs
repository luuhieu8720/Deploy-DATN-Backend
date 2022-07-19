using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoFormRequest;
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
    public class FormRequestRepository : Repository<FormRequest>, IFormRequestRepository
    {
        private DataContext dataContext;

        public FormRequestRepository(DataContext dataContext) : base(dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(FormRequestForm formRequest)
        {
            formRequest.RequestDate = DateTime.Parse(formRequest.RequestDate.ToString()).ToLocalTime();
            formRequest.SubmittedTime = DateTime.Now;

            await base.Create(formRequest);
        }

        public async Task<List<FormRequestDetail>> FilterRequest(RequestsFilter requestsFilter)
        {
            return await dataContext.FormRequests
                .Where(x => (requestsFilter.DepartmentId.HasValue ? x.User.DepartmentId == requestsFilter.DepartmentId.Value 
                : x != null))
                .Where(x => requestsFilter.UserId.HasValue ? x.UserId == requestsFilter.UserId.Value
                : x != null)
                .Where(x => requestsFilter.DateTime.HasValue ? 
                (x.SubmittedTime.Day == requestsFilter.DateTime.Value.Day
                 && x.SubmittedTime.Month == requestsFilter.DateTime.Value.Month
                && x.SubmittedTime.Year == requestsFilter.DateTime.Value.Year)
                : x != null)
                .Where(x => requestsFilter.FormStatusId.HasValue ? x.FormStatus.Id == requestsFilter.FormStatusId.Value
                : x != null)
                .Where(x => requestsFilter.TypeId.HasValue ? x.RequestTypeId == requestsFilter.TypeId.Value
                : x != null)
                .Include(x => x.FormStatus)
                .Include(x => x.RequestType)
                .Include(x => x.User)
                .Select(x => x.ConvertTo<FormRequestDetail>())
                .ToListAsync();
        }

        public async Task<FormRequestDetail> Get(Guid id)
        {
            var entry = await dataContext.FormRequests
                .Include(x => x.FormStatus)
                .Include(x => x.RequestType)
                .Include(x => x.User)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (entry == null)
            {
                throw new NotFoundException("This form request cannot be found");
            }

            return entry.ConvertTo<FormRequestDetail>();
        }

        public async Task<List<FormRequestItem>> Get()
        {
            return await dataContext.FormRequests
                .Include(x => x.FormStatus)
                .Select(x => x.ConvertTo<FormRequestItem>())
                .ToListAsync();
        }

        public async Task Update(Guid id, FormRequestForm formRequest)
        {
            formRequest.UpdatedTime = DateTime.Now;

            await base.Update(id, formRequest);
        }

        public async Task<List<FormRequestDetail>> FilterRequestForUser(RequestsFilter requestsFilter)
        {
            return await dataContext.FormRequests
                .Where(x => (requestsFilter.DepartmentId.HasValue ? x.User.DepartmentId == requestsFilter.DepartmentId.Value
                : x != null))
                .Where(x => requestsFilter.UserId.HasValue ? x.UserId == requestsFilter.UserId.Value
                : x != null)
                .Where(x => requestsFilter.DateTime.HasValue ?
                (x.SubmittedTime.Month == requestsFilter.DateTime.Value.Month
                && x.SubmittedTime.Year == requestsFilter.DateTime.Value.Year)
                : x != null)
                .Where(x => requestsFilter.FormStatusId.HasValue ? x.FormStatus.Id == requestsFilter.FormStatusId.Value
                : x != null)
                .Where(x => requestsFilter.TypeId.HasValue ? x.RequestTypeId == requestsFilter.TypeId.Value
                : x != null)
                .Include(x => x.FormStatus)
                .Include(x => x.RequestType)
                .Include(x => x.User)
                .Select(x => x.ConvertTo<FormRequestDetail>())
                .ToListAsync();
        }

        public async Task ConfirmRequest(Guid id, FormRequestConfirm formRequestConfirm)
        {
            var entry = await dataContext.FormRequests.FindAsync(id) ??
                throw new NotFoundException("This request cannot be found");

            var timekeeping = await dataContext.Timekeepings
                .Where(x => x.UserId == entry.UserId
                && x.CheckinTime.Value.Day == entry.RequestDate.Day
                && x.CheckinTime.Value.Month == entry.RequestDate.Month
                && x.CheckinTime.Value.Year == entry.RequestDate.Year)
                .FirstOrDefaultAsync();

            if (timekeeping != null)
            {
                if (timekeeping.PunishedTime < 1 && entry.RequestTypeId != 5 && entry.RequestTypeId != 6)
                {
                    throw new BadRequestException("This user didnt be punished this day");
                }

                if (formRequestConfirm.StatusId == 2 && timekeeping.PunishedTime != 0)
                {
                    timekeeping.PunishedTime = timekeeping.PunishedTime - 1;
                }

                var entryTimeKeeping = await dataContext.Timekeepings.FindAsync(timekeeping.Id);
                timekeeping.CopyTo(entryTimeKeeping);
                dataContext.Entry(entryTimeKeeping);
            }

            dataContext.Entry(entry).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();

            await base.Update(id, formRequestConfirm);


        }
    }
}
