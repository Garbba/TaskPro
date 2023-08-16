using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTaskPro_WS.O
{
    public class comment
    {
        public int id { get; set; }
        public System.DateTime datecomment { get; set; }
        public string commentUser1 { get; set; }
        public int user_id { get; set; }
        public int task_id { get; set; }
    }
}
