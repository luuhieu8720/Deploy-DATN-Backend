using DATN_Back_end.Exceptions;
using DATN_Back_end.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public class ForgetPasswordService : IForgetPasswordService
    {
        private readonly DataContext dataContext;

        private const string validChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public ForgetPasswordService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ForgetPassword> CreateCode(string email)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Email == email)
                                        ?? throw new BadRequestException("The email address you entered couldn't be found.");

            var code = GenerateCode(6);

            var forgetPassword = new ForgetPassword
            {
                UserId = user.Id,
                Code = code,
                CreateDate = DateTime.Now
            };

            await dataContext.ForgetPasswords.AddAsync(forgetPassword);

            await dataContext.SaveChangesAsync();

            return forgetPassword;
        }

        public static string GenerateCode(int length)
        {
            return string.Concat(validChar.OrderBy(x => Guid.NewGuid()).Take(length));
        }
    }
}
