using System;
using System.Windows.Forms;

namespace StudentsApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Infrastructure.Database.EnsureCreated();
            Application.Run(new UI.MainForm());
        }
    }
}
