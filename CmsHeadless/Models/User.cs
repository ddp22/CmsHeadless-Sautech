using CmsHeadless.Models;
using Microsoft.AspNetCore.Identity;

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
        
        public virtual ICollection<Content> Content { get; set; }
    }
}