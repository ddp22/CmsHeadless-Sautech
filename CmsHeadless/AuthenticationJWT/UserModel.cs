using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.AuthenticationJWT
{
    public class UserModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }

}
