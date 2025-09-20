using System;
using System.Linq;
using System.Windows.Forms;
using StudentsApp.Domain;
using StudentsApp.Services;

namespace StudentsApp.UI
{
    public class MainForm : Form
    {
        private readonly StudentService _service = new();
        private DataGridView grid = new();
        private TextBox txtId = new(){ ReadOnly = true };
        private TextBox txtName = new();
        private TextBox txtRM = new();
        private TextBox txtCPF = new();
        private TextBox txtEmail = new();
        private Button btnAdd = new(){ Text = "Adicionar" };
        private Button btnUpdate = new(){ Text = "Atualizar" };
        private Button btnDelete = new(){ Text = "Excluir" };
        private Button btnClear = new(){ Text = "Limpar" };
        private Button btnReload = new(){ Text = "Recarregar" };
        private Button btnExpJson = new(){ Text = "Exportar JSON" };
        private Button btnImpJson = new(){ Text = "Importar JSON" };
        private Button btnExpTxt = new(){ Text = "Exportar TXT" };

        public MainForm()
        {
            Text = "Cadastro de Alunos - WinForms + SQLite + JSON/TXT";
            Width = 1050;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;

            grid.Dock = DockStyle.Bottom;
            grid.Height = 320;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.ReadOnly = true;
            grid.AutoGenerateColumns = true;
            grid.CellClick += Grid_CellClick;

            var lblId = new Label(){ Text="Id", Left=20, Top=20, Width=100 };
            txtId.Left=20; txtId.Top=40; txtId.Width=150;

            var lblName = new Label(){ Text="Nome", Left=200, Top=20, Width=100 };
            txtName.Left=200; txtName.Top=40; txtName.Width=300;

            var lblRM = new Label(){ Text="RM", Left=520, Top=20, Width=80 };
            txtRM.Left=520; txtRM.Top=40; txtRM.Width=120;

            var lblCPF = new Label(){ Text="CPF", Left=660, Top=20, Width=100 };
            txtCPF.Left=660; txtCPF.Top=40; txtCPF.Width=150;

            var lblEmail = new Label(){ Text="Email", Left=830, Top=20, Width=150 };
            txtEmail.Left=830; txtEmail.Top=40; txtEmail.Width=180;

            btnAdd.Left=20; btnAdd.Top=100; btnAdd.Width=120; btnAdd.Click += (s,e)=>AddStudent();
            btnUpdate.Left=150; btnUpdate.Top=100; btnUpdate.Width=120; btnUpdate.Click += (s,e)=>UpdateStudent();
            btnDelete.Left=280; btnDelete.Top=100; btnDelete.Width=120; btnDelete.Click += (s,e)=>DeleteStudent();
            btnClear.Left=410; btnClear.Top=100; btnClear.Width=120; btnClear.Click += (s,e)=>ClearForm();
            btnReload.Left=540; btnReload.Top=100; btnReload.Width=120; btnReload.Click += (s,e)=>Reload();

            btnExpJson.Left=680; btnExpJson.Top=100; btnExpJson.Width=140; btnExpJson.Click += (s,e)=>ExportJson();
            btnImpJson.Left=830; btnImpJson.Top=100; btnImpJson.Width=140; btnImpJson.Click += (s,e)=>ImportJson();
            btnExpTxt.Left=20; btnExpTxt.Top=150; btnExpTxt.Width=120; btnExpTxt.Click += (s,e)=>ExportTxt();

            Controls.AddRange(new Control[]{ lblId, txtId, lblName, txtName, lblRM, txtRM, lblCPF, txtCPF, lblEmail, txtEmail,
                btnAdd, btnUpdate, btnDelete, btnClear, btnReload, btnExpJson, btnImpJson, btnExpTxt, grid });

            Reload();
        }

        private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && grid.Rows[e.RowIndex].DataBoundItem is Student s)
            {
                txtId.Text = s.Id.ToString();
                txtName.Text = s.Name;
                txtRM.Text = s.RM;
                txtCPF.Text = s.CPF;
                txtEmail.Text = s.Email;
            }
        }

        private Student FromForm() => new Student{
            Id = int.TryParse(txtId.Text, out var id) ? id : 0,
            Name = txtName.Text.Trim(),
            RM = txtRM.Text.Trim(),
            CPF = txtCPF.Text.Trim(),
            Email = txtEmail.Text.Trim()
        };

        private void AddStudent()
        {
            try
            {
                var s = FromForm();
                _service.Add(s);
                ClearForm();
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao adicionar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStudent()
        {
            try
            {
                var s = FromForm();
                _service.Update(s);
                ClearForm();
                Reload();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao atualizar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteStudent()
        {
            if (!int.TryParse(txtId.Text, out var id))
            {
                MessageBox.Show("Selecione um aluno no grid.", "Atenção");
                return;
            }
            if (MessageBox.Show("Confirma exclusão?", "Excluir", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _service.Remove(id);
                ClearForm();
                Reload();
            }
        }

        private void ClearForm()
        {
            txtId.Clear(); txtName.Clear(); txtRM.Clear(); txtCPF.Clear(); txtEmail.Clear();
            txtName.Focus();
        }

        private void Reload()
        {
            grid.DataSource = _service.GetAll();
        }

        private void ExportJson()
        {
            using var sfd = new SaveFileDialog(){ Filter="JSON (*.json)|*.json", FileName="students.json" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileService.ExportJson(sfd.FileName, _service.GetAll());
                MessageBox.Show("Exportado para JSON.");
            }
        }

        private void ImportJson()
        {
            using var ofd = new OpenFileDialog(){ Filter="JSON (*.json)|*.json" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var list = FileService.ImportJson(ofd.FileName);
                foreach (var s in list)
                    _service.Add(s);
                Reload();
                MessageBox.Show("Importado do JSON.");
            }
        }

        private void ExportTxt()
        {
            using var sfd = new SaveFileDialog(){ Filter="TXT (*.txt)|*.txt", FileName="students.txt" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileService.ExportTxt(sfd.FileName, _service.GetAll());
                MessageBox.Show("Exportado para TXT.");
            }
        }
    }
}
