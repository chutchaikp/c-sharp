using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_win_service_with_multi_thread
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Thread ReaderThread = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            ThreadStart start = new ThreadStart(analyseMessages);
            ReaderThread = new Thread(start);
            ReaderThread.Priority = ThreadPriority.AboveNormal;
            ReaderThread.Start();
            ThreadPool.SetMaxThreads(Program._MAXNBTHREAD, Program._MAXNBTHREAD);
        }

        private void analyseMessages()
        {
            while ((Thread.CurrentThread.IsAlive) && (!Program.DoExit))
            {
                int availableWorkerThreads;
                int availablePortThreads;

                ThreadPool.GetAvailableThreads(out availableWorkerThreads, out availablePortThreads);

                if (availableWorkerThreads == 0)
                {
                    Thread.Sleep(500);
                    continue;
                }

                var users = FakeData();
                foreach (var user in users)
                {
                    AnalysingTaskInfo ati = null;
                    if (user.Gender == Gender.Male)
                    {
                        ati = new AnalysingTaskInfo(user, new MaleAnalyser());
                    }
                    else // if (user.Gender == Gender.Female) 
                    {
                        ati = new AnalysingTaskInfo(user, new FemaleAnalyser());
                    }

                    if (ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadAnalyse.AnalyseOne), ati) == false)
                    {
                        log.Error("Unable to queue ThreadPool request.");
                    }
                }                
            }
        }

        List<User> FakeData()
        {
            FakeData fake = new FakeData();            
            return fake.Data(10);
        }

        private delegate void incrementMaleDelegate(object item);
        public void incrementNbMale(object item)
        {
            if (this.label1.InvokeRequired)
            {
                this.lblMale.Invoke(new incrementMaleDelegate(this.incrementNbMale), item);
            }
            else
            {
                var res = Convert.ToInt32(this.lblMale.Text) + 1; // or + item
                this.lblMale.Text = res.ToString();
            }
        }

        private delegate void incrementFemaleDelegate(object item);
        public void incrementNbFemale(object item)
        {
            if (this.lblFemale.InvokeRequired)
            {
                this.lblFemale.Invoke(new incrementFemaleDelegate(this.incrementNbFemale), item);
            }
            else
            {
                var res = Convert.ToInt32(this.lblFemale.Text) + 1; // or + item
                this.lblFemale.Text = res.ToString();
            }
        }
    }
}

