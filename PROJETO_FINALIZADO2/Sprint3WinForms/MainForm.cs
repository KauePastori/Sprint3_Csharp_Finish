using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace Sprint3WinForms;

public partial class MainForm : Form
{
    private readonly AppDbContext _db = new();
    private readonly FileService _files;
    private readonly BindingSource _binding = new();

    private System.Collections.Generic.List<Apostador> _allItems = new();
    private FancyUi? _fancy;

    public MainForm()
    {
        InitializeComponent();
        _files = new FileService(_db);
        dgvApostadores.AutoGenerateColumns = false;
        dgvApostadores.DataSource = _binding;
        dgvApostadores.CellFormatting += dgvApostadores_CellFormatting;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SetupColumns();
        BeautifyGrid();
        _fancy = FancyUi.Attach(this, dgvApostadores, (q, n) => ApplyFilter(q, n));
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await _db.Database.EnsureCreatedAsync();

        if (await _db.Apostadores.AnyAsync(x => string.IsNullOrWhiteSpace(x.NivelRisco) || string.IsNullOrWhiteSpace(x.Recomendacao)))
        {
            var todos = await _db.Apostadores.ToListAsync();
            foreach (var a in todos)
            {
                var n = CalcularNivel(a);
                a.NivelRisco = n;
                a.Recomendacao = GerarRecomendacao(n, a);
                if (a.DataUltimaAvaliacao == default) a.DataUltimaAvaliacao = DateTime.Today;
            }
            await _db.SaveChangesAsync();
        }

        await LoadGridAsync();
    }

    private async Task LoadGridAsync()
    {
        _allItems = await _db.Apostadores.OrderBy(b => b.Id).ToListAsync();
        _binding.DataSource = _allItems;
        _fancy?.UpdateStats(_allItems);
    }

    private Apostador? Current => _binding.Current as Apostador;

    private void dgvApostadores_SelectionChanged(object sender, EventArgs e)
    {
        if (Current is null) return;
        txtId.Text = Current.Id.ToString();
        txtNome.Text = Current.Nome;
        txtSinaisAlerta.Text = Current.SinaisAlerta;
        numPerdasUltimoMes.Value = Current.PerdasUltimoMes;
        dtPub.Value = Current.DataUltimaAvaliacao == default ? DateTime.Today : Current.DataUltimaAvaliacao;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
        txtId.Text = "0";
        txtNome.Clear();
        txtSinaisAlerta.Clear();
        numPerdasUltimoMes.Value = 0;
        dtPub.Value = DateTime.Today;
        txtNome.Focus();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNome.Text) || string.IsNullOrWhiteSpace(txtSinaisAlerta.Text))
        {
            MessageBox.Show("Nome e SinaisAlerta são obrigatórios.");
            return;
        }

        int id = int.TryParse(txtId.Text, out var v) ? v : 0;
        if (id == 0)
        {
            var b = new Apostador
            {
                Nome = txtNome.Text.Trim(),
                SinaisAlerta = txtSinaisAlerta.Text.Trim(),
                PerdasUltimoMes = (int)numPerdasUltimoMes.Value,
                DataUltimaAvaliacao = dtPub.Value.Date
            };
            var nivel = CalcularNivel(b);
            b.NivelRisco = nivel;
            b.Recomendacao = GerarRecomendacao(nivel, b);
            _db.Apostadores.Add(b);
        }
        else
        {
            var b = await _db.Apostadores.FindAsync(id);
            if (b is null) { MessageBox.Show("Registro não encontrado."); return; }
            b.Nome = txtNome.Text.Trim();
            b.SinaisAlerta = txtSinaisAlerta.Text.Trim();
            b.PerdasUltimoMes = (int)numPerdasUltimoMes.Value;
            b.DataUltimaAvaliacao = dtPub.Value.Date;
            var nivel = CalcularNivel(b);
            b.NivelRisco = nivel;
            b.Recomendacao = GerarRecomendacao(nivel, b);
            _db.Apostadores.Update(b);
        }

        await _db.SaveChangesAsync();
        await LoadGridAsync();
        await _files.AppendAuditAsync($"Salvou apostador: {txtNome.Text}");
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        int id = int.TryParse(txtId.Text, out var v) ? v : 0;
        if (id == 0) return;
        if (MessageBox.Show("Excluir registro selecionado?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            var b = await _db.Apostadores.FindAsync(id);
            if (b is not null) _db.Apostadores.Remove(b);
            await _db.SaveChangesAsync();
            await LoadGridAsync();
            await _files.AppendAuditAsync($"Excluiu apostador Id {id}");
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e) => await LoadGridAsync();

    private async void mnuImportJson_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = "JSON (*.json)|*.json", Title = "Importar apostadores (JSON)" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var count = await _files.ImportJsonAsync(dlg.FileName);
                await LoadGridAsync();
                MessageBox.Show($"Importados: {count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao importar: " + ex.Message);
            }
        }
    }

    private async void mnuExportJson_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "JSON (*.json)|*.json", FileName = "apostadores.json" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            await _files.ExportJsonAsync(dlg.FileName);
            MessageBox.Show("JSON exportado!");
        }
    }

    private async void mnuExportTxt_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "TXT (*.txt)|*.txt", FileName = "apostadores.txt" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            await _files.ExportTxtAsync(dlg.FileName);
            MessageBox.Show("TXT exportado!");
        }
    }

    private string CalcularNivel(Apostador a)
    {
        int pontos = 0;
        if (a.FrequenciaSemanal >= 4) pontos += 2;
        if (a.TempoMedioSessaoMin >= 120) pontos += 2;
        if (a.PerdasUltimoMes >= 1000) pontos += 2;
        if (!string.IsNullOrWhiteSpace(a.SinaisAlerta)) pontos += 2;
        return pontos >= 6 ? "Alto" : (pontos >= 3 ? "Médio" : "Baixo");
    }

    private string GerarRecomendacao(string nivel, Apostador a) =>
        nivel switch
        {
            "Alto"  => "Encaminhar para apoio especializado; definir limites rígidos; contato semanal.",
            "Médio" => "Definir limites e monitorar; alerta financeiro; revisão quinzenal.",
            _       => "Educação financeira básica; revisão mensal."
        };

    private void btnAvaliar_Click(object? sender, EventArgs e)
    {
        if (_binding.Current is Apostador a)
        {
            a.NivelRisco = CalcularNivel(a);
            a.Recomendacao = GerarRecomendacao(a.NivelRisco, a);
            a.DataUltimaAvaliacao = DateTime.UtcNow;
            _db.Update(a);
            _db.SaveChanges();
            _binding.ResetBindings(false);
            var list = _binding.List.Cast<Apostador>().ToList();
            _fancy?.UpdateStats(list);
        }
    }

    private void SetupColumns()
    {
        dgvApostadores.AutoGenerateColumns = false;
        dgvApostadores.Columns.Clear();
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.Nome), HeaderText = "Nome", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.Idade), HeaderText = "Idade", Width = 70, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.FrequenciaSemanal), HeaderText = "Freq/Sem", Width = 80, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.TempoMedioSessaoMin), HeaderText = "Min/Sessão", Width = 90, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.PerdasUltimoMes), HeaderText = "Perdas Mês", Width = 90, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight, Format = "N0" } });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.SinaisAlerta), HeaderText = "Sinais de Alerta", Width = 200 });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.NivelRisco), HeaderText = "Nível", Width = 80 });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.Recomendacao), HeaderText = "Recomendação", Width = 250 });
        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.DataUltimaAvaliacao), HeaderText = "Última", Width = 100, DefaultCellStyle = { Format = "yyyy-MM-dd" } });
    }

    private void BeautifyGrid()
    {
        var g = dgvApostadores;
        g.BorderStyle = BorderStyle.None;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.RowHeadersVisible = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.MultiSelect = false;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.ColumnHeadersHeight = 36;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f);
        g.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
        g.RowTemplate.Height = 28;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
        g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 249, 249);
        g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 236, 255);
        g.DefaultCellStyle.SelectionForeColor = Color.Black;
        g.GridColor = Color.FromArgb(230, 230, 230);
    }

    private void dgvApostadores_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dgvApostadores.Rows[e.RowIndex];
        if (row?.DataBoundItem is not Apostador item) return;
        Color? bg = item.NivelRisco switch
        {
            "Alto"  => Color.FromArgb(255, 235, 238),
            "Médio" => Color.FromArgb(255, 249, 196),
            "Baixo" => Color.FromArgb(232, 245, 233),
            _       => null
        };
        if (bg.HasValue)
        {
            row.DefaultCellStyle.BackColor = bg.Value;
            row.DefaultCellStyle.SelectionBackColor = ControlPaint.Light(bg.Value);
        }
    }

    private void ApplyFilter(string query, string nivel)
    {
        System.Collections.Generic.IEnumerable<Apostador> q = _allItems;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var t = query.Trim().ToLowerInvariant();
            q = q.Where(a =>
                (a.Nome ?? "").ToLowerInvariant().Contains(t) ||
                (a.SinaisAlerta ?? "").ToLowerInvariant().Contains(t) ||
                (a.Recomendacao ?? "").ToLowerInvariant().Contains(t));
        }
        if (!string.IsNullOrWhiteSpace(nivel) && nivel != "Todos")
            q = q.Where(a => string.Equals(a.NivelRisco, nivel, StringComparison.OrdinalIgnoreCase));
        var list = q.ToList();
        _binding.DataSource = list;
        _binding.ResetBindings(false);
        _fancy?.UpdateStats(list);
    }
}
