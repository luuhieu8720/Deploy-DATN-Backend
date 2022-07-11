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
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private DataContext dataContext;

        public Repository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task Create(object source)
        {
            await dataContext.Set<T>().AddAsync(source.ConvertTo<T>());

            await dataContext.SaveChangesAsync();
        }

        public async Task<List<O>> Get<O>()
        {
            return await dataContext.Set<T>()
                .Select(x => x.ConvertTo<O>())
                .ToListAsync();
        }

        public async Task<O> Get<O>(Guid id)
        {
            var entry = await GetByIdOrThrow(id);

            return entry.ConvertTo<O>();
        }

        public async Task<T> GetByIdOrThrow(Guid id)
        {
            return await dataContext.Set<T>().FindAsync(id) ?? 
                throw new NotFoundException("Item can't be found");
        }

        public async Task Update(Guid id, object source)
        {
            var entry = await GetByIdOrThrow(id);
            source.CopyTo(entry);
            dataContext.Entry(entry).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();
        }

        public virtual async Task Delete(Guid id)
        {
            var entry = await GetByIdOrThrow(id);

            dataContext.Remove(entry);

            await dataContext.SaveChangesAsync();
        }
    }
}
