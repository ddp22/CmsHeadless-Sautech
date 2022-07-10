using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.ViewModels.Typology
{
    public class EditTypologyViewModel
    {
        [Required]
        public string TypologyName { get; set; } = null!;
    }
}
