using IUstaProject.Data;
using IUstaProject.Models;
using IUstaProject.Models.Dtos;

namespace IUstaProject.Services
{
    public class LoginWorker : ILoginRegisterWorker
    {
        private readonly WorkerDbContext _context;
        public LoginWorker(WorkerDbContext context)
        {
            this._context = context;
        }
        public async Task<string> Login(WorkerDto request)
        {
            var user = _context.Workers.FirstOrDefault(U => U.UserName == request.UserName) ??
                throw new Exception("Username or Password is wrong!");
            if (!PasswordHash.ConfirmPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Username or Password is wrong!");
            var token = JWTTokenService.CreateToken(user);
            return token;
        }


        public async Task<bool> Register(WorkerDto request)
        {
            if (_context.Workers.Any(u => u.UserName == request.UserName))
                throw new Exception("Username is already taken!");

            PasswordHash.Create(request.Password, out byte[] PassHash, out byte[] PassSalt);

            Worker user = new()
            {
                UserName = request.UserName,
                PasswordHash = PassHash,
                PasswordSalt = PassSalt,
            };

            await _context.Workers.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
