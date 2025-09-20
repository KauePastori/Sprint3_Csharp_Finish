using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.Id);
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.Property(b => b.Author).IsRequired().HasMaxLength(120);
            e.Property(b => b.Price).HasPrecision(10,2);
        });
    }
}

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _db;
    public BookRepository(AppDbContext db) => _db = db;

    public async Task<Book?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Books.FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<List<Book>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Books.OrderBy(b => b.Id).ToListAsync(ct);

    public async Task<Book> AddAsync(Book book, CancellationToken ct = default)
    {
        _db.Books.Add(book);
        await _db.SaveChangesAsync(ct);
        return book;
    }

    public async Task UpdateAsync(Book book, CancellationToken ct = default)
    {
        _db.Books.Update(book);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var book = await _db.Books.FindAsync(new object?[] { id }, ct);
        if (book is null) return;
        _db.Books.Remove(book);
        await _db.SaveChangesAsync(ct);
    }
}