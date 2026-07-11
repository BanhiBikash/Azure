using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Person
    {
        [Key]
        public Guid pId { get; set; } = Guid.NewGuid();

        public string? name {  get; set; }

        public string? email { get; set; }
    }
}
