using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_redis
{
    public class RecordEvent
    {
        public int Id { get; set; }
        public string Serial { get; set; }
        public bool IsMoving { get; set; }
        public double Speed { get; set; }
        public int EventCode { get; set; }
        public DateTime EventTime { get; set; }
    }
}
