
using Microsoft.EntityFrameworkCore;

namespace Sprint3WinForms;

/// <summary>EF Core DbContext using a local SQLite file.</summary>
public class AppDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Title).IsRequired().HasMaxLength(200);
            e.Property(x => x.Author).IsRequired().HasMaxLength(120);
            e.Property(x => x.Price).HasPrecision(10,2);
        });
    }
}
