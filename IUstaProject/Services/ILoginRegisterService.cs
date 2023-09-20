using IUstaProject.Models;
using IUstaProject.Models.Dtos;

namespace IUstaProject.Services
{
    public interface ILoginRegisterService
    {
        Task<bool> Register(UserDto request);
        Task<string> Login(UserDto request);
    }
}
