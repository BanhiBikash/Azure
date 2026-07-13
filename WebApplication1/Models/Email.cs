namespace WebApplication1.Models
{
    public class Email
    {
        public string EmailAddress { get; set; }

        public string? Subject { get; set; }

        public DateTime TimeStamp { get; set; }

        public string? Message { get; set; } 
    }
}
