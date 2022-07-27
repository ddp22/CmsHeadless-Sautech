using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class UserTypology
    {
        [Key]
        public int UserTypologyId { get; set; }
        public string UserId { get; set; }
        public CmsUser User { get; set; } = null!;
        public int TypologyId { get; set; }
        public Typology Typology { get; set; } = null!;

    }
}
