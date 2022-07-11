using DATN_Back_end.Dto.DtoRequestType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IRequestTypeRepository
    {
        Task Create(RequestTypeForm requestTypeForm);

        Task<List<RequestTypeItem>> Get();

        Task<RequestTypeDetail> Get(int id);

        Task Update(int id, RequestTypeForm requestTypeForm);
    }
}
