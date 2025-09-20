using Application.Files;
using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Infrastructure;

public class FileService : IFileService
{
    private readonly AppDbContext _db;
    private readonly string _dataDir;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    { WriteIndented = true };

    public FileService(AppDbContext db)
    {
        _db = db;
        _dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(_dataDir);
    }

    public async Task<int> ImportBooksFromJsonAsync(Stream jsonStream, CancellationToken ct = default)
    {
        var items = await JsonSerializer.DeserializeAsync<List<Book>>(jsonStream, _jsonOptions, ct) 
            ?? new List<Book>();
        await _db.Books.AddRangeAsync(items, ct);
        var count = await _db.SaveChangesAsync(ct);
        await AppendAuditLogAsync($"Imported {items.Count} books from JSON", ct);
        return count;
    }

    public async Task<byte[]> ExportBooksToJsonAsync(CancellationToken ct = default)
    {
        var books = await _db.Books.AsNoTracking().ToListAsync(ct);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(books, _jsonOptions);
        await AppendAuditLogAsync($"Exported {books.Count} books to JSON", ct);
        return bytes;
    }

    public async Task<byte[]> ExportBooksToTxtAsync(CancellationToken ct = default)
    {
        var sb = new StringBuilder();
        var books = await _db.Books.AsNoTracking().ToListAsync(ct);
        foreach (var b in books)
            sb.AppendLine($"{b.Id}	{b.Title}	{b.Author}	{b.Price}	{b.PublishedOn:yyyy-MM-dd}");
        await AppendAuditLogAsync($"Exported {books.Count} books to TXT", ct);
        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    public async Task AppendAuditLogAsync(string message, CancellationToken ct = default)
    {
        var line = $"[{DateTime.UtcNow:O}] {message}";
        var logPath = Path.Combine(_dataDir, "audit.log");
        await File.AppendAllTextAsync(logPath, line + Environment.NewLine, Encoding.UTF8, ct);
    }
}