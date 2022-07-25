using CmsHeadless.Models;
using System.ComponentModel.DataAnnotations;

namespace CmsHeadlessApi.ModelsController
{
    public class UserControllerModel
    {
        public string UserId { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        public UserControllerModel(string userId, string username, string password)
        {
            UserId = userId;
            UserName = username;
            Password = password;
        }
    }
}
