using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels
{
    public class CategoryViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? CategoryParentId { get; set; }
        
        public IFormFile? Media { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
    }
}