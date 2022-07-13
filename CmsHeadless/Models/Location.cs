using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }
        public int? NationId { get; set; }
        public int? RegionId { get; set; }
        public int? ProvinceId { get; set; }
        
        public Nation? Nation { get; set; } = null;
        public Region? Region { get; set; } = null;
        public Province? Province { get; set; } = null;
        public string? City { get; set; }
        public virtual ICollection<ContentLocation>? ContentLocation { get; set; }
        public Location()
        {

        }
        public Location(int? nation, int? region, int? province, string? city)
        {
            NationId = nation;
            RegionId = region;
            ProvinceId = province;
            City = city;
        }
    }
}
