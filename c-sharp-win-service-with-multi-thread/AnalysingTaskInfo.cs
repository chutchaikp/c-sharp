using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_win_service_with_multi_thread
{
    public class AnalysingTaskInfo
    {
        public BaseAnalyser Analyser;
        public User user;

        public AnalysingTaskInfo(User u, BaseAnalyser analyser)
        {
            user = u;
            Analyser = analyser;
        }
    }

    public class ThreadAnalyse
    {
        public static void AnalyseOne(Object stateInfo)
        {
            AnalysingTaskInfo ati = (AnalysingTaskInfo)stateInfo;

            ati.Analyser.Log(ati.user);

        }

    }
}
