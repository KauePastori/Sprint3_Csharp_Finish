
using System.Windows.Forms;

namespace Sprint3WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dgvApostadores;
        private TextBox txtId;
        private TextBox txtNome;
        private TextBox txtSinaisAlerta;
        private NumericUpDown numPerdasUltimoMes;
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
        private DataGridViewTextBoxColumn colNome;
        private DataGridViewTextBoxColumn colSinaisAlerta;
        private DataGridViewTextBoxColumn colPerdasUltimoMes;
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
            this.Text = "Sprint3 - Apostadors (SQLite + JSON/TXT)";
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

            dgvApostadores = new DataGridView();
            dgvApostadores.Top = 30;
            dgvApostadores.Left = 10;
            dgvApostadores.Width = 880;
            dgvApostadores.Height = 320;
            dgvApostadores.ReadOnly = true;
            dgvApostadores.AllowUserToAddRows = false;
            dgvApostadores.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvApostadores.MultiSelect = false;

            colId = new DataGridViewTextBoxColumn(){ HeaderText = "Id", DataPropertyName = "Id", Width = 60 };
            colNome = new DataGridViewTextBoxColumn(){ HeaderText = "Nome", DataPropertyName = "Nome", Width = 240 };
            colSinaisAlerta = new DataGridViewTextBoxColumn(){ HeaderText = "SinaisAlerta", DataPropertyName = "SinaisAlerta", Width = 220 };
            colPerdasUltimoMes = new DataGridViewTextBoxColumn(){ HeaderText = "PerdasUltimoMes", DataPropertyName = "PerdasUltimoMes", Width = 90 };
            colPub = new DataGridViewTextBoxColumn(){ HeaderText = "DataUltimaAvaliacao", DataPropertyName = "DataUltimaAvaliacao", Width = 180 };

            dgvApostadores.Columns.AddRange(new DataGridViewColumn[]{ colId, colNome, colSinaisAlerta, colPerdasUltimoMes, colPub });
            dgvApostadores.SelectionChanged += dgvApostadores_SelectionChanged;
            Controls.Add(dgvApostadores);

            var lblId = new Label(){ Text = "Id:", Left = 10, Top = 370, Width = 40};
            txtId = new TextBox(){ Left = 60, Top = 365, Width = 70, ReadOnly = true };

            var lblNome = new Label(){ Text = "Nome:", Left = 150, Top = 370, Width = 50};
            txtNome = new TextBox(){ Left = 200, Top = 365, Width = 260 };

            var lblSinaisAlerta = new Label(){ Text = "SinaisAlerta:", Left = 480, Top = 370, Width = 60};
            txtSinaisAlerta = new TextBox(){ Left = 540, Top = 365, Width = 220 };

            var lblPerdasUltimoMes = new Label(){ Text = "PerdasUltimoMes:", Left = 10, Top = 410, Width = 50};
            numPerdasUltimoMes = new NumericUpDown(){ Left = 60, Top = 405, Width = 120, DecimalPlaces = 2, Maximum = 1000000 };

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

            Controls.AddRange(new Control[]{ lblId, txtId, lblNome, txtNome, lblSinaisAlerta, txtSinaisAlerta, lblPerdasUltimoMes, numPerdasUltimoMes, lblPub, dtPub, btnNew, btnSave, btnDelete, btnRefresh });

            this.Load += MainForm_Load;
        }
    }
}
