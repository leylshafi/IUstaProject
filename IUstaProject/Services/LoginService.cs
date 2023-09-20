using IUstaProject.Data;
using IUstaProject.Models;
using IUstaProject.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using System;

namespace IUstaProject.Services
{
    public class LoginRegisterService : ILoginRegisterService
    {
        private readonly WorkerDbContext _context;
        public LoginRegisterService(WorkerDbContext context)
        {
            this._context = context;
        }
        public async Task<string> Login(UserDto request)
        {
            var user = _context.Admins.FirstOrDefault(U => U.UserName == request.UserName) ??
                throw new Exception("Username or Password is wrong!");
            if (!PasswordHash.ConfirmPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                throw new Exception("Username or Password is wrong!");
            var token = JWTTokenService.CreateToken(user);
            return token;
        }


        public async Task<bool> Register(UserDto request)
        {
            if (_context.Admins.Any(u => u.UserName == request.UserName))
                throw new Exception("Username is already taken!");

            PasswordHash.Create(request.Password, out byte[] PassHash, out byte[] PassSalt);

            Admin user = new()
            {
                UserName = request.UserName,
                PasswordHash = PassHash,
                PasswordSalt = PassSalt,
            };

            await _context.Admins.AddAsync(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
