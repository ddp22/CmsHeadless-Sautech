using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class ContentTag
    {
        [Key]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public int TagId { get; set; }
        public Tag Tag { get; set; } = null!;
    }
}
