using IUstaProject.Models;
using IUstaProject.Models.Dtos;

namespace IUstaProject.Services
{
    public interface ILoginRegisterService
    {
        Task<bool> Register(AdminDto request);
        Task<string> Login(AdminDto request);
    }
}
