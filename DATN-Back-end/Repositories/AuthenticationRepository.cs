using DATN_Back_end.Dto.DtoAuth;
using DATN_Back_end.Exceptions;
using DATN_Back_end.Extensions;
using DATN_Back_end.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext dataContext;

        public AuthenticationRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<User> Login(LoginForm loginForm)
        {
            return await dataContext.Users
                            .FirstOrDefaultAsync(x => x.Email == loginForm.EmailOrUsername || x.Username == loginForm.EmailOrUsername && x.Password == loginForm.Password.Encrypt())
                           ?? throw new BadRequestException("Wrong email or password");
        }
    }
}
