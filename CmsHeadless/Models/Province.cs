using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        [Required]
        public string ProvinceName { get; set; }
        public virtual Region Region { get; set; }
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public bool ProvinceIsActive { get; set; } = true;
    }
}
