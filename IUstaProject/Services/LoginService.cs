using IUstaProject.Data;
using IUstaProject.Models;
using IUstaProject.Models.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Xml;

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
            if (_context.Admins.Any(u => u.UserName == request.UserName) && request.Role == "Admin" ||
                _context.Workers.Any(u => u.UserName == request.UserName) && request.Role == "Worker" ||
                _context.Customers.Any(u => u.UserName == request.UserName) && request.Role == "Customer")
            {
                throw new Exception("Username is already taken for the specified role!");
            }

            PasswordHash.Create(request.Password, out byte[] PassHash, out byte[] PassSalt);

            User user = new()
            {
                UserName = request.UserName,
                PasswordHash = PassHash,
                PasswordSalt = PassSalt,
            };

            switch (request.Role)
            {
                case "Admin":
                    Admin admin = new()
                    {
                        Id = user.Id,
                        PasswordHash = user.PasswordHash,
                        PasswordSalt = user.PasswordSalt,
                        UserName = user.UserName,
                    };
                    await _context.Admins.AddAsync(admin);
                    break;
                case "Worker":
                    try
                    {
                        Worker worker = new Worker()
                        {
                            Id = user.Id,
                            PasswordHash = user.PasswordHash,
                            PasswordSalt = user.PasswordSalt,
                            UserName = user.UserName,
                            CategoryId = Guid.Parse("dbf918f8-add9-4d3e-a698-1eb0f6305502")
                        };
                        await _context.Workers.AddAsync(worker);
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.InnerException?.Message);
                        throw;
                    }
                    
                    break;
                case "Customer":
                    Customer customer = new()
                    {
                        Id = user.Id,
                        PasswordHash = user.PasswordHash,
                        PasswordSalt = user.PasswordSalt,
                        UserName = user.UserName,
                    };
                    await _context.Customers.AddAsync(customer);

                    break;
                default:
                    throw new Exception("Invalid role specified.");
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
