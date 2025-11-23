using Dark.Net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CD_Player
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (Properties.Settings.Default.IS_APP_SETUP_FINISHED == false)
            {
                Application.Run(new finalizeappsetup());
            }
            else if (Properties.Settings.Default.IS_APP_SETUP_FINISHED == true)
            {
                Application.Run(new Form1());
            }

            //Application.Run(new Form1());
        }

    }
}
