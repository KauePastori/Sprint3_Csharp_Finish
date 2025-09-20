
using System.Windows.Forms;

namespace Sprint3WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvBooks;
        private TextBox txtId;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private NumericUpDown numPrice;
        private DateTimePicker dtPub;
        private Button btnNew;
        private Button btnSave;
        private Button btnDelete;
        private Button btnRefresh;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem arquivoToolStripMenuItem;
        private ToolStripMenuItem mnuImportJson;
        private ToolStripMenuItem mnuExportJson;
        private ToolStripMenuItem mnuExportTxt;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colTitle;
        private DataGridViewTextBoxColumn colAuthor;
        private DataGridViewTextBoxColumn colPrice;
        private DataGridViewTextBoxColumn colPub;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            this.Text = "Sprint3 - Books (SQLite + JSON/TXT)";
            this.Width = 920;
            this.Height = 620;
            this.StartPosition = FormStartPosition.CenterScreen;

            menuStrip1 = new MenuStrip();
            arquivoToolStripMenuItem = new ToolStripMenuItem("Arquivo");
            mnuImportJson = new ToolStripMenuItem("Importar JSON");
            mnuExportJson = new ToolStripMenuItem("Exportar JSON");
            mnuExportTxt = new ToolStripMenuItem("Exportar TXT");
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]{ mnuImportJson, mnuExportJson, mnuExportTxt });
            menuStrip1.Items.Add(arquivoToolStripMenuItem);
            Controls.Add(menuStrip1);

            dgvBooks = new DataGridView();
            dgvBooks.Top = 30;
            dgvBooks.Left = 10;
            dgvBooks.Width = 880;
            dgvBooks.Height = 320;
            dgvBooks.ReadOnly = true;
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.MultiSelect = false;

            colId = new DataGridViewTextBoxColumn(){ HeaderText = "Id", DataPropertyName = "Id", Width = 60 };
            colTitle = new DataGridViewTextBoxColumn(){ HeaderText = "Title", DataPropertyName = "Title", Width = 240 };
            colAuthor = new DataGridViewTextBoxColumn(){ HeaderText = "Author", DataPropertyName = "Author", Width = 220 };
            colPrice = new DataGridViewTextBoxColumn(){ HeaderText = "Price", DataPropertyName = "Price", Width = 90 };
            colPub = new DataGridViewTextBoxColumn(){ HeaderText = "PublishedOn", DataPropertyName = "PublishedOn", Width = 180 };

            dgvBooks.Columns.AddRange(new DataGridViewColumn[]{ colId, colTitle, colAuthor, colPrice, colPub });
            dgvBooks.SelectionChanged += dgvBooks_SelectionChanged;
            Controls.Add(dgvBooks);

            var lblId = new Label(){ Text = "Id:", Left = 10, Top = 370, Width = 40};
            txtId = new TextBox(){ Left = 60, Top = 365, Width = 70, ReadOnly = true };

            var lblTitle = new Label(){ Text = "Title:", Left = 150, Top = 370, Width = 50};
            txtTitle = new TextBox(){ Left = 200, Top = 365, Width = 260 };

            var lblAuthor = new Label(){ Text = "Author:", Left = 480, Top = 370, Width = 60};
            txtAuthor = new TextBox(){ Left = 540, Top = 365, Width = 220 };

            var lblPrice = new Label(){ Text = "Price:", Left = 10, Top = 410, Width = 50};
            numPrice = new NumericUpDown(){ Left = 60, Top = 405, Width = 120, DecimalPlaces = 2, Maximum = 1000000 };

            var lblPub = new Label(){ Text = "Published On:", Left = 200, Top = 410, Width = 100};
            dtPub = new DateTimePicker(){ Left = 300, Top = 405, Width = 170, Format = DateTimePickerFormat.Short };

            btnNew = new Button(){ Text = "Novo", Left = 10, Top = 460, Width = 100 };
            btnSave = new Button(){ Text = "Salvar", Left = 120, Top = 460, Width = 100 };
            btnDelete = new Button(){ Text = "Excluir", Left = 230, Top = 460, Width = 100 };
            btnRefresh = new Button(){ Text = "Atualizar", Left = 340, Top = 460, Width = 100 };

            btnNew.Click += btnNew_Click;
            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;

            mnuImportJson.Click += mnuImportJson_Click;
            mnuExportJson.Click += mnuExportJson_Click;
            mnuExportTxt.Click += mnuExportTxt_Click;

            Controls.AddRange(new Control[]{ lblId, txtId, lblTitle, txtTitle, lblAuthor, txtAuthor, lblPrice, numPrice, lblPub, dtPub, btnNew, btnSave, btnDelete, btnRefresh });

            this.Load += MainForm_Load;
        }
    }
}
