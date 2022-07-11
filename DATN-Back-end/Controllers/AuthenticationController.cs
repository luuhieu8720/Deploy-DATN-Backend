using DATN_Back_end.Dto.DtoAuth;
using DATN_Back_end.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Controllers
{
    [Route("api/auths")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<TokenResponse> Post([FromBody] LoginForm loginForm) => await authenticationService.Login(loginForm);

        [HttpGet]
        [Authorize]
        public AuthenUser Get() => authenticationService.CurrentUser;
    }
}
