using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CmsHeadless.Models
{
    public class AuthTokens
    {
        [Key]
        [ForeignKey("Id")]
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
