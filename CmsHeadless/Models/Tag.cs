using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class Tag
    {
        [Key]
        public int TagId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [DataType(DataType.Url)]
        [MaxLength(150)]
        public string Url { get; set; }
    }
}
