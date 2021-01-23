using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_win_service_with_multi_thread
{
    class FemaleAnalyser : BaseAnalyser
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void Log(User user)
        {
            log.Info(user.ToJSON());            
            // Program.mainForm.incrementNbFemale(1); WinApp
        }
    }
}
