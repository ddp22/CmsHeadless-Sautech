using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace CmsHeadless.Models


{
    public class ContentCategory
    {
        [Key]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public Content Content { get; set; } = null!;

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
