using CmsHeadless.Models;
using System.ComponentModel.DataAnnotations;

namespace CmsHeadlessApi.Models
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
