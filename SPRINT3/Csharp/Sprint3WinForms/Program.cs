
using System;
using System.Windows.Forms;

namespace Sprint3WinForms
{
    /// <summary>Main entry point.</summary>
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}
