using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTaskPro_WS.O
{
    public class attachment
    {
        public int id { get; set; }
        public System.DateTime datefile { get; set; }
        public string attachmentFilename { get; set; }
        public string attachmentLink { get; set; }
        public int user_id { get; set; }
        public int task_id { get; set; }
    }
}
