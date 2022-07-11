using DATN_Back_end.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public interface IForgetPasswordService
    {
        Task<ForgetPassword> CreateCode(string email);
    }
}
