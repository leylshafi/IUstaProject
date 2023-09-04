using System.Reflection.PortableExecutable;

namespace IUstaProject.Models
{
    public class Worker:User
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
