using Microsoft.AspNetCore.Identity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUser>().ToTable("User");
        }
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<ContentAttributes> ContentAttributes { get; set; }
        public DbSet<ContentTag> ContentTag { get; set; }
        public DbSet<ContentCategory> ContentCategory { get; set; }
        public  DbSet<User> User { get; set; }
        public  DbSet<Location> Location { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Nation> Nation { get; set; }
        public DbSet<ContentLocation> ContentLocation { get; set; }
        public DbSet<Typology> Typology { get; set; }
        public DbSet<AttributesTypology> AttributesTypology { get; set; }
    }
    
}
