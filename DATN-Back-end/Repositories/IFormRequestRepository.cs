using DATN_Back_end.Dto.DtoFilter;
using DATN_Back_end.Dto.DtoFormRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IFormRequestRepository
    {
        Task Create(FormRequestForm formRequest);

        Task Update(Guid id, FormRequestForm formRequest);

        Task<FormRequestDetail> Get(Guid id);

        Task<List<FormRequestItem>> Get();

        Task<List<FormRequestDetail>> FilterRequest(RequestsFilter requestsFilter);

        Task<List<FormRequestDetail>> FilterRequestForUser(RequestsFilter requestsFilter);

        Task ConfirmRequest(Guid id, FormRequestConfirm formRequestConfirm);
    }
}
