using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class AttributesGeolocation
    {
        [Key]
        public int AttributesGeolocationId { get; set; }
        public int GeolocationId { get; set; }
        public Geolocation Geolocation { get; set; } = null!;
        public int AttributesId { get; set; }
        public Attributes Attributes { get; set; } = null!;

    }
}
