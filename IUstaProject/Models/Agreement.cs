namespace IUstaProject.Models
{
    public class Agreement
    {
        public int Id { get; set; } 
        public Guid CustomerId { get; set; }
        public Guid WorkerId { get; set; }
        public string AgreementText { get; set; } 
        public DateTime DateCreated { get; set; } 

    }
}
