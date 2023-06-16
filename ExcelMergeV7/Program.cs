using System;
using System.Threading;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        ///
        private static Mutex mutex = new Mutex(false, DateTime.Now.ToString("hhmmss"));

        [STAThread]
        private static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                MessageBox.Show("Application already started!", "", MessageBoxButtons.OK);
                return;
            }
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Start());
            }
            finally { mutex.ReleaseMutex(); }
            //Application.Run(new Reporte());
        }
    }
}