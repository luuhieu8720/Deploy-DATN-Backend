using DATN_Back_end.Config;
using DATN_Back_end.Dto.DtoMail;
using DATN_Back_end.Dto.DtoPassword;
using DATN_Back_end.Exceptions;
using DATN_Back_end.Extensions;
using DATN_Back_end.Models;
using DATN_Back_end.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public class PasswordService : IPasswordService
    {
        private readonly IForgetPasswordService forgetPasswordService;

        private readonly IUserRepository userRepository;

        private readonly IAuthenticationService authenticationService;

        private readonly ISendMailService sendMailService;

        private readonly DataContext dataContext;

        private readonly ApplicationConfig passwordConfig;

        public PasswordService(IForgetPasswordService forgetPasswordService,
            IUserRepository userRepository,
            ISendMailService sendMailService,
            DataContext dataContext,
            ApplicationConfig passwordConfig,
            IAuthenticationService authenticationService)
        {
            this.forgetPasswordService = forgetPasswordService;
            this.userRepository = userRepository;
            this.sendMailService = sendMailService;
            this.dataContext = dataContext;
            this.passwordConfig = passwordConfig;
            this.authenticationService = authenticationService;
        }

        public async Task Change(ChangePasswordForm changePasswordForm)
        {
            var user = await dataContext.Users
                .Include(a => a.ForgetPasswords)
                .FirstOrDefaultAsync(x => x.Email == changePasswordForm.Email)
                ?? throw new BadRequestException("The email address you entered couldn't be found.");

            var forgetPasswords = user.ForgetPasswords;

            var passwordForm = forgetPasswords.OrderBy(x => x.CreateDate)
                .LastOrDefault() ?? throw new BadRequestException("You haven't sent any request yet");

            if (!CanChangePasswordWithCode(passwordForm, changePasswordForm.Code))
            {
                throw new BadRequestException("Your code is not valid or has been expired");
            }

            user.Password = changePasswordForm.Password.Encrypt();
            dataContext.Users.Update(user);
            dataContext.ForgetPasswords.RemoveRange(forgetPasswords);
            await dataContext.SaveChangesAsync();
        }

        private bool CanChangePasswordWithCode(ForgetPassword passwordForm, string code)
        {
            var expireTime = passwordForm.CreateDate.AddSeconds(passwordConfig.ResetPasswordCodeExpire);
            return code == passwordForm.Code && expireTime > DateTime.Now;
        }

        public async Task Forget(string email)
        {
            var forgetPassword = await forgetPasswordService.CreateCode(email);
            var user = await userRepository.Get(email);

            var htmpTemplate = File.ReadAllText("Templates/forget_password.html");

            var subject = htmpTemplate.FindRegex("<title>(.*)</title>");

            var emailBody = htmpTemplate
                .Replace("@first_name@", user.FirstName)
                .Replace("@last_name@", user.LastName)
                .Replace("@email@", email)
                .Replace("@code@", forgetPassword.Code);

            var mailContent = new MailContent()
            {
                To = email,
                Subject = subject,
                Body = emailBody
            };

            await sendMailService.Send(mailContent);
        }

        public async Task Update(Guid id, UpdatePasswordForm updatePasswordForm)
        {
            var userId = id;
            var currentUser = await dataContext.Users.FindAsync(userId);

            if (currentUser.Password != updatePasswordForm.OldPassword.Encrypt())
                throw new BadRequestException("Old passwords don't match");

            if (updatePasswordForm.Password.CompareTo(updatePasswordForm.ConfirmPassword) != 0)
            {
                throw new BadRequestException("New passwords don't match");
            }

            currentUser.Password = updatePasswordForm.Password.Encrypt();
            dataContext.Entry(currentUser).State = EntityState.Modified;

            await dataContext.SaveChangesAsync();
        }
    }
}
