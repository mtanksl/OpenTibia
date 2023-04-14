using System;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.Run(new MainForm() );
        }
    }
}