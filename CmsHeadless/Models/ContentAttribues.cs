using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class ContentAttributes
    {
        [Key]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public int AttributeId { get; set; }
        public Attributes Attributes { get; set; } = null!;
    }
}
