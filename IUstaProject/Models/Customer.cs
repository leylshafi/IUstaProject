namespace IUstaProject.Models
{
    public class Customer:User
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
