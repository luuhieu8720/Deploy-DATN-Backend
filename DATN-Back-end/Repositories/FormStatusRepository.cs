using DATN_Back_end.Dto.DtoStatus;
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
    public class FormStatusRepository : IFormStatusRepository
    {
        private DataContext dataContext;

        public FormStatusRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(FormStatusForm formStatusForm)
        {
            await dataContext.FormStatuses
                .AddAsync(formStatusForm.ConvertTo<FormStatus>());

            await dataContext.SaveChangesAsync();
        }

        public async Task<List<FormStatusItem>> Get()
        {
            return await dataContext.FormStatuses
                .Select(x => x.ConvertTo<FormStatusItem>())
                .ToListAsync();
        }

        public async Task<FormStatusDetail> Get(int id)
        {
            var result = await dataContext.FormStatuses.FindAsync(id)
                ?? throw new NotFoundException("This form status cannot be found");

            return result.ConvertTo<FormStatusDetail>();
        }

        public async Task Update(int id, FormStatusForm formStatusForm)
        {
            var entry = await dataContext.FormStatuses.FindAsync(id)
               ?? throw new NotFoundException("This form status cannot be found");

            formStatusForm.CopyTo(entry);
            dataContext.Entry(entry).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();
        }
    }
}
