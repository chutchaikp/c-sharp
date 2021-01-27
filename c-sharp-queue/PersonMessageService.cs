using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_queue
{
    class PersonMessageService : MessageHandler
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        bool Alive = false;
        Person _Person;
        MessagePool _MessagePool;

        public PersonMessageService(Person p, MessagePool pool)
        {
            _Person = p;
            _MessagePool = pool;
        }

        override public void Process()
        {
            // 
            ProcessDataReceived();
        }
        protected override void ProcessDataReceived()
        {
            Program.mainForm.updateNbInQueue(_MessagePool.Count);
            Program.mainForm.incrementNbLogged();

            Alive = true; // on progress
            // System.Threading.Thread.Sleep(1000);
            
            log.Debug( _Person.ToJSON());

            Alive = false; // done
        }
        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override bool IsAlive()
        {
            return Alive;
        }
    }
}
