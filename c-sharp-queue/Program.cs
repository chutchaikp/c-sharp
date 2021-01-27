using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_queue
{
    static class Program
    {
        public static MainForm mainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());

            mainForm = new MainForm();
            Application.Run(mainForm);

        }
        static public MainForm MainForm {
            get { return mainForm; }
            set { mainForm = value; }
        }
    }
}
