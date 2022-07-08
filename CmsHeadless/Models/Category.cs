using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; } = null!;
        [MaxLength(200)]
        public string? Description { get; set; }
        public int? CategoryParentId { get; set; }
        [MaxLength(150)]
        public string? Media { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public virtual ICollection<ContentCategory> ContentCategory { get; set; } = null!;
        public Category()
        {
        }

        public Category(int CategoryId, string Name, string? Description, int?CategoryParentId, string Media, DateTime CreationDate)
        {
            this.CategoryId = CategoryId;
            this.Name = Name;
            this.Description = Description;
            this.CategoryParentId = CategoryParentId;
            this.Media = Media;
            this.CreationDate = CreationDate;
        }
    }
}
