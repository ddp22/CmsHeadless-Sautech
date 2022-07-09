using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        [Required]
        public string RegionName { get; set; }
        public virtual Nation Nation { get; set; }
        [ForeignKey("Nation")]
        public int NationId { get; set; }
        public bool RegionIsActive { get; set; } = true;
    }
}
