using IUstaProject.Models.Dtos;

namespace IUstaProject.Services
{
    public interface ILoginRegisterWorker
    {
        Task<bool> Register(WorkerDto request);
        Task<string> Login(WorkerDto request);
    }
}
