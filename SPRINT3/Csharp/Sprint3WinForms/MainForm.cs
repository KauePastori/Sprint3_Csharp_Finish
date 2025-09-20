
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
        dgvBooks.AutoGenerateColumns = false;
        dgvBooks.DataSource = _binding;
    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        await _db.Database.MigrateAsync();
        await LoadGridAsync();
    }

    private async Task LoadGridAsync()
    {
        var items = await _db.Books.OrderBy(b => b.Id).ToListAsync();
        _binding.DataSource = items;
    }

    private Book? Current => _binding.Current as Book;

    private void dgvBooks_SelectionChanged(object sender, EventArgs e)
    {
        if (Current is null) return;
        txtId.Text = Current.Id.ToString();
        txtTitle.Text = Current.Title;
        txtAuthor.Text = Current.Author;
        numPrice.Value = Current.Price;
        dtPub.Value = Current.PublishedOn == default ? DateTime.Today : Current.PublishedOn;
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
        txtId.Text = "0";
        txtTitle.Clear();
        txtAuthor.Clear();
        numPrice.Value = 0;
        dtPub.Value = DateTime.Today;
        txtTitle.Focus();
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text) || string.IsNullOrWhiteSpace(txtAuthor.Text))
        {
            MessageBox.Show("Title e Author são obrigatórios.");
            return;
        }

        int id = int.TryParse(txtId.Text, out var v) ? v : 0;
        if (id == 0)
        {
            var b = new Book
            {
                Title = txtTitle.Text.Trim(),
                Author = txtAuthor.Text.Trim(),
                Price = numPrice.Value,
                PublishedOn = dtPub.Value.Date
            };
            _db.Books.Add(b);
        }
        else
        {
            var b = await _db.Books.FindAsync(id);
            if (b is null) { MessageBox.Show("Registro não encontrado."); return; }
            b.Title = txtTitle.Text.Trim();
            b.Author = txtAuthor.Text.Trim();
            b.Price = numPrice.Value;
            b.PublishedOn = dtPub.Value.Date;
            _db.Books.Update(b);
        }
        await _db.SaveChangesAsync();
        await LoadGridAsync();
        await _files.AppendAuditAsync($"Salvou livro: {txtTitle.Text}");
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        int id = int.TryParse(txtId.Text, out var v) ? v : 0;
        if (id == 0) return;
        if (MessageBox.Show("Excluir registro selecionado?", "Confirmação", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            var b = await _db.Books.FindAsync(id);
            if (b is not null) _db.Books.Remove(b);
            await _db.SaveChangesAsync();
            await LoadGridAsync();
            await _files.AppendAuditAsync($"Excluiu livro Id {id}");
        }
    }

    private async void btnRefresh_Click(object sender, EventArgs e) => await LoadGridAsync();

    private async void mnuImportJson_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = "JSON (*.json)|*.json", Title = "Importar livros (JSON)" };
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
        using var dlg = new SaveFileDialog { Filter = "JSON (*.json)|*.json", FileName = "books.json" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            await _files.ExportJsonAsync(dlg.FileName);
            MessageBox.Show("JSON exportado!");
        }
    }

    private async void mnuExportTxt_Click(object sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog { Filter = "TXT (*.txt)|*.txt", FileName = "books.txt" };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            await _files.ExportTxtAsync(dlg.FileName);
            MessageBox.Show("TXT exportado!");
        }
    }
}
