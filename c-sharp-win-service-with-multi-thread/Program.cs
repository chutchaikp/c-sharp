using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_win_service_with_multi_thread
{
    static class Program
    {
        public static int _MAXNBTHREAD = 10;
        public static bool DoExit = false;
        public static Form1 mainForm;
        
        [STAThread]
        static void Main()
        {
            if (ConfigurationManager.AppSettings["MAXNBTHREAD"] != null)
                _MAXNBTHREAD = Convert.ToInt32(ConfigurationManager.AppSettings["MAXNBTHREAD"]);


#if !DEBUG
            // Win Service
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] { new PersonService() };
            ServiceBase.Run(ServicesToRun);
#else 
            // Win App
            mainForm = new Form1();
            Application.ApplicationExit += new EventHandler(ApplicationExit);
            Application.ThreadExit += new EventHandler(ApplicationExit);
            Application.Run(MainForm);
#endif
        }

        static void ApplicationExit(object sender, EventArgs e)
        {
            if (Program.DoExit == false)
            {
                Program.DoExit = true;
                Thread.Sleep(2000);
                try
                {
                    mainForm.ReaderThread.Abort();
                }
                catch { }

                Thread.Sleep(1000);
            }
        }

        static public Form1 MainForm {
            get { return mainForm; }
            set { mainForm = value; }
        }

    }
}
