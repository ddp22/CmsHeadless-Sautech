using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CmsHeadless.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(30)]
        public string? Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        public int? CategoryParentId { get; set; }
        [MaxLength(150)]
        public string? Media { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public ICollection<ContentCategory> ContentCategory { get; set; } = null!;
    }
}
