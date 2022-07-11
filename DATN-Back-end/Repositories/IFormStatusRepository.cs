using DATN_Back_end.Dto.DtoStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IFormStatusRepository
    {
        Task Create(FormStatusForm formStatusForm);

        Task<List<FormStatusItem>> Get();

        Task<FormStatusDetail> Get(int id);

        Task Update(int id, FormStatusForm formStatusForm);
    }
}
