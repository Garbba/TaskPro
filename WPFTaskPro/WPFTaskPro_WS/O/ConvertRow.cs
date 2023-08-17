using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace WPFTaskPro_WS.O
{
    public class ConvertRow
    {
        public user user(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            user usuario = new user
            {
                id = int.Parse(row["id"].ToString()),
                nickname = row["nickname"].ToString(),
                username = row["username"].ToString(),
                lastname = row["lastname"].ToString(),
                email = row["email"].ToString(),
                userpassword = row["userpassword"].ToString()
            };
            return usuario;
        }
        public list list(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            list list = new list
            {
                id = int.Parse(row["id"].ToString()),
                listName = row["listName"].ToString(),
            };
            return list;
        }
        public listaccess listaccess(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            listaccess listacess = new listaccess
            {
                user_id = int.Parse(row["user_id"].ToString()),
                list_id = int.Parse(row["list_id"].ToString()),
            };
            return listacess;
        }
        public task task(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            task task = new task
            {
                id = int.Parse(row["id"].ToString()),
                title = row["title"].ToString(),
                taskStatus = row["taskStatus"].ToString(),
                isfavorite = byte.Parse(row["isfavorite"].ToString()),
                isonmyday = byte.Parse(row["isonmyday"].ToString()),
                startdate = row["startdate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["startdate"]),
                enddate = row["enddate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["enddate"]),
                taskPriority = row["taskPriority"].ToString(),
                list_id = int.Parse(row["list_id"].ToString()),

            };
            return task;
        }
        public tag tag(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            tag tag = new tag
            {
                id = int.Parse(row["id"].ToString()),
                tagName = row["tagName"].ToString(),
                list_id = int.Parse(row["list_id"].ToString()),

            };
            return tag;
        }
        public tasktag tasktag(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            tasktag tasktag = new tasktag
            {
                id = int.Parse(row["id"].ToString()),
                task_id = int.Parse(row["task_id"].ToString()),
                tag_id = int.Parse(row["tag_id"].ToString())

            };
            return tasktag;
        }
        public attachment attachment(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            attachment attachment = new attachment
            {
                id = int.Parse(row["id"].ToString()),
                datefile = Convert.ToDateTime(row["datefile"]),
                attachmentFilename = row["attachmentFilename"].ToString(),
                attachmentLink = row["attachmentLink"].ToString(),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["task_id"].ToString()),
            };
            return attachment;
        }
        public comment comment(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            comment commentUser = new comment
            {
                id = int.Parse(row["id"].ToString()),
                datecomment = DateTime.ParseExact(row["datecomment"].ToString(), "dd/MM/yyyy", null),
                commentUser1 = row["commentUser"].ToString(),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["list_id"].ToString()),
            };
            return commentUser;
        }
        public timetrack ConvertRowToTimeTrack(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            timetrack timetrack = new timetrack
            {
                id = int.Parse(row["id"].ToString()),
                starttime = Convert.ToDateTime(row["starttime"]),
                endtime = Convert.ToDateTime(row["endtime"]),
                isfinished = byte.Parse(row["isfinished"].ToString()),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["task_id"].ToString()),
            };
            return timetrack;
        }
        public member member(DataSet ds)
        {
            DataRow row = ds.Tables[0].Rows[0];
            member member = new member
            {
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["task_id"].ToString()),
            };
            return member;
        }

        public List<list> listprecharged()
        {
            List<list> listtolist = new List<list>();
            int i = 0;
            foreach(string l in new string[]{ "On My day","Favorite", "Assigned to me" })
            {
                list li = new O.list();
                li.id = i;
                li.listName = l;
                listtolist.Add(li);
                i++;
            }

            return listtolist;
        }


        public List<list> listtolist(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<list> listtolist = new List<list>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    list l = new list();
                    l.id = int.Parse(row["id"].ToString());
                    l.listName = row["listName"].ToString();

                    listtolist.Add(l);
                }
                return listtolist;
            }
            else
            {
                return null;
            }
        }
        public List<listaccess> listtolistaccess(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<listaccess> listtolistaccess = new List<listaccess>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    listaccess la = new listaccess();
                    la.user_id = int.Parse(row["user_id"].ToString());
                    la.list_id = int.Parse(row["list_id"].ToString());

                    listtolistaccess.Add(la);
                }
                return listtolistaccess;
            }
            else
            {
                return null;
            }
        }
        public List<task> listtotask(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<task> listtotask = new List<task>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    task t = new task();
                    t.id = int.Parse(row["id"].ToString());
                    t.title = row["title"].ToString();
                    t.taskStatus = row["taskStatus"].ToString();
                    t.isfavorite = byte.Parse(row["isfavorite"].ToString());
                    t.isonmyday = byte.Parse(row["isonmyday"].ToString());
                    t.startdate = row["startdate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["startdate"]);
                    t.enddate = row["enddate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["enddate"]);
                    t.taskPriority = row["taskPriority"].ToString();
                    t.list_id = int.Parse(row["list_id"].ToString());

                    listtotask.Add(t);
                }
                return listtotask;
            }
            else
            {
                return null;
            }
        }
        public List<tag> listtotag(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<tag> listtotag = new List<tag>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    tag t = new tag();
                    t.id = int.Parse(row["id"].ToString());
                    t.tagName = row["tagName"].ToString();
                    t.list_id = int.Parse(row["list_id"].ToString());

                    listtotag.Add(t);
                }
                return listtotag;
            }
            else
            {
                return null;
            }
        }
        public List<tasktag> listtotasktag(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<tasktag> listtotasktag = new List<tasktag>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    tasktag tt = new tasktag();
                    tt.id = int.Parse(row["id"].ToString());
                    tt.task_id = int.Parse(row["task_id"].ToString());
                    tt.tag_id = int.Parse(row["tag_id"].ToString());

                    listtotasktag.Add(tt);
                }
                return listtotasktag;
            }
            else
            {
                return null;
            }
        }
        public List<attachment> listtoattachment(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<attachment> listtoattachment = new List<attachment>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    attachment a = new attachment();
                    a.id = int.Parse(row["id"].ToString());
                    a.datefile = Convert.ToDateTime(row["datefile"]);
                    a.attachmentFilename = row["attachmentFilename"].ToString();
                    a.attachmentLink = row["attachmentLink"].ToString();
                    a.user_id = int.Parse(row["user_id"].ToString());
                    a.task_id = int.Parse(row["task_id"].ToString());

                    listtoattachment.Add(a);
                }
                return listtoattachment;
            }
            else
            {
                return null;
            }
        }
        public List<comment> listtocomment(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<comment> listtocomment = new List<comment>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    comment c = new comment();
                    c.id = int.Parse(row["id"].ToString());
                    c.datecomment = DateTime.ParseExact(row["datecomment"].ToString(), "dd/MM/yyyy", null);
                    c.commentUser1 = row["commentUser"].ToString();
                    c.user_id = int.Parse(row["user_id"].ToString());
                    c.task_id = int.Parse(row["list_id"].ToString());

                    listtocomment.Add(c);
                }
                return listtocomment;
            }
            else
            {
                return null;
            }
        }
        public List<timetrack> listtotimetrack(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<timetrack> listtotimetrack = new List<timetrack>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    timetrack tt = new timetrack();
                    tt.id = int.Parse(row["id"].ToString());
                    tt.starttime = Convert.ToDateTime(row["starttime"]);
                    tt.endtime = Convert.ToDateTime(row["endtime"]);
                    tt.isfinished = byte.Parse(row["isfinished"].ToString());
                    tt.user_id = int.Parse(row["user_id"].ToString());
                    tt.task_id = int.Parse(row["task_id"].ToString());

                    listtotimetrack.Add(tt);
                }
                return listtotimetrack;
            }
            else
            {
                return null;
            }
        }
        public List<member> listtomember(DataSet ds)
        {
            if (ds.Tables[0].Rows.Count != 0)
            {
                List<member> listtomember = new List<member>();

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ds.Tables[0].Rows[i];
                    member m = new member();
                    m.user_id = int.Parse(row["user_id"].ToString());
                    m.task_id = int.Parse(row["task_id"].ToString());

                    listtomember.Add(m);
                }
                return listtomember;
            }
            else
            {
                return null;
            }
        }
    }
}
