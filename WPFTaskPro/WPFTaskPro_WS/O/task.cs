using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTaskPro_WS.O
{
    public class task
    {
        public int id { get; set; }
        public string title { get; set; }
        public string taskdescription { get; set; }
        public string taskStatus { get; set; }
        public byte isfavorite { get; set; }
        public byte isonmyday { get; set; }
        public Nullable<System.DateTime> startdate { get; set; }
        public Nullable<System.DateTime> enddate { get; set; }
        public string taskPriority { get; set; }
        public int list_id { get; set; }
    }
}
