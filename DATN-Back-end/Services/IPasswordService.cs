using DATN_Back_end.Dto.DtoPassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public interface IPasswordService
    {
        Task Forget(string email);

        Task Change(ChangePasswordForm changePasswordForm);

        Task Update(Guid id, UpdatePasswordForm updatePasswordForm);
    }
}
