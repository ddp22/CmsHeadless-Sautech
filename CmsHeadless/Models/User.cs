using CmsHeadless.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace CmsHeadless.Models
{
    public class User : IdentityUser

    {
        public string Nickname { get; set; }
        public double LatitudeUser { get; set; }
        public double LongitudeUser { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public virtual ICollection<Content>? Content { get; set; }
        public virtual ICollection<UserTypology>? UserTypology { get; set; }
        public virtual Location? Location { get; set; }
    }
}