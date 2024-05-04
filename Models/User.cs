using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Introduce { get; set; }
    }
}
