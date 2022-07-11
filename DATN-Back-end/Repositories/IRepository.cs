using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IRepository<T> where T : BaseModel
    {
        Task Create(object source);

        Task<List<O>> Get<O>();

        Task<O> Get<O>(Guid id);

        Task Update(Guid id, object source);

        Task<T> GetByIdOrThrow(Guid id);

        Task Delete(Guid id);
    }
}
