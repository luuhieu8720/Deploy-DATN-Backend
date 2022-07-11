using DATN_Back_end.Dto.DtoUser;
using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid Id);

        Task<UserDetail> Get(Guid Id);

        Task Create(UserFormCreate userForm);

        Task<User> Get(string email);

        Task Update(Guid id, UserFormUpdate userFormUpdate);

        Task<List<UserItem>> GetAll();

        Task<List<UserItem>> GetUserByDepartmentId(Guid id);
    }
}
