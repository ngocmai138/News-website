using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class Subscription
    {
        [Key]
        public int Id {  get; set; }
        public string? Email {  get; set; }
        [DisplayName("Ngày đăng ký")]
        public DateOnly DateSubscribed { get; set; }
    }
}
