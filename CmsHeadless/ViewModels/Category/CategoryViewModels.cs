using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels.Category
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryParentId { get; set; }

        public IFormFile? Media { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
    }
}