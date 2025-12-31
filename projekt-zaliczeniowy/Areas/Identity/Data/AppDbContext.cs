using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using projekt_zaliczeniowy.Areas.Identity.Data;
using projekt_zaliczeniowy.Models;
using System.Reflection.Emit;

namespace projekt_zaliczeniowy.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<Categories> Category { get; set; }
    public DbSet<History> History { get; set; }
    public DbSet<Books> Book { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<Categories>().HasData(
            new Categories { Id = 1, Nazwa = "Kryminał" },
            new Categories { Id = 2, Nazwa = "Dokument" },
            new Categories { Id = 3, Nazwa = "Naukowe" }
        );
        builder.Entity<Books>().HasData(
            new Books { Id = 1, Nazwa = "Tytuł Kryminału", Autor="Jan Kowalski", CategoryId=1, Regal=120 },
            new Books { Id = 2, Nazwa = "Tytuł Dokumentu", Autor = "Jan Nowak", CategoryId = 2, Regal = 130 },
            new Books { Id = 3, Nazwa = "Tytuł Książki Naukowej", Autor = "Jan Woźniak", CategoryId = 3, Regal = 10 }
        );
    }

    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(255);
        builder.Property(x => x.LastName).HasMaxLength(255);
    }

}