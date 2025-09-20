using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Sprint3WinForms;

public class FancyUi
{
    private readonly Form _form;
    private readonly DataGridView _grid;
    private readonly Action<string, string> _onFilterChanged;

    private ToolStripTextBox _txtSearch = null!;
    private ToolStripDropDownButton _ddNivel = null!;
    private Label _lblTotal = null!;
    private Label _lblAlto = null!;
    private Label _lblMedio = null!;
    private Label _lblBaixo = null!;

    private FancyUi(Form form, DataGridView grid, Action<string, string> onFilterChanged)
    {
        _form = form;
        _grid = grid;
        _onFilterChanged = onFilterChanged;
    }

    public static FancyUi Attach(Form form, DataGridView grid, Action<string, string> onFilterChanged)
    {
        var f = new FancyUi(form, grid, onFilterChanged);
        f.BuildTopBar();
        f.HookChipRendering();
        return f;
    }

    private void BuildTopBar()
    {
        var tool = new ToolStrip
        {
            Dock = DockStyle.Top,
            GripStyle = ToolStripGripStyle.Hidden,
            BackColor = Color.White,
            Padding = new Padding(6, 6, 6, 6),
            Renderer = new ToolStripProfessionalRenderer(new SoftColors())
        };

        tool.Items.Add(new ToolStripLabel("Buscar:"));

        _txtSearch = new ToolStripTextBox
        {
            AutoSize = false,
            Width = 280,
            BorderStyle = BorderStyle.FixedSingle
        };
        _txtSearch.TextChanged += (_, __) => FireFilterChanged();
        tool.Items.Add(_txtSearch);

        tool.Items.Add(new ToolStripSeparator());

        _ddNivel = new ToolStripDropDownButton("Nível: Todos");
        foreach (var n in new[] { "Todos", "Alto", "Médio", "Baixo" })
        {
            var item = new ToolStripMenuItem(n) { Checked = n == "Todos" };
            item.Click += (_, __) => SetNivelFilter(n);
            _ddNivel.DropDownItems.Add(item);
        }
        tool.Items.Add(_ddNivel);

        _form.Controls.Add(tool);
        _form.Controls.SetChildIndex(tool, 1);

        var cards = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 80,
            Padding = new Padding(12, 8, 12, 8),
            BackColor = Color.White
        };

        _lblTotal = MakeStatCard("Total", "0");
        _lblAlto  = MakeStatCard("Alto",  "0", Color.FromArgb(244, 67, 54));
        _lblMedio = MakeStatCard("Médio", "0", Color.FromArgb(255, 193, 7));
        _lblBaixo = MakeStatCard("Baixo", "0", Color.FromArgb(76, 175, 80));

        cards.Controls.Add(_lblTotal.Parent!);
        cards.Controls.Add(_lblAlto.Parent!);
        cards.Controls.Add(_lblMedio.Parent!);
        cards.Controls.Add(_lblBaixo.Parent!);

        _form.Controls.Add(cards);
        _form.Controls.SetChildIndex(cards, 2);
    }

    private Label MakeStatCard(string title, string value, Color? accent = null)
    {
        var p = new Panel
        {
            Width = 200,
            Height = 56,
            Margin = new Padding(0, 0, 12, 0),
            Padding = new Padding(12),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblTitle = new Label
        {
            Text = title,
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 9f),
            ForeColor = Color.FromArgb(90, 90, 90)
        };

        var lblVal = new Label
        {
            Text = value,
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 16f),
            ForeColor = accent ?? Color.FromArgb(50, 50, 50),
            Location = new Point(0, 22)
        };

        p.Controls.Add(lblTitle);
        p.Controls.Add(lblVal);
        return lblVal;
    }

    public void UpdateStats(List<Apostador> list)
    {
        int total = list.Count;
        int alto  = list.Count(a => string.Equals(a.NivelRisco, "Alto", StringComparison.OrdinalIgnoreCase));
        int medio = list.Count(a => string.Equals(a.NivelRisco, "Médio", StringComparison.OrdinalIgnoreCase));
        int baixo = list.Count(a => string.Equals(a.NivelRisco, "Baixo", StringComparison.OrdinalIgnoreCase));

        _lblTotal.Text = total.ToString();
        _lblAlto.Text  = alto.ToString();
        _lblMedio.Text = medio.ToString();
        _lblBaixo.Text = baixo.ToString();
    }

    private void SetNivelFilter(string nivel)
    {
        foreach (ToolStripMenuItem it in _ddNivel.DropDownItems)
            it.Checked = string.Equals(it.Text, nivel, StringComparison.OrdinalIgnoreCase);

        _ddNivel.Text = $"Nível: {nivel}";
        FireFilterChanged();
    }

    private void FireFilterChanged()
    {
        string nivel = _ddNivel.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(i => i.Checked)?.Text ?? "Todos";
        _onFilterChanged(_txtSearch.Text, nivel);
    }

    private void HookChipRendering()
    {
        _grid.CellPainting -= Grid_CellPainting;
        _grid.CellPainting += Grid_CellPainting;
    }

    private void Grid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        var col = _grid.Columns[e.ColumnIndex];
        if (!string.Equals(col.DataPropertyName, nameof(Apostador.NivelRisco), StringComparison.Ordinal))
            return;

        e.Handled = true;
        e.PaintBackground(e.ClipBounds, true);

        var text = e.FormattedValue?.ToString() ?? "";
        if (string.IsNullOrWhiteSpace(text))
        {
            e.PaintContent(e.ClipBounds);
            return;
        }

        var (bg, fg) = ChipColors(text);
        using var back = new SolidBrush(bg);
        using var fore = new SolidBrush(fg);

        var r = e.CellBounds;
        var pad = 6;
        var chipRect = new Rectangle(
            r.X + pad,
            r.Y + (r.Height / 2 - 14),
            Math.Max(44, TextRenderer.MeasureText(text, _grid.Font).Width + 18),
            28
        );
        var radius = 14;

        using (var path = RoundedRect(chipRect, radius))
        using (var _ = new SmoothingModeExt.SmoothingModeWrapper(e.Graphics!))
        {
            e.Graphics.FillPath(back, path);
            var textPoint = new Point(
                chipRect.X + 9,
                chipRect.Y + (chipRect.Height - TextRenderer.MeasureText(text, _grid.Font).Height) / 2
            );
            TextRenderer.DrawText(e.Graphics, text, _grid.Font, textPoint, fg);
        }

        e.Graphics.DrawLine(new Pen(Color.FromArgb(245, 245, 245)), e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
    }

    private static (Color bg, Color fg) ChipColors(string nivel) => nivel switch
    {
        "Alto"  => (Color.FromArgb(255, 224, 229), Color.FromArgb(183, 28, 28)),
        "Médio" => (Color.FromArgb(255, 243, 205), Color.FromArgb(158, 109, 0)),
        "Baixo" => (Color.FromArgb(223, 245, 230), Color.FromArgb(27, 94, 32)),
        _       => (Color.FromArgb(234, 234, 234), Color.FromArgb(60, 60, 60))
    };

    private static System.Drawing.Drawing2D.GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        var path = new System.Drawing.Drawing2D.GraphicsPath();
        int d = radius * 2;
        path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
        path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
        path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
        path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private class SoftColors : ProfessionalColorTable
    {
        public override Color ToolStripGradientBegin => Color.White;
        public override Color ToolStripGradientMiddle => Color.White;
        public override Color ToolStripGradientEnd => Color.White;
        public override Color MenuStripGradientBegin => Color.White;
        public override Color MenuStripGradientEnd => Color.White;
        public override Color ImageMarginGradientBegin => Color.White;
        public override Color ImageMarginGradientEnd => Color.White;
        public override Color MenuItemSelected => Color.FromArgb(230, 238, 255);
        public override Color MenuItemBorder => Color.FromArgb(210, 210, 210);
    }
}

static class SmoothingModeExt
{
    public class SmoothingModeWrapper : IDisposable
    {
        private readonly Graphics _g;
        private readonly System.Drawing.Drawing2D.SmoothingMode _old;
        public SmoothingModeWrapper(Graphics g)
        {
            _g = g;
            _old = g.SmoothingMode;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        }
        public void Dispose() => _g.SmoothingMode = _old;
    }
}
