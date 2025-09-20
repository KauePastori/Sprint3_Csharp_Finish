using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sprint3WinForms;

public class FileService
{
    private readonly AppDbContext _db;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web) { WriteIndented = true };
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
        var items = await JsonSerializer.DeserializeAsync<List<Apostador>>(fs, _json, ct) ?? new();

        foreach (var a in items)
        {
            a.Id = 0;
            var n = CalcularNivel(a);
            a.NivelRisco = n;
            a.Recomendacao = GerarRecomendacao(n, a);
            if (a.DataUltimaAvaliacao == default) a.DataUltimaAvaliacao = DateTime.Today;
        }

        await _db.Apostadores.AddRangeAsync(items, ct);
        var count = await _db.SaveChangesAsync(ct);
        await AppendAuditAsync($"Importou {items.Count} apostadores de {Path.GetFileName(filePath)}", ct);
        return count;
    }

    public async Task ExportJsonAsync(string filePath, CancellationToken ct = default)
    {
        var list = await _db.Apostadores.AsNoTracking().ToListAsync(ct);
        var json = JsonSerializer.Serialize(list, _json);
        await File.WriteAllTextAsync(filePath, json, ct);
        await AppendAuditAsync($"Exportou {list.Count} apostadores para JSON ({Path.GetFileName(filePath)})", ct);
    }

    public async Task ExportTxtAsync(string filePath, CancellationToken ct = default)
    {
        var list = await _db.Apostadores.AsNoTracking().ToListAsync(ct);
        var sb = new StringBuilder();
        foreach (var a in list)
            sb.AppendLine($"{a.Id}\t{a.Nome}\t{a.SinaisAlerta}\t{a.PerdasUltimoMes}\t{a.DataUltimaAvaliacao:yyyy-MM-dd}");
        await File.WriteAllTextAsync(filePath, sb.ToString(), ct);
        await AppendAuditAsync($"Exportou {list.Count} apostadores para TXT ({Path.GetFileName(filePath)})", ct);
    }

    public async Task ExportCsvAsync(string filePath, CancellationToken ct = default)
    {
        var list = await _db.Apostadores.AsNoTracking().ToListAsync(ct);
        var sb = new StringBuilder();
        sb.AppendLine("Id,Nome,Idade,FrequenciaSemanal,TempoMedioSessaoMin,PerdasUltimoMes,SinaisAlerta,NivelRisco,Recomendacao,DataUltimaAvaliacao");

        static string Esc(string? s) => "\"" + (s ?? string.Empty).Replace("\"", "\"\"") + "\"";

        foreach (var a in list)
        {
            sb.AppendLine(string.Join(",",
                a.Id,
                Esc(a.Nome),
                a.Idade,
                a.FrequenciaSemanal,
                a.TempoMedioSessaoMin,
                a.PerdasUltimoMes,
                Esc(a.SinaisAlerta),
                Esc(a.NivelRisco),
                Esc(a.Recomendacao),
                a.DataUltimaAvaliacao.ToString("yyyy-MM-dd")
            ));
        }

        await File.WriteAllTextAsync(filePath, sb.ToString(), ct);
        await AppendAuditAsync($"Exportou {list.Count} apostadores para CSV ({Path.GetFileName(filePath)})", ct);
    }

    public Task AppendAuditAsync(string message, CancellationToken ct = default)
    {
        var line = $"[{DateTime.UtcNow:O}] {message}";
        var path = Path.Combine(_dataDir, "audit.log");
        return File.AppendAllTextAsync(path, line + Environment.NewLine, ct);
    }

    private static string CalcularNivel(Apostador a)
    {
        int pontos = 0;
        if (a.FrequenciaSemanal >= 4) pontos += 2;
        if (a.TempoMedioSessaoMin >= 120) pontos += 2;
        if (a.PerdasUltimoMes >= 1000) pontos += 2;
        if (!string.IsNullOrWhiteSpace(a.SinaisAlerta)) pontos += 2;
        return pontos >= 6 ? "Alto" : (pontos >= 3 ? "Médio" : "Baixo");
    }

    private static string GerarRecomendacao(string nivel, Apostador a) =>
        nivel switch
        {
            "Alto"  => "Encaminhar para apoio especializado; definir limites rígidos; contato semanal.",
            "Médio" => "Definir limites e monitorar; alerta financeiro; revisão quinzenal.",
            _       => "Educação financeira básica; revisão mensal."
        };
}
