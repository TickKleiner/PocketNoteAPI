using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PocketNoteAPI.Models
{
    public class PocketNoteAPIContext : DbContext
    {
        public PocketNoteAPIContext(DbContextOptions<PocketNoteAPIContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Device>()
                .HasOne(p => p.PocketNoteAPIItem)
                .WithMany(b => b.Devices);
            modelBuilder.Entity<File>()
               .HasOne(p => p.PocketNoteAPIItem)
               .WithMany(b => b.Files);
            modelBuilder.Entity<Session>()
                .HasKey(c => new { c.DeviceId, c.GoogleUserId });
            modelBuilder.Entity<Session>()
                .HasOne(p => p.Device)
                .WithMany(b => b.Sessions);

        }

        public DbSet<PocketNoteAPIItem> PocketNoteAPIItems { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Session> Sessions { get; set; }
    }

    public class PocketNoteAPIContextFactory : IDesignTimeDbContextFactory<PocketNoteAPIContext>
    {
        public PocketNoteAPIContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<PocketNoteAPIContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new PocketNoteAPIContext(optionsBuilder.Options);
        }
    }
}