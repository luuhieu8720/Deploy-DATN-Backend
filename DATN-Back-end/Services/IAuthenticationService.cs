using DATN_Back_end.Dto.DtoAuth;
using System;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> Login(LoginForm loginForm);

        AuthenUser CurrentUser { get; }

        Guid CurrentUserId { get; }
    }
}