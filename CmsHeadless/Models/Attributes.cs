using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Attributes
    {
        [Key]
        public int AttributesId { get; set; }
        [MaxLength(50)]
        public string AttributeName { get; set; }
        [MaxLength(150)]
        public string AttributeValue { get; set; }
        public ICollection<ContentAttributes> ContentAttributes { get; set; } = null!;

    }
}
