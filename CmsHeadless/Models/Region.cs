using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Region
    {
        [Key]
        public int RegionId { get; set; }
        [Required]
        //[Display(Name = "Nome Regione")]
        public string RegionName { get; set; }
        public virtual Nation Nation { get; set; }
        [ForeignKey("Nation")]
        //[Display(Name = "Nazione")]
        public int NationId { get; set; }
        //[Display(Name = "Regione Attiva")]
        public bool RegionIsActive { get; set; } = true;
    }
}
