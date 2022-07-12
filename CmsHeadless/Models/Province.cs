using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Province
    {
        [Key]
        public int ProvinceId { get; set; }
        [Required]
        //[Display(Name = "Nome Provincia")]
        public string ProvinceName { get; set; }
        public virtual Region Region { get; set; }
        [ForeignKey("Region")]
        //[Display(Name = "Regione")]
        public int RegionId { get; set; }
        //[Display(Name = "Provincia Attiva")]
        public bool ProvinceIsActive { get; set; } = true;
    }
}
