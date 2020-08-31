using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SupremeXE_UI
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            InterceptKeys._hookID = InterceptKeys.SetHook(InterceptKeys._proc);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SupremeXE_UI_Main());
            InterceptKeys.UnhookWindowsHookEx(InterceptKeys._hookID);
        }
    }
}
