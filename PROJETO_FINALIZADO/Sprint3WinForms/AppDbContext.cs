using Microsoft.EntityFrameworkCore;

namespace Sprint3WinForms;

/// <summary>EF Core DbContext usando SQLite local.</summary>
public class AppDbContext : DbContext
{
    public DbSet<Apostador> Apostadores => Set<Apostador>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlite("Data Source=app.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Apostador>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.SinaisAlerta).HasMaxLength(1000);
            e.Property(x => x.NivelRisco).HasMaxLength(20);
            e.Property(x => x.Recomendacao).HasMaxLength(500);

            // Tipos simples
            e.Property(x => x.Idade);
            e.Property(x => x.FrequenciaSemanal);
            e.Property(x => x.TempoMedioSessaoMin);
            e.Property(x => x.PerdasUltimoMes);
            e.Property(x => x.DataUltimaAvaliacao);
        });
    }
}
