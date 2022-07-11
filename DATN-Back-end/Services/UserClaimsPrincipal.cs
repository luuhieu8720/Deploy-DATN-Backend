using DATN_Back_end.Dto.DtoAuth;
using DATN_Back_end.Models;
using System.Security.Claims;

namespace DATN_Back_end.Services
{
    public class UserClaimsPrincipal : ClaimsPrincipal
    {
        public AuthenUser AuthenUser { get; private set; }
        public UserClaimsPrincipal(ClaimsIdentity claimsIdentity, User user) : base(claimsIdentity)
        {
            AuthenUser = new AuthenUser(claimsIdentity, user);
        }
    }
}
