using DATN_Back_end.Config;
using DATN_Back_end.Dto.DtoAuth;
using DATN_Back_end.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace DATN_Back_end.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly TokenConfig tokenConfig;

        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IAuthenticationRepository authenticationRepository;

        public AuthenticationService(TokenConfig tokenConfig,
            IHttpContextAccessor httpContextAccessor,
            IAuthenticationRepository authenticationRepository)
        {
            this.tokenConfig = tokenConfig;
            this.httpContextAccessor = httpContextAccessor;
            this.authenticationRepository = authenticationRepository;
        }

        public AuthenUser CurrentUser => ((UserClaimsPrincipal)httpContextAccessor.HttpContext.User).AuthenUser;

        public Guid CurrentUserId => CurrentUser.Id;

        public async Task<TokenResponse> Login(LoginForm loginForm)
        {
            var user = await authenticationRepository.Login(loginForm);

            var authenUser = new AuthenUser(user);

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfig.Key));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var tokenString = new JwtSecurityToken(tokenConfig.Issuer, tokenConfig.Audience, claims: authenUser.GetClaims(), signingCredentials: signingCredentials);

            return new TokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenString),
                UserId = authenUser.Id,
                FirstName = authenUser.FirstName,
                LastName = authenUser.LastName,
                Email = authenUser.Email,
                Username = authenUser.Username,
                Role = authenUser.Role
            };
        }
    }
}
