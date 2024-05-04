using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace News.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Tiêu đề")]
        public string? Title { get; set; }
        [DisplayName("Mô tả")]
        public string? Description { get; set; }
        [DisplayName("Nội dung")]
        public string? Content { get; set; }
        [DisplayName("Ngày đăng")]
        public DateOnly? DateCreated { get; set; }
        [DisplayName("Đường dẫn ảnh")]
        public string? ImageUrl { get; set; }
        [ForeignKey("User")]
        [DisplayName("Tác giả")]
        public int? AuthorId { get; set; }
        public virtual User? User { get; set; }
        [ForeignKey("Category")]
        [DisplayName("Danh mục")]
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        [DisplayName("Bài nổi bật")]
        public bool? IsFeatured { get; set; }
        [DisplayName("Đường dẫn ảnh bé")]
        public string? ThumbUrl { get; set; }
        [DisplayName("Đã kiểm duyệt")]
        public int? IsCensored { get; set; }
        public Article()
        {
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        }
        public string GetCensorStatus()
        {
            switch (IsCensored)
            {
                case 0: 
                    return "Đang chờ kiểm duyệt";
                case 1:
                    return "Đã kiểm duyệt";
                case 2:
                    return "Không được kiểm duyệt";
                default:
                    return "Không xác định";
            }
        }
        public string GetFeaturedStatus()
        {
            return IsFeatured.HasValue && IsFeatured.Value? "True": "False";
        }
    }
}
