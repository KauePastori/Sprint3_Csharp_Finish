using Microsoft.EntityFrameworkCore;

namespace Sprint3WinForms;

public partial class MainForm : Form
{
    private readonly AppDbContext _db = new();
    private readonly FileService _files;
    private readonly BindingSource _binding = new();

    public MainForm()
    {
        InitializeComponent();
        _files = new FileService(_db);
        dgvApostadores.AutoGenerateColumns = false;
        dgvApostadores.DataSource = _binding;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        SetupColumns();
        // opcional (se incluir UiThemeHelper.cs):
        // UiThemeHelper.ApplyDark(this);
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await _db.Database.EnsureCreatedAsync();
        await LoadGridAsync();
    }

    private async Task LoadGridAsync()
    {
        var items = await _db.Apostadores.OrderBy(b => b.Id).ToListAsync();
        _binding.DataSource = items;
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
                // opcional se incluir ErrorDialog.cs
                // ErrorDialog.Show("Falha ao importar", "Não foi possível concluir a importação.", ex);
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

    // OPCIONAL: se adicionar um item de menu "Exportar CSV" e ligar a este handler:
    private async void mnuExportCsv_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv", FileName = "apostadores.csv" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            await _files.ExportCsvAsync(dlg.FileName);
            MessageBox.Show("CSV exportado!");
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

    private void btnAvaliar_Click(object? sender, EventArgs e)
    {
        if (_binding.Current is Apostador a)
        {
            a.NivelRisco = CalcularNivel(a);
            a.DataUltimaAvaliacao = DateTime.UtcNow;
            _db.Update(a);
            _db.SaveChanges();
            _binding.ResetBindings(false);
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

        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.NivelRisco), HeaderText = "Nível", Width = 70 });

        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.Recomendacao), HeaderText = "Recomendação", Width = 200 });

        dgvApostadores.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(Apostador.DataUltimaAvaliacao), HeaderText = "Última Avaliação", Width = 120, DefaultCellStyle = { Format = "yyyy-MM-dd" } });
    }
}
