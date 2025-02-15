using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Bicycle.Models;

namespace Bicycle.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    
    public DbSet<Review> Reviews { get; set; }
    public DbSet<FileModel> FileModel { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Review>()
            .HasOne(review => review.File)
            .WithMany()
            .HasForeignKey(review => review.FileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}