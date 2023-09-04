namespace IUstaProject.Models
{
    public class Admin:User
    {
        
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
