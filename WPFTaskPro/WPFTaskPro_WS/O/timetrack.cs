using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTaskPro_WS.O
{
    public class timetrack
    {
        public int id { get; set; }
        public System.DateTime starttime { get; set; }
        public System.DateTime endtime { get; set; }
        public byte isfinished { get; set; }
        public int user_id { get; set; }
        public int task_id { get; set; }
    }
}
