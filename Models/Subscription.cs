using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class Subscription
    {
        [Key]
        public int Id {  get; set; }
        public string? Email {  get; set; }
        public DateOnly DateSubscribed { get; set; }
    }
}
