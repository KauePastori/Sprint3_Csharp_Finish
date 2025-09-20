// MainForm.Designer.cs (versão corrigida)
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Linq;

namespace Sprint3WinForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        // Controles originais
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

        // Novos
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private TextBox txtSearch;
        private ShimmerPanel shimmer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ---------- FORM ----------
            Text = "Sprint3 • Livros (SQLite + JSON/TXT)";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1024, 720);
            Size = new Size(1200, 760);
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.FromArgb(248, 250, 252);
            DoubleBuffered = true;
            KeyPreview = true;

            Load += (_, __) =>
            {
                var dark = NativeTheme.IsSystemDark();
                NativeTheme.TryApplyWin11Effects(this, darkMode: dark, acrylic: false);
                statusLabel.Text = "Pronto";
            };

            KeyDown += (s, e) =>
            {
                if (e.Control && e.KeyCode == Keys.K)
                {
                    OpenCommandPalette();
                    e.SuppressKeyPress = true;
                }
            };

            // ---------- MENU ----------
            menuStrip1 = new MenuStrip { Font = new Font("Segoe UI", 10F), GripStyle = ToolStripGripStyle.Hidden };
            menuStrip1.Renderer = new ToolStripProfessionalRenderer(new FlatColorTable());

            arquivoToolStripMenuItem = new ToolStripMenuItem("&Arquivo");
            mnuImportJson = new ToolStripMenuItem("&Importar JSON", null, mnuImportJson_Click) { ShortcutKeys = Keys.Control | Keys.I };
            mnuExportJson = new ToolStripMenuItem("E&xportar JSON", null, mnuExportJson_Click) { ShortcutKeys = Keys.Control | Keys.E };
            mnuExportTxt = new ToolStripMenuItem("Exportar &TXT", null, mnuExportTxt_Click) { ShortcutKeys = Keys.Control | Keys.T };
            arquivoToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mnuImportJson, mnuExportJson, new ToolStripSeparator(), mnuExportTxt });
            menuStrip1.Items.Add(arquivoToolStripMenuItem);
            Controls.Add(menuStrip1);

            // Atalhos “fantasmas”
            var newCmd = new ToolStripMenuItem { ShortcutKeys = Keys.Control | Keys.N };
            newCmd.ShowShortcutKeys = false; newCmd.Click += btnNew_Click; menuStrip1.Items.Add(newCmd);
            var saveCmd = new ToolStripMenuItem { ShortcutKeys = Keys.Control | Keys.S };
            saveCmd.ShowShortcutKeys = false; saveCmd.Click += btnSave_Click; menuStrip1.Items.Add(saveCmd);
            var delCmd = new ToolStripMenuItem { ShortcutKeys = Keys.Delete };
            delCmd.ShowShortcutKeys = false; delCmd.Click += btnDelete_Click; menuStrip1.Items.Add(delCmd);
            var refCmd = new ToolStripMenuItem { ShortcutKeys = Keys.F5 };
            refCmd.ShowShortcutKeys = false; refCmd.Click += btnRefresh_Click; menuStrip1.Items.Add(refCmd);

            // ---------- STATUS ----------
            statusStrip = new StatusStrip { SizingGrip = false, BackColor = Color.White };
            statusLabel = new ToolStripStatusLabel("Carregando…");
            statusStrip.Items.Add(statusLabel);
            Controls.Add(statusStrip);

            // ---------- ROOT LAYOUT ----------
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = 1, RowCount = 3 };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 44)); // search bar
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 60));  // grid
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 40));  // form
            Controls.Add(root);

            // ---------- SEARCH BAR ----------
            var searchBar = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            searchBar.Paint += (s, e) =>
            {
                var p = searchBar.ClientRectangle; p.Width--; p.Height--;
                using var pen = new Pen(Color.FromArgb(229, 231, 235));
                e.Graphics.DrawRectangle(pen, p);
            };
            var lblSearch = new Label
            {
               
                AutoSize = true,
                Dock = DockStyle.Left,
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            txtSearch = new TextBox { Dock = DockStyle.Right, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 11f), Width = 360 };
            txtSearch.TextChanged += (_, __) => ApplyGridFilter();
            searchBar.Controls.Add(txtSearch);
            searchBar.Controls.Add(lblSearch);
            root.Controls.Add(searchBar, 0, 0);

            // ---------- GRID ----------
            dgvBooks = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersHeight = 44,
                EnableHeadersVisualStyles = false
            };

            var header = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(30, 64, 175),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                SelectionBackColor = Color.FromArgb(30, 64, 175),
                SelectionForeColor = Color.White
            };
            dgvBooks.ColumnHeadersDefaultCellStyle = header;

            dgvBooks.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.White,
                ForeColor = Color.FromArgb(17, 24, 39),
                SelectionBackColor = Color.FromArgb(219, 234, 254),
                SelectionForeColor = Color.FromArgb(17, 24, 39),
                Font = new Font("Segoe UI", 10F)
            };
            dgvBooks.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(247, 249, 252) };

            colId = new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id", FillWeight = 10, MinimumWidth = 60, SortMode = DataGridViewColumnSortMode.Automatic };
            colTitle = new DataGridViewTextBoxColumn { HeaderText = "Título", DataPropertyName = "Title", FillWeight = 28, MinimumWidth = 160, SortMode = DataGridViewColumnSortMode.Automatic };
            colAuthor = new DataGridViewTextBoxColumn { HeaderText = "Autor", DataPropertyName = "Author", FillWeight = 26, MinimumWidth = 150, SortMode = DataGridViewColumnSortMode.Automatic };
            colPrice = new DataGridViewTextBoxColumn { HeaderText = "Preço", DataPropertyName = "Price", FillWeight = 12, MinimumWidth = 90, SortMode = DataGridViewColumnSortMode.Automatic, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" } };
            colPub = new DataGridViewTextBoxColumn { HeaderText = "Publicado em", DataPropertyName = "PublishedOn", FillWeight = 24, MinimumWidth = 140, SortMode = DataGridViewColumnSortMode.Automatic, DefaultCellStyle = new DataGridViewCellStyle { Format = "d" } };
            dgvBooks.Columns.AddRange(colId, colTitle, colAuthor, colPrice, colPub);

            dgvBooks.SelectionChanged += dgvBooks_SelectionChanged;
            dgvBooks.CellFormatting += (s, e) =>
            {
                if (dgvBooks.Columns[e.ColumnIndex] == colPrice && e.Value is decimal val)
                {
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.SelectionForeColor = Color.White;
                    var bg = val >= 200m ? Color.FromArgb(239, 68, 68) :
                             val <= 50m ? Color.FromArgb(5, 150, 105) :
                                           Color.FromArgb(30, 64, 175);
                    e.CellStyle.BackColor = bg;
                    e.CellStyle.SelectionBackColor = bg;
                    e.Value = val.ToString("C2");
                }
            };

            var gridHost = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            gridHost.Paint += (s, e) =>
            {
                var r = gridHost.ClientRectangle; r.Width--; r.Height--;
                using var pen = new Pen(Color.FromArgb(229, 231, 235));
                e.Graphics.DrawRectangle(pen, r);
            };
            gridHost.Controls.Add(dgvBooks);

            // shimmer overlay
            shimmer = new ShimmerPanel { Dock = DockStyle.Fill, Visible = false };
            gridHost.Controls.Add(shimmer);

            root.Controls.Add(gridHost, 0, 1);

            // ---------- FORM DETALHES ----------
            var formCard = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(16) };
            formCard.Paint += (s, e) =>
            {
                var r = formCard.ClientRectangle; r.Width--; r.Height--;
                using var pen = new Pen(Color.FromArgb(229, 231, 235));
                e.Graphics.DrawRectangle(pen, r);
            };

            var formGrid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 6, RowCount = 3, AutoSize = true };
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18));
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80));
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            formGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32));
            formGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            formGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));
            formGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 56));

            var lblId = MakeLabel("ID:");
            var lblTitle = MakeLabel("Título:");
            var lblAuthor = MakeLabel("Autor:");
            var lblPrice = MakeLabel("Preço:");
            var lblPub = MakeLabel("Publicado em:");

            txtId = new TextBox { ReadOnly = true, Dock = DockStyle.Fill, BackColor = Color.FromArgb(243, 244, 246) };
            txtTitle = new TextBox { Dock = DockStyle.Fill };
            txtAuthor = new TextBox { Dock = DockStyle.Fill };
            numPrice = new NumericUpDown { Dock = DockStyle.Fill, DecimalPlaces = 2, Maximum = 1000000, ThousandsSeparator = true, TextAlign = HorizontalAlignment.Right };
            dtPub = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };

            formGrid.Controls.Add(lblId, 0, 0); formGrid.Controls.Add(txtId, 1, 0);
            formGrid.Controls.Add(lblTitle, 2, 0); formGrid.Controls.Add(txtTitle, 3, 0);
            formGrid.Controls.Add(lblAuthor, 4, 0); formGrid.Controls.Add(txtAuthor, 5, 0);

            formGrid.Controls.Add(lblPrice, 0, 1); formGrid.Controls.Add(numPrice, 1, 1);
            formGrid.Controls.Add(lblPub, 2, 1); formGrid.Controls.Add(dtPub, 3, 1);

            var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, Padding = new Padding(0, 8, 0, 0) };

            btnNew = MakePrimaryButton("&Novo", btnNew_Click);
            btnSave = MakeSuccessButton("&Salvar", btnSave_Click);
            btnDelete = MakeDangerButton("E&xcluir", btnDelete_Click);
            btnRefresh = MakeGhostButton("&Atualizar", btnRefresh_Click);
            actions.Controls.AddRange(new Control[] { btnNew, btnSave, btnDelete, btnRefresh });

            formGrid.Controls.Add(actions, 0, 2);
            formGrid.SetColumnSpan(actions, 6);

            formCard.Controls.Add(formGrid);
            root.Controls.Add(formCard, 0, 2);

            // ---------- DICAS ----------
            var tips = new ToolTip(components);
            tips.SetToolTip(txtTitle, "Título do livro");
            tips.SetToolTip(txtAuthor, "Autor/autora");
            tips.SetToolTip(numPrice, "Preço (R$)");
            tips.SetToolTip(dtPub, "Data de publicação");

            // ---------- HOOKS ----------
            this.Load += MainForm_Load;
        }

        // ---------- UX EXTRAS ----------
        private void ApplyGridFilter()
        {
            try
            {
                if (dgvBooks.DataSource is BindingSource bs && bs.List != null)
                {
                    if (bs.DataSource is System.Data.DataTable dt)
                    {
                        var q = EscapeLike(txtSearch.Text);
                        dt.DefaultView.RowFilter =
                            $"Convert(Id, 'System.String') LIKE '%{q}%' OR " +
                            $"Title LIKE '%{q}%' OR Author LIKE '%{q}%'";
                        return;
                    }
                }
                string query = (txtSearch.Text ?? "").Trim().ToLowerInvariant();
                dgvBooks.SuspendLayout();
                foreach (DataGridViewRow row in dgvBooks.Rows)
                {
                    bool visible =
                        (row.Cells[colId.Index].Value?.ToString() ?? "").ToLowerInvariant().Contains(query) ||
                        (row.Cells[colTitle.Index].Value?.ToString() ?? "").ToLowerInvariant().Contains(query) ||
                        (row.Cells[colAuthor.Index].Value?.ToString() ?? "").ToLowerInvariant().Contains(query);
                    row.Visible = visible;
                }
                dgvBooks.ResumeLayout();
            }
            catch { }
        }

        private static string EscapeLike(string s) => s?.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]") ?? "";

        private void OpenCommandPalette()
        {
            var cmds = new[]
            {
                new CommandPaletteForm.Cmd { Title = "Novo (Ctrl+N)",     Action = () => btnNew.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Salvar (Ctrl+S)",   Action = () => btnSave.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Excluir (Del)",     Action = () => btnDelete.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Atualizar (F5)",    Action = () => btnRefresh.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Importar JSON",     Action = () => mnuImportJson.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Exportar JSON",     Action = () => mnuExportJson.PerformClick() },
                new CommandPaletteForm.Cmd { Title = "Exportar TXT",      Action = () => mnuExportTxt.PerformClick() }
            };
            using var dlg = new CommandPaletteForm(cmds);
            dlg.ShowDialog(this);
        }

        private static Label MakeLabel(string text) =>
            new Label { Text = text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };

        private static Button MakeBaseButton(string text, EventHandler onClick)
        {
            var b = new Button
            {
                Text = text,
                AutoSize = true,
                Padding = new Padding(14, 8, 14, 8),
                FlatStyle = FlatStyle.Flat
            };
            b.FlatAppearance.BorderSize = 0;
            b.Click += onClick;
            return b;
        }

        private static Button MakePrimaryButton(string text, EventHandler onClick)
        {
            var b = MakeBaseButton(text, onClick);
            b.BackColor = Color.FromArgb(30, 64, 175);
            b.ForeColor = Color.White;
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(29, 78, 216);
            b.MouseLeave += (_, __) => b.BackColor = Color.FromArgb(30, 64, 175);
            return b;
        }
        private static Button MakeSuccessButton(string text, EventHandler onClick)
        {
            var b = MakeBaseButton(text, onClick);
            b.BackColor = Color.FromArgb(5, 150, 105);
            b.ForeColor = Color.White;
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(4, 120, 87);
            b.MouseLeave += (_, __) => b.BackColor = Color.FromArgb(5, 150, 105);
            return b;
        }
        private static Button MakeDangerButton(string text, EventHandler onClick)
        {
            var b = MakeBaseButton(text, onClick);
            b.BackColor = Color.FromArgb(239, 68, 68);
            b.ForeColor = Color.White;
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(220, 38, 38);
            b.MouseLeave += (_, __) => b.BackColor = Color.FromArgb(239, 68, 68);
            return b;
        }
        private static Button MakeGhostButton(string text, EventHandler onClick)
        {
            var b = MakeBaseButton(text, onClick);
            b.BackColor = Color.White;
            b.ForeColor = Color.FromArgb(30, 64, 175);
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Color.FromArgb(199, 210, 254);
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(241, 245, 249);
            b.MouseLeave += (_, __) => b.BackColor = Color.White;
            return b;
        }

        // ========= CLASSES DE SUPORTE (no mesmo arquivo) =========

        private class FlatColorTable : ProfessionalColorTable
        {
            public override Color ToolStripGradientBegin => Color.White;
            public override Color ToolStripGradientMiddle => Color.White;
            public override Color ToolStripGradientEnd => Color.White;
            public override Color MenuBorder => Color.FromArgb(229, 231, 235);
            public override Color MenuItemBorder => Color.FromArgb(199, 210, 254);
            public override Color MenuItemSelected => Color.FromArgb(238, 242, 255);
            public override Color ImageMarginGradientBegin => Color.White;
            public override Color ImageMarginGradientMiddle => Color.White;
            public override Color ImageMarginGradientEnd => Color.White;
        }

        // Tema Win11 (Mica/cantos/dark)
        private static class NativeTheme
        {
            private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
            private const int DWMWA_SYSTEMBACKDROP_TYPE = 38;

            private enum DwmWindowCornerPreference { Default = 0, DoNotRound = 1, Round = 2, RoundSmall = 3 }
            private enum DwmSystemBackdropType { Auto = 0, None = 1, Mica = 2, Acrylic = 3, Tabbed = 4 }

            [DllImport("dwmapi.dll", PreserveSig = true)]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

            public static bool TryApplyWin11Effects(Form form, bool darkMode = false, bool acrylic = false)
            {
                try
                {
                    if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000)) return false;
                    int useDark = darkMode ? 1 : 0;
                    _ = DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int));
                    int pref = (int)DwmWindowCornerPreference.Round;
                    _ = DwmSetWindowAttribute(form.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(int));
                    int backdrop = (int)(acrylic ? DwmSystemBackdropType.Acrylic : DwmSystemBackdropType.Mica);
                    _ = DwmSetWindowAttribute(form.Handle, DWMWA_SYSTEMBACKDROP_TYPE, ref backdrop, sizeof(int));
                    return true;
                }
                catch { return false; }
            }

            public static bool IsSystemDark()
            {
                return SystemColors.WindowText.GetBrightness() > SystemColors.Window.GetBrightness();
            }
        }

        // Toast animado (Timer totalmente qualificado)
        private class Toast : Control
        {
            private System.Windows.Forms.Timer life = new() { Interval = 16 };
            private float opacity = 0f;
            private int lifeMs = 2200, elapsed;
            private readonly string subtitle;

            public static void Show(Form parent, string title, string subtitle = "")
            {
                var t = new Toast(title, subtitle);
                parent.Controls.Add(t);
                t.BringToFront();
                t.Animate();
            }

            private Toast(string title, string subtitle = "")
            {
                Text = title; this.subtitle = subtitle;
                SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
                Width = 360; Height = 68;
                BackColor = Color.Transparent;
                life.Tick += (_, __) =>
                {
                    elapsed += life.Interval;
                    float fadeIn = Math.Min(1f, elapsed / 250f);
                    float hold = elapsed < (lifeMs - 300) ? 1f : Math.Max(0f, 1f - (elapsed - (lifeMs - 300)) / 300f);
                    opacity = fadeIn * hold;
                    if (elapsed >= lifeMs) { life.Stop(); Parent?.Controls.Remove(this); Dispose(); }
                    Invalidate();
                };
            }

            private void Animate()
            {
                var p = Parent as Form;
                Location = new Point((p.ClientSize.Width - Width) - 24, 24);
                life.Start();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics; g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int alpha = (int)(opacity * 220);
                using var bg = new SolidBrush(Color.FromArgb(alpha, 17, 24, 39));
                var r = ClientRectangle; r.Inflate(-1, -1);
                using var gp = new System.Drawing.Drawing2D.GraphicsPath();
                AddRoundedRect(gp, r, 14);
                g.FillPath(bg, gp);

                using var white = new SolidBrush(Color.FromArgb((int)(opacity * 255), 255, 255, 255));
                using var bold = new Font("Segoe UI Semibold", 10.5f);
                g.DrawString(Text, bold, white, new PointF(16, 12));
                if (!string.IsNullOrWhiteSpace(subtitle))
                {
                    using var normal = new Font("Segoe UI", 9f);
                    using var subBrush = new SolidBrush(Color.FromArgb((int)(opacity * 210), 236, 239, 244));
                    g.DrawString(subtitle, normal, subBrush, new PointF(16, 36));
                }
            }

            private static void AddRoundedRect(System.Drawing.Drawing2D.GraphicsPath gp, Rectangle r, int radius)
            {
                int d = radius * 2;
                gp.StartFigure();
                gp.AddArc(r.X, r.Y, d, d, 180, 90);
                gp.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                gp.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                gp.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                gp.CloseFigure();
            }
        }

        // Shimmer skeleton (Timer totalmente qualificado)
        private class ShimmerPanel : Panel
        {
            private System.Windows.Forms.Timer t = new() { Interval = 16 };
            private float x;
            public ShimmerPanel()
            {
                DoubleBuffered = true;
                t.Tick += (_, __) => { x += 2.2f; if (x > Width * 2) x = -Width; Invalidate(); };
            }
            public void Start() { Visible = true; t.Start(); }
            public void Stop() { t.Stop(); Visible = false; }
            protected override void OnPaint(PaintEventArgs e)
            {
                var g = e.Graphics; g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                var baseC = Color.FromArgb(235, 238, 241);
                var shine1 = Color.FromArgb(245, 247, 250);
                var shine2 = Color.FromArgb(225, 229, 235);
                using var lg = new System.Drawing.Drawing2D.LinearGradientBrush(new RectangleF(x - Width, 0, Width * 1.6f, Height), shine1, shine2, 0f);
                g.Clear(baseC);
                g.FillRectangle(lg, ClientRectangle);
                using var pen = new Pen(Color.FromArgb(220, 226, 232));
                for (int y = 12; y < Height; y += 34) g.DrawLine(pen, 0, y, Width, y);
            }
        }

        // Command Palette Ctrl+K (fix do CS0173: sempre retorna Cmd[])
        private class CommandPaletteForm : Form
        {
            private TextBox query = new() { BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 11f), Dock = DockStyle.Top, Height = 36 };
            private ListBox list = new() { Dock = DockStyle.Fill, BorderStyle = BorderStyle.None, IntegralHeight = false, Font = new Font("Segoe UI", 10f) };

            public class Cmd
            {
                public string Title { get; set; } = "";
                public Action Action { get; set; } = () => { };
                public override string ToString() => Title;
            }

            private readonly System.Collections.Generic.List<Cmd> commands;

            public CommandPaletteForm(System.Collections.Generic.IEnumerable<Cmd> cmds)
            {
                commands = new System.Collections.Generic.List<Cmd>(cmds);
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                Size = new Size(560, 420);
                DoubleBuffered = true;
                BackColor = Color.White;

                var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16) };
                var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
                card.Paint += (s, e) =>
                {
                    var r = card.ClientRectangle; r.Width--; r.Height--;
                    using var pen = new Pen(Color.FromArgb(225, 228, 234));
                    e.Graphics.DrawRectangle(pen, r);
                };

                query.PlaceholderText = "Digite um comando… (ex.: novo, salvar, excluir, atualizar, importar json)";
                query.TextChanged += (_, __) => ApplyFilter();
                query.KeyDown += (_, e) => { if (e.KeyCode == Keys.Escape) Close(); if (e.KeyCode == Keys.Enter) Execute(); };
                list.DoubleClick += (_, __) => Execute();

                Controls.Add(panel);
                panel.Controls.Add(card);
                card.Controls.Add(list);
                card.Controls.Add(new Panel { Dock = DockStyle.Top, Height = 8 });
                card.Controls.Add(query);

                Shown += (_, __) => { query.Focus(); ApplyFilter(); };
            }

            private void Execute()
            {
                if (list.SelectedItem is Cmd c) { Close(); c.Action(); }
            }

            private void ApplyFilter()
            {
                string q = query.Text.Trim();
                // >>> Fix: ambos os ramos retornam Cmd[]
                Cmd[] items = string.IsNullOrEmpty(q)
                    ? commands.ToArray()
                    : commands
                        .Select(c => (c, score: FuzzyScore(c.Title, q)))
                        .Where(x => x.score > 0)
                        .OrderByDescending(x => x.score)
                        .Select(x => x.c)
                        .ToArray();

                list.BeginUpdate();
                list.Items.Clear();
                list.Items.AddRange(items);
                if (list.Items.Count > 0) list.SelectedIndex = 0;
                list.EndUpdate();
            }

            private static int FuzzyScore(string text, string pattern)
            {
                if (string.IsNullOrEmpty(pattern)) return 1;
                text = text.ToLowerInvariant(); pattern = pattern.ToLowerInvariant();
                int ti = 0, pi = 0, score = 0, streak = 0;
                while (ti < text.Length && pi < pattern.Length)
                {
                    if (text[ti] == pattern[pi]) { score += 5 + streak * 2; streak++; pi++; }
                    else streak = 0;
                    ti++;
                }
                return (pi == pattern.Length) ? score : 0;
            }

            protected override CreateParams CreateParams
            {
                get { var cp = base.CreateParams; cp.ExStyle |= 0x00080000; return cp; }
            }
        }

        // ========= UTILIDADES =========
        private void ShowToast(string title, string subtitle = "")
        {
            Toast.Show(this, title, subtitle);
            statusLabel.Text = title;
        }
        private void ShowLoading(bool loading)
        {
            if (loading) shimmer.Start(); else shimmer.Stop();
        }
    }
}
