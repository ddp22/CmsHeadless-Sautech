using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CmsHeadless.Models
{
    public class QrCode
    {
        [Key]
        public int QrCodeId { get; set; }
        [Index(IsUnique = true)]
        [Required]
        public string QrCodeLabel { get; set; }
        [ForeignKey("ContentId")]
        public int ContentId { get; set; }
    }
}
