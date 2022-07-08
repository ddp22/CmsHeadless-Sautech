using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class Typology
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<UserTypology>? UserTypology { get; set; }
        public virtual ICollection<AttributesTypology>? AttributesTypology { get; set; }
    }
}
