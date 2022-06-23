using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsHeadless.Models
{
    public class CmsHeadlessDbContext : DbContext
    {
        public CmsHeadlessDbContext()
        {
        }
        public CmsHeadlessDbContext(DbContextOptions options)
            : base(options)
        {
        }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Tag> Tag { get; set; }
    }

}
