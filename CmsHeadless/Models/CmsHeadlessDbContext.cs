using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmsHeadless.Models
{
    public class CmsHeadlessDbContext : IdentityDbContext<CmsUser>
    {
        public CmsHeadlessDbContext()
        {
        }
        public CmsHeadlessDbContext(DbContextOptions<CmsHeadlessDbContext> options)
            : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=192.168.10.73;Database=CmsHeadless;User ID=guest1;Password=Sautech15*");
                /*"Server=FABRIZIO\\SQLEXPRESS;Database=CmsHeadless2;Trusted_Connection=True;MultipleActiveResultSets=true"*/
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        //partial void OnModelCreating(ModelBuilder modelBuilder);
        public DbSet<Attributes> Attributes { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Content> Content { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<ContentAttributes> ContentAttributes { get; set; }
        public DbSet<ContentTag> ContentTag { get; set; }
        public DbSet<ContentCategory> ContentCategory { get; set; }
        public virtual DbSet<CmsUser> CmsUser { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<Nation> Nation { get; set; }
        public DbSet<ContentLocation> ContentLocation { get; set; }
        public DbSet<Typology> Typology { get; set; }
        public DbSet<AttributesTypology> AttributesTypology { get; set; }
        public DbSet<LogType> LogType { get; set; }
        public DbSet<LogEvent> LogEvent { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<AuthTokens> AuthTokens { get; set; }
        public DbSet<QrCode> QrCode { get; set; }

    }
    
}
