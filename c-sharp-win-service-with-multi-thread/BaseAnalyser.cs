using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_win_service_with_multi_thread
{
    public abstract class BaseAnalyser
    {
        public abstract void Log(User user);
    }
}
