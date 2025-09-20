
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Sprint3WinForms;

/// <summary>Handles import/export and audit log.</summary>
public class FileService
{
    private readonly AppDbContext _db;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web){ WriteIndented = true };
    private readonly string _dataDir;

    public FileService(AppDbContext db)
    {
        _db = db;
        _dataDir = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(_dataDir);
    }

    public async Task<int> ImportJsonAsync(string filePath, CancellationToken ct = default)
    {
        using var fs = File.OpenRead(filePath);
        var items = await JsonSerializer.DeserializeAsync<List<Book>>(fs, _json, ct) ?? new();
        await _db.Books.AddRangeAsync(items, ct);
        var count = await _db.SaveChangesAsync(ct);
        await AppendAuditAsync($"Importou {items.Count} livros de {Path.GetFileName(filePath)}", ct);
        return count;
    }

    public async Task ExportJsonAsync(string filePath, CancellationToken ct = default)
    {
        var list = await _db.Books.AsNoTracking().ToListAsync(ct);
        var json = JsonSerializer.Serialize(list, _json);
        await File.WriteAllTextAsync(filePath, json, ct);
        await AppendAuditAsync($"Exportou {list.Count} livros para JSON ({Path.GetFileName(filePath)})", ct);
    }

    public async Task ExportTxtAsync(string filePath, CancellationToken ct = default)
    {
        var list = await _db.Books.AsNoTracking().ToListAsync(ct);
        var sb = new StringBuilder();
        foreach (var b in list)
            sb.AppendLine($"{b.Id}\t{b.Title}\t{b.Author}\t{b.Price}\t{b.PublishedOn:yyyy-MM-dd}");
        await File.WriteAllTextAsync(filePath, sb.ToString(), ct);
        await AppendAuditAsync($"Exportou {list.Count} livros para TXT ({Path.GetFileName(filePath)})", ct);
    }

    public Task AppendAuditAsync(string message, CancellationToken ct = default)
    {
        var line = $"[{DateTime.UtcNow:O}] {message}";
        var path = Path.Combine(_dataDir, "audit.log");
        return File.AppendAllTextAsync(path, line + Environment.NewLine, ct);
    }
}
