using DATN_Back_end.Dto.DtoAuth;
using DATN_Back_end.Models;
using System.Threading.Tasks;

namespace DATN_Back_end.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<User> Login(LoginForm loginForm);
    }
}
