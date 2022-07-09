using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class Nation
    {
        [Key]
        public int NationId { get; set; }
        [Required]
        public string NationName { get; set; }
        public bool NationIsActive { get; set; } = true;
    }
}
