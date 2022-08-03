using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels.Attributes
{
    public class EditPOIViewModel
    {
        [Required]
        public string AttributeValue { get; set; } = null!;
        public List<int>? Typology { get; set; }
    }
}