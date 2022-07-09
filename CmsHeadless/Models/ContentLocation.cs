using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class ContentLocation
    {
        [Key]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public int LocationId { get; set; }
        public Location Location { get; set; } = null!;
    }
}
