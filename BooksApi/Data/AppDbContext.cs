using BooksApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
}