using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Geolocation
    {
        [Key]
        public int GeolocationId { get; set; }
        //[Required]
        //public virtual Nation Nation { get; set; }
        //[Required]
        //public virtual Region Region { get; set; }
        //public virtual City City { get; set; }
        public virtual ICollection<AttributesGeolocation>? AttributesGeolocation { get; set; }
        public virtual ICollection<User>? User { get; set; }
    }
}
