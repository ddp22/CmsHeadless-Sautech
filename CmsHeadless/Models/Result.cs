using System.ComponentModel.DataAnnotations;

namespace CmsHeadless.Models
{
    public class ResponseApi
    {
        public ResponseApi()
        {
        }
        public bool result { get; set; }
        public string details { get; set; } = null!;
        public User User { get; set; }


    }
}
