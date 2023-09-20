namespace IUstaProject.Models
{
    public class AgreementRequest
    {
        public Guid CustomerId { get; set; }
        public Guid WorkerId { get; set; }   
        public string AgreementText { get; set; } 
    }
}
