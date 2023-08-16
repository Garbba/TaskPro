using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                //datefile = DateTime.ParseExact(row["datefile"].ToString(), "dd/MM/yyyy", null),
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
    }
}
