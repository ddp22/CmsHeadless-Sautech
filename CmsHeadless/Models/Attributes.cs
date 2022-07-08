using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Attributes
    {
        public Attributes()
        {
        }

        public Attributes(int attributesId, string attributeName, string attributeValue)
        {
            AttributesId = attributesId;
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }

        [Key]
        public int AttributesId { get; set; }
        [MaxLength(50)]
        public string AttributeName { get; set; }
        [MaxLength(150)]
        public string AttributeValue { get; set; }
        public ICollection<ContentAttributes> ContentAttributes { get; set; }
        public ICollection<AttributesTypology>? AttributesTypology { get; set; }
        public virtual Geolocation? Geolocation { get; set; }
    }
}
