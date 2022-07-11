using DATN_Back_end.Dto.DtoRequestType;
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
    public class RequestTypeRepository : IRequestTypeRepository
    {
        private DataContext dataContext;

        public RequestTypeRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(RequestTypeForm requestTypeForm)
        {
            await dataContext.RequestTypes
                .AddAsync(requestTypeForm.ConvertTo<RequestType>());

            await dataContext.SaveChangesAsync();
        }

        public async Task<List<RequestTypeItem>> Get()
        {
            return await dataContext.RequestTypes
                .Select(x => x.ConvertTo<RequestTypeItem>())
                .ToListAsync();
        }

        public async Task<RequestTypeDetail> Get(int id)
        {
            var result = await dataContext.RequestTypes.FindAsync(id)
                ?? throw new NotFoundException("This request type cannot be found");

            return result.ConvertTo<RequestTypeDetail>();
        }

        public async Task Update(int id, RequestTypeForm requestTypeForm)
        {
            var entry = await dataContext.FormStatuses.FindAsync(id)
               ?? throw new NotFoundException("This request type cannot be found");

            requestTypeForm.CopyTo(entry);
            dataContext.Entry(entry).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();
        }
    }
}
