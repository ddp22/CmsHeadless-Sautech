using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.ViewModels.Typology
{
    public class TypologyViewModel
    {
        [Required]
        public string TypologyName { get; set; } = null!;
    }
}
