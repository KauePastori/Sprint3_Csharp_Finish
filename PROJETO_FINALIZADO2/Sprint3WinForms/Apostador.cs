using System;

namespace Sprint3WinForms;

public class Apostador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Idade { get; set; }
    public int FrequenciaSemanal { get; set; }
    public int TempoMedioSessaoMin { get; set; }
    public int PerdasUltimoMes { get; set; }
    public string? SinaisAlerta { get; set; }
    public string? NivelRisco { get; set; }
    public string? Recomendacao { get; set; }
    public DateTime DataUltimaAvaliacao { get; set; }
}
