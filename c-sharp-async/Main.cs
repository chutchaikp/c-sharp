using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_async
{
    public partial class Main : Form
    {
        // C# Async tutorial 
        // https://drive.google.com/drive/folders/1U5AnFwQE6Q5bkYag5Za-iHRmPw4EOj60

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        Thread monitorThread;
        Thread counterThread;

        public Main()
        {
            InitializeComponent();

            log.Debug("Hello Async");

            monitorThread = new Thread(new ThreadStart(Monitor));
            counterThread = new Thread(new ThreadStart(Counter));

            monitorThread.Start();
            counterThread.Start();

        }

        void Monitor()
        {
            while (true)
            {
                string fullPattern = DateTimeFormatInfo.CurrentInfo.FullDateTimePattern;
                fullPattern = Regex.Replace(fullPattern, "(:ss|:s)", "$1.fff");
                log.Debug( DateTime.Now.ToString(fullPattern) );

                Thread.Sleep(200);

            }
        }

        void Counter()
        {
            int counter = 0;
            while(true)
            {
                counter++;

                log.Info(counter);

                Thread.Sleep(300);
            }
        }

        private void Main_Leave(object sender, EventArgs e)
        {
            log.Debug("------------------- ON LEAVE ---------------");

            try
            {
                monitorThread.Abort();
                counterThread.Abort();
            }
            catch (Exception error)
            {
                log.Error(error.Message);
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.Debug("------------------- ON LEAVE ---------------");

            try
            {
                monitorThread.Abort();
                counterThread.Abort();
            }
            catch (Exception error)
            {
                log.Error(error.Message);
            }
        }
    }
}
