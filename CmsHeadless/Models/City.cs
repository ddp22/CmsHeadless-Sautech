using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }
        [Required]
        public string CityName { get; set; }
        //public virtual ICollection<Geolocation>? Geolocations { get; set; }
    }
}
