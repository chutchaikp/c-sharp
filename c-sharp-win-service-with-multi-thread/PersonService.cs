using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_win_service_with_multi_thread
{
    partial class PersonService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Thread ReaderThread = null;

        public PersonService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            log.Debug("OnStart Services ^^^^^^^^^^^^^^^^^^^^^^^");
            Thread.Sleep(1000);

            ThreadStart start = new ThreadStart(analyseMessage);
            ReaderThread = new Thread(start);
            ReaderThread.Priority = ThreadPriority.Normal;
            ReaderThread.Start();
        }

        protected override void OnStop()
        {
            try
            {
                Program.DoExit = true;
                ReaderThread.Abort();
                ReaderThread = null;                
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            log.Debug("OnStop Services ^^^^^^^^^^^^^^^^^^^^^^^");
        }
        
        private void analyseMessage()
        {
            try
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
            catch (Exception error)
            {
                log.Error(error);
            }
        }

        List<User> FakeData()
        {
            FakeData fake = new FakeData();
            return fake.Data(10);
        }

    }
}
