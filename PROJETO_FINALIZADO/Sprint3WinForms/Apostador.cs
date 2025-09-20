
namespace Sprint3WinForms;

/// <summary>Entidade de domínio representando um Apostador e sua avaliação de risco.</summary>
public class Apostador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public int FrequenciaSemanal { get; set; } // nº de sessões por semana
    public int TempoMedioSessaoMin { get; set; } // em minutos
    public decimal PerdasUltimoMes { get; set; }
    public string SinaisAlerta { get; set; } = string.Empty; // ex.: insônia; dívidas; mentiras
    public string NivelRisco { get; set; } = "Baixo"; // Baixo, Médio, Alto
    public string Recomendacao { get; set; } = string.Empty;
    public DateTime DataUltimaAvaliacao { get; set; } = DateTime.UtcNow;
}
