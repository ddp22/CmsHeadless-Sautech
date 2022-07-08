using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class AttributesTypology
    {
        [Key]
        public int AttributesTypologyId { get; set; }
        public int AttributesId { get; set; }
        public Attributes Attributes { get; set; } = null!;
        public int TypologyId { get; set; }
        public Typology Typology { get; set; } = null!;
    }
}
