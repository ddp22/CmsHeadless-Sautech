using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{

    public class Content
    {
        [Key]
        public int ContentId { get; set; }
        [MaxLength(50)]
        public string? Title { get; set; }
        [MaxLength(150)]
        public string? Media { get; set; }
        [MaxLength(100)]
        public string? Description { get; set; }

        [DataType(DataType.Date)]
        public DateTime InsertionDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Text { get; set; }

        [Required]
        public string UserId { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastEdit { get; set; }
        
        public virtual ICollection<ContentAttributes> ContentAttributes { get; set; } = null!;
        public virtual ICollection<ContentCategory> ContentCategory { get; set; } = null!;
        public virtual ICollection<ContentTag> ContentTag { get; set; } = null!;

    }



    

    
}