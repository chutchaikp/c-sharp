using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_socket_server_with_queue
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ServerForm serverForm;
        public static string PROTOCOL;
        public static string SERVER_ADDRESS;
        public static int SERVER_PORT;
        public static int BUFFER_SIZE;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            PROTOCOL = ConfigurationManager.AppSettings["PROTOCOL"];
            SERVER_ADDRESS = ConfigurationManager.AppSettings["SERVER_ADDRESS"];
            SERVER_PORT = Convert.ToInt32(ConfigurationManager.AppSettings["SERVER_PORT"]);
            BUFFER_SIZE = Convert.ToInt32(ConfigurationManager.AppSettings["BUFFER_SIZE"]);

            Application.ApplicationExit += new EventHandler(ApplicationExit);
            Application.ThreadExit += new EventHandler(ApplicationExit);

            serverForm = new ServerForm();
            Application.Run(serverForm);
        }

        static public ServerForm ServerForm {
            get { return serverForm; }
            set { serverForm = value; }
        }
        public static void ApplicationExit(object sender, EventArgs e)
        {
            try
            {
                if (serverForm.DoExit == false)
                {
                    serverForm.DoExit = true;
                    Thread.Sleep(2000);

                    serverForm.listeningThread.Abort();
                    serverForm.recordingThread.Abort();
                    Thread.Sleep(1000);
                }
            }
            catch { }

        }
    }
}
