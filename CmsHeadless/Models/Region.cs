using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        [Required]
        public string RegionName { get; set; }
        //public virtual ICollection<Geolocation>? Geolocations { get; set; }
    }
}
