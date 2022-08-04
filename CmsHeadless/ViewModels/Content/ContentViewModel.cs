using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels.Content
{
    public class ContentViewModel
    {
        public List<int>? ContentAttributes { get; set; }
        public List<int>? ContentCategory { get; set; }
        public List<int>? ContentTag { get; set; }
        public string Title { get; set; } = null!;
        public IFormFile? Media { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.MultilineText)]
        [Required]
        public string Text { get; set; }
        [DataType(DataType.Date)]
        public DateTime? PubblicationDate { get; set; }
        public int? Nation { get; set; }
        public int? Region { get; set; }
        public int? Province { get; set; }
        public string? City { get; set; }
        public string? QrCode { get; set; }
    }
}
