using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Book>()
        .HasIndex(b => new { b.Title, b.Author })
        .IsUnique();
}
    public DbSet<Book> Books { get; set; }
}