using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tên")]
        public string? Name { get; set; }
        public string? Email { get; set; }
        [DisplayName("Mật khẩu")]
        public string? Password { get; set; }
        [DisplayName("Quyền")]
        public string? Role { get; set; }
        [DisplayName("Ảnh đại diện")]
        public string? AvatarUrl { get; set; }
        [DisplayName("Giới thiệu")]
        public string? Introduce { get; set; }
    }
}
