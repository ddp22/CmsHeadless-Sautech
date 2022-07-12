using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class Nation
    {
        [Key]
        public int NationId { get; set; }
        [Required]
        //[Display(Name = "Nome Nazioone")]
        public string NationName { get; set; }
        //[Display(Name = "Nazione Attiva")]
        public bool NationIsActive { get; set; } = true;
    }
}
