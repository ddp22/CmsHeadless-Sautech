using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels.Attributes
{
    public class AttributesViewModel
    {
        [Required]
        public string AttributeName { get; set; } = null!;
        [Required]
        public string AttributeValue { get; set; } = null!;
    }
}