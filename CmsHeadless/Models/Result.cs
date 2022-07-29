using Microsoft.AspNetCore.Identity;
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
        public string token { get; set; } = null!;
        public string role { get; set; } = null!;
        public CmsUser User { get; set; }
        


    }
}
