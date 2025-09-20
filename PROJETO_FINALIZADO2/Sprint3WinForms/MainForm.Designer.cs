using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sprint3WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dgvApostadores = null!;
        private TextBox txtId = null!;
        private TextBox txtNome = null!;
        private TextBox txtSinaisAlerta = null!;
        private NumericUpDown numPerdasUltimoMes = null!;
        private DateTimePicker dtPub = null!;
        private Button btnNew = null!;
        private Button btnSave = null!;
        private Button btnDelete = null!;
        private Button btnRefresh = null!;
        private MenuStrip menuStrip1 = null!;
        private ToolStripMenuItem arquivoToolStripMenuItem = null!;
        private ToolStripMenuItem mnuImportJson = null!;
        private ToolStripMenuItem mnuExportJson = null!;
        private ToolStripMenuItem mnuExportTxt = null!;
        private Label lblId = null!;
        private Label lblNome = null!;
        private Label lblSinaisAlerta = null!;
        private Label lblPerdasUltimoMes = null!;
        private Label lblPub = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 640);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sprint3 - Apostadores (SQLite + JSON/TXT)";

            menuStrip1 = new MenuStrip
            {
                ImageScalingSize = new Size(20, 20),
                Dock = DockStyle.Top,
                BackColor = Color.White
            };
            arquivoToolStripMenuItem = new ToolStripMenuItem { Text = "Arquivo" };
            mnuImportJson = new ToolStripMenuItem { Text = "Importar JSON" };
            mnuExportJson = new ToolStripMenuItem { Text = "Exportar JSON" };
            mnuExportTxt  = new ToolStripMenuItem { Text = "Exportar TXT" };
            mnuImportJson.Click += mnuImportJson_Click;
            mnuExportJson.Click += mnuExportJson_Click;
            mnuExportTxt.Click  += mnuExportTxt_Click;
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]{ mnuImportJson, mnuExportJson, mnuExportTxt });
            menuStrip1.Items.AddRange(new ToolStripItem[]{ arquivoToolStripMenuItem });
            Controls.Add(menuStrip1);

            dgvApostadores = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 340,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(230, 230, 230),
                EnableHeadersVisualStyles = false
            };
            dgvApostadores.ColumnHeadersHeight = 36;
            dgvApostadores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvApostadores.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(60, 60, 60);
            dgvApostadores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            dgvApostadores.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgvApostadores.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 236, 255);
            dgvApostadores.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvApostadores.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 249, 249);
            dgvApostadores.RowTemplate.Height = 28;
            dgvApostadores.SelectionChanged += dgvApostadores_SelectionChanged;
            Controls.Add(dgvApostadores);

            var formPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 120,
                ColumnCount = 6,
                RowCount = 2,
                Padding = new Padding(12),
                BackColor = Color.White
            };
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            formPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            formPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            formPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));

            lblId = new Label { Text = "Id:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0,0,6,0) };
            txtId = new TextBox { ReadOnly = true, Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(0,0,12,0) };

            lblNome = new Label { Text = "Nome", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0,0,6,0) };
            txtNome = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right, Margin = new Padding(0,0,12,0) };

            lblSinaisAlerta = new Label { Text = "Sinais de Alerta", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0,0,6,0) };
            txtSinaisAlerta = new TextBox { Anchor = AnchorStyles.Left | AnchorStyles.Right };

            lblPerdasUltimoMes = new Label { Text = "Perda (mês)", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0,0,6,0) };
            numPerdasUltimoMes = new NumericUpDown { Anchor = AnchorStyles.Left | AnchorStyles.Right, Maximum = 10000000, ThousandsSeparator = true, Margin = new Padding(0,0,12,0), Width = 140 };

            lblPub = new Label { Text = "Publicado em:", Anchor = AnchorStyles.Left, AutoSize = true, Margin = new Padding(0,0,6,0) };
            dtPub = new DateTimePicker { Anchor = AnchorStyles.Left, Format = DateTimePickerFormat.Custom, CustomFormat = "yyyy-MM-dd", Width = 150 };

            formPanel.Controls.Add(lblId, 0, 0);
            formPanel.Controls.Add(txtId, 1, 0);
            formPanel.Controls.Add(lblNome, 2, 0);
            formPanel.Controls.Add(txtNome, 3, 0);
            formPanel.Controls.Add(lblSinaisAlerta, 4, 0);
            formPanel.Controls.Add(txtSinaisAlerta, 5, 0);
            formPanel.Controls.Add(lblPerdasUltimoMes, 0, 1);
            formPanel.Controls.Add(numPerdasUltimoMes, 1, 1);
            formPanel.Controls.Add(lblPub, 2, 1);
            formPanel.Controls.Add(dtPub, 3, 1);
            Controls.Add(formPanel);

            var buttonBar = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(12, 8, 12, 12),
                Height = 60,
                BackColor = Color.White
            };
            btnNew = MakeActionButton("Novo");
            btnSave = MakeActionButton("Salvar");
            btnDelete = MakeActionButton("Excluir");
            btnRefresh = MakeActionButton("Atualizar");
            btnNew.Click += btnNew_Click;
            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
            btnRefresh.Click += btnRefresh_Click;
            buttonBar.Controls.AddRange(new Control[]{ btnNew, btnSave, btnDelete, btnRefresh });
            Controls.Add(buttonBar);

            Load += MainForm_Load;
            MainMenuStrip = menuStrip1;
        }

        private static Button MakeActionButton(string text) => new Button
        {
            Text = text,
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(14, 6, 14, 6),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(242, 244, 248),
            ForeColor = Color.FromArgb(40, 40, 40),
        };
    }
}
