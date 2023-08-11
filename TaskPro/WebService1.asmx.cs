using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services;

namespace TaskPro
{
    /// <summary>
    /// Descripción breve de WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        //DB connection
        public SqlConnection conexiondb()
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = "Data Source=.;Initial Catalog=TP;Integrated Security=True";
            return cn;
        }
        public DataSet selectTP(string table, string showColumns ,string where ) 
        {
            SqlDataAdapter da = null;
            if (where == null || where == "")
            {
                da = new SqlDataAdapter("SELECT " + showColumns + " from " + table, conexiondb());

            }
            else
            {
                da = new SqlDataAdapter("SELECT " + showColumns + " from " + table + " where " + where, conexiondb());
            }
                
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }
        //User
        public string userValidation(string nickname, string username, string lastname, string email, string userpassword)
        {
            DataSet dsNickname = selectTP("userlist", "*", $"nickname = '{nickname}'");
            DataSet dsEmail = selectTP("userlist", "*", $"email = '{email}'");

            if (((nickname ?? username ?? lastname ?? email ?? userpassword) == null) || ((nickname ?? username ?? lastname ?? email ?? userpassword) == ""))
            {
                return "Valide los campos, alguno se encuentra incompleto";
            }
            else if (dsNickname.Tables[0].Rows.Count == 1)
            {
                return "El usuario ya existe, debes cambiarlo.";
            }
            else if (dsEmail.Tables[0].Rows.Count == 1 || !emailValidation(email))
            {
                return "El correo ya existe o no tiene el formato correcto, debes cambiarlo.";
            }
            else
            {
                return null;

            }
        }
        public bool emailValidation(string email)
        {
            bool validEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase);
            return validEmail;
        }
        public userlist ConvertRowToUsuario(DataRow row)
        {
            userlist usuario = new userlist
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
        [WebMethod]
        public string userCreate(string nickname, string username, string lastname, string email, string userpassword)
        {
            string validation = userValidation(nickname, username, lastname, email, userpassword);
            if (validation != null)
            {
                return validation;
            } else
            {
                using (TPEntities tp = new TPEntities()){
                    var usuario = new userlist();

                    usuario.nickname = nickname;
                    usuario.username = username;
                    usuario.lastname = lastname;
                    usuario.email = email;
                    usuario.userpassword = userpassword;

                    tp.userlist.Add(usuario);
                    tp.SaveChanges();

                    return "Usuario agregado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet userReadById(int id)
        {
            DataSet ds = selectTP("userlist", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet userReadByNickname(string nickname)
        {
            DataSet ds = selectTP("userlist", "*", $"nickname = '{nickname}'");
            return ds;
        }
        [WebMethod]
        public DataSet userReadByEmail(string email)
        {
            DataSet ds = selectTP("userlist", "*", $"email = '{email}'");

            return ds;
        }
        [WebMethod]
        public string userUpdate(int id, string nickname, string username, string lastname, string email, string userpassword)
        {

            DataSet dsUser = userReadById(id);
            DataSet dsEmail = selectTP("userlist", "*", $"email = '{email}'");

            bool isnotnullorblank = false;

            foreach (string i in new string[] { nickname, username, lastname, email, userpassword })
            {
                if (i == null || i == "")
                {
                    isnotnullorblank = true;
                }
            }

            if (dsUser.Tables[0].Rows.Count == 0)
            {
                return "El usuario no existe";
            }
            userlist u = ConvertRowToUsuario(dsUser.Tables[0].Rows[0]);

            if (isnotnullorblank)
            {
                return "Valide los campos, alguno se encuentra incompleto";
            } else if (userReadByNickname(nickname).Tables[0].Rows.Count != 0 && nickname != u.nickname)
            {
                return "El usuario ya existe, debes cambiarlo.";
            } else if ((userReadByEmail(email).Tables[0].Rows.Count != 0 && email != u.email) || !emailValidation(email))
            {
                return "El correo ya existe o no tiene el formato correcto, debes cambiarlo.";
                
            } else
            {
                using (TPEntities tp = new TPEntities())
                {
                    u.nickname = nickname;
                    u.username = username;
                    u.lastname = lastname;
                    u.email = email;
                    u.userpassword = userpassword;

                    tp.Entry(u).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Usuario actualizado correctamente";
                }

            }  
        }
        [WebMethod]
        public string userDelete(int id)
        {
            using (TPEntities tp = new TPEntities())
            {
                userlist deleteUser = tp.userlist.Find(id);

                if (deleteUser == null)
                {
                    return "El usuario no existe";
                }
                else
                {
                    tp.userlist.Remove(deleteUser);
                    tp.SaveChanges();
                }
                return "Usuario eliminado correctamente";
            }
        }
        [WebMethod]
        public DataSet userLogin(string nicknameOrEmail, string password)
        {
            DataSet email = userReadByEmail(nicknameOrEmail);
            DataSet nickname = userReadByNickname(nicknameOrEmail);
            if (((nicknameOrEmail ?? password) == null) || ((nicknameOrEmail ?? password) == ""))
            {
                return null;
            }
            else if (email.Tables[0].Rows.Count == 0 && nickname.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            else if (email.Tables[0].Rows.Count != 0 && ConvertRowToUsuario(email.Tables[0].Rows[0]).userpassword == password)
            {
                return email;
            } 
            else if (nickname.Tables[0].Rows.Count != 0 && ConvertRowToUsuario(nickname.Tables[0].Rows[0]).userpassword == password)
            {
                return nickname;
            } 
            else
            {
                return null;
            }
        }
        //List
        public list ConvertRowToList(DataRow row)
        {
            list list = new list
            {
                id = int.Parse(row["id"].ToString()),
                listName = row["listName"].ToString(),
            };
            return list;
        }
        [WebMethod]
        public string listCreate(string listname)
        {
            if (listname == null || listname == "")
            {
                return "La lista debe tener un nombre, no debe estar en blanco.";
            }
            else
            {
                using (TPEntities tp = new TPEntities())
                {
                    var list = new list();

                    list.listName = listname;

                    tp.list.Add(list);
                    tp.SaveChanges();

                    return "Lista agregada correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet listReadById(int id)
        {
            DataSet ds = selectTP("list", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public string listUpdate(int id, string listname)
        {
            DataSet dsList = listReadById(id);

            if (dsList.Tables[0].Rows.Count == 0)
            {
                return "La lista no existe";
            }
            else
            {
                list u = ConvertRowToList(dsList.Tables[0].Rows[0]);
                using (TPEntities tp = new TPEntities())
                {
                    u.id = id;
                    u.listName = listname;
                    tp.Entry(u).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Lista actualizado correctamente";
                }
            }

        }
        [WebMethod]
        public string listDelete(int id)
        {

            using (TPEntities tp = new TPEntities())
            {
                list deleteList = tp.list.Find(id);

                if (deleteList == null)
                {
                    return "La lista no existe o campos en blanco";
                }
                else
                {
                    tp.list.Remove(deleteList);
                    tp.SaveChanges();
                    return "Lista eliminada correctamente";
                }
            }
        }
        //List Access
        public listacess ConvertRowToListAccess(DataRow row)
        {
            listacess listacess = new listacess
            {
                user_id = int.Parse(row["user_id"].ToString()),
                list_id = int.Parse(row["list_id"].ToString()),
            };
            return listacess;
        }
        [WebMethod]
        public string listAccessCreateUpdate(int idUsuario, int idLista, string accessType)
        {
            DataSet user = userReadById(idUsuario);
            DataSet list = listReadById(idLista);
            DataSet listacess = listAccessReadById(idUsuario, idLista);


            if (idUsuario == null || idLista == null)
            {
                return "Debe seleccioniar el usuario y la lista para agregar el acceso.";
            } 
            else if (!(accessType == "OWNER" || accessType == "ADMIN" || accessType == "MEMBER"))
            {
                return "El usuario solo puede ser OWNER, ADMIN O MEMBER";
            } 
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "Usa un usuario valido.";
            }
            else if (list.Tables[0].Rows.Count == 0)
            {
                return "Usa una lista valida.";
            }
            else if (listacess.Tables[0].Rows.Count == 0)
            {
                //create
                using (TPEntities tp = new TPEntities())
                {
                    var la = new listacess();

                    la.user_id = idUsuario;
                    la.list_id = idLista;
                    la.accesstype = accessType;

                    tp.listacess.Add(la);
                    tp.SaveChanges();

                    return "Usuario agregado a la lista correctamente";
                }
            }
            else
            {
                //update
                listacess la = ConvertRowToListAccess(listacess.Tables[0].Rows[0]);
                using (TPEntities tp = new TPEntities())
                {
                    la.accesstype = accessType;
                    tp.Entry(la).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Acceso de Usuario actualizado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet listAccessReadById(int idUser, int idList)
        {
            DataSet ds = selectTP("listacess", "*", $"user_id = '{idUser}' and list_id = '{idList}'");
            return ds;
        }
        [WebMethod]
        public string listAccessDelete(int idUser, int idList)
        {

            using (TPEntities tp = new TPEntities())
            {
                listacess deleteListacess = tp.listacess.Find(idUser, idList);

                if (deleteListacess == null)
                {
                    return "El acceso a la lista usuario no existe o hay campos en blanco";
                }
                else
                {
                    tp.listacess.Remove(deleteListacess);
                    tp.SaveChanges();
                    return "El acceso a la lista usuario fue eliminado correctamente";
                }
            }
        }
        //Task
        public task ConvertRowToTask(DataRow row)
        {
            task task = new task
            {
                // title, taskdescription, taskStatus, isfavorite, isonmyday, startdate, enddate, taskPriority, list_id

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
        [WebMethod]
        public string taskCreate(string title, string taskdescription, string taskStatus, bool isfavorite, bool isonmyday, Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate, string taskPriority, int list_id)
        {
            

            DataSet list = listReadById(list_id);
            byte isfav = 0;
            byte isomd = 0;

            if (isfavorite) isfav = 1;
            if (isonmyday) isomd = 1;


            if ((title ?? taskStatus ?? taskPriority) == null || (title ?? taskStatus ?? taskPriority) == "" || list_id == null || isfavorite == null ||  isonmyday == null)
            {
                return "Revisa que los campos obligatorios tengan informacion";
            } 
            else if (list.Tables[0].Rows.Count == 0)
            {
                return "La tarea no se pudo asignar a la lista, revisa que la lista exista.";
            } 
            else if (!(taskStatus == "NOT STARTED" || taskStatus == "IN PROGRESS" || taskStatus == "COMPLETED"))
            {
                return "El status de la tarea solo puede ser NOT STARTED, IN PROGRESS o COMPLETED.";
            }
            else if (!(taskPriority == "LOW" || taskPriority == "MEDIUM" || taskPriority == "IMPORTANT" || taskPriority == "URGENT"))
            {
                return "La prioridad de la tarea solo puede ser LOW, MEDIUM, IMPORTANT, URGENT";
            }
            else
            {
                using (TPEntities tp = new TPEntities())
                {
                    var t = new task();

                    t.title = title;
                    t.taskdescription = taskdescription;
                    t.taskStatus = taskStatus;
                    t.isfavorite = isfav;
                    t.isonmyday = isomd;
                    t.startdate = startdate;
                    t.enddate = enddate;
                    t.taskPriority = taskPriority;
                    t.list_id = list_id;
                    

                    tp.task.Add(t);
                    tp.SaveChanges();

                    return "Tarea agregada correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet taskReadById(int id)
        {
            DataSet ds = selectTP("task", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public string taskUpdate(int id, string title, string taskdescription, string taskStatus, bool isfavorite, bool isonmyday, Nullable<System.DateTime> startdate, Nullable<System.DateTime> enddate, string taskPriority, int list_id)
        {
            DataSet dsTask = taskReadById(id);

            if (dsTask.Tables[0].Rows.Count == 0)
            {
                return "La tarea no existe.";
            }
            else if (listReadById(list_id).Tables[0].Rows.Count == 0)
            {
                return "No existe la lista la cual agregarle una tarea.";
            }
            else
            {
                task t = ConvertRowToTask(dsTask.Tables[0].Rows[0]);
                byte isfav = 0;
                byte isomd = 0;

                if (isfavorite) isfav = 1;
                if (isonmyday) isomd = 1;
                using (TPEntities tp = new TPEntities())
                {
                    t.id = id;
                    t.title = title;
                    t.taskdescription = taskdescription;
                    t.taskStatus = taskStatus;
                    t.isfavorite = isfav;
                    t.isonmyday = isomd;
                    t.startdate = startdate;
                    t.enddate = enddate;
                    t.taskPriority = taskPriority;
                    t.list_id = list_id;

                    tp.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Tag actualizado correctamente";
                }
            }
        }
        [WebMethod]
        public string taskDelete(int id)
        {

            using (TPEntities tp = new TPEntities())
            {
                task deleteTask = tp.task.Find(id);

                if (deleteTask == null)
                {
                    return "La tarea no existe o campos en blanco";
                }
                else
                {
                    tp.task.Remove(deleteTask);
                    tp.SaveChanges();
                    return "La tarea se elimino correctamente";
                }
            }
        }
        //Tag
        public tag ConvertRowToTag(DataRow row)
        {
            tag tag = new tag
            {
                id = int.Parse(row["id"].ToString()),
                tagName = row["tagName"].ToString(),
                list_id = int.Parse(row["list_id"].ToString()),

            };
            return tag;
        }
        [WebMethod]
        public string tagCreate(string tagname, int idList)
        {

            if (tagname == null || tagname == "")
            {
                return "El tag debe tener un nombre, no debe estar en blanco.";
            } else if (listReadById(idList).Tables[0].Rows.Count == 0)
            {
                return "No existe la lista la cual agregarle un tag";
            }

            else 
            {
                using (TPEntities tp = new TPEntities())
                {
                    var tag = new tag();

                    tag.tagName = tagname;
                    tp.tag.Add(tag);
                    tp.SaveChanges();

                    return "Tag agregado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet tagReadById(int id)
        {
            DataSet ds = selectTP("tag", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public string tagUpdate(int id, string tagname, int idList)
        {
            DataSet dsTag = tagReadById(id);

            if (dsTag.Tables[0].Rows.Count == 0)
            {
                return "El tag no existe";
            } else if(listReadById(idList).Tables[0].Rows.Count == 0)
            {
                return "No existe la lista la cual agregarle un tag";
            }
            else
            {
                tag t = ConvertRowToTag(dsTag.Tables[0].Rows[0]);
                using (TPEntities tp = new TPEntities())
                {
                    t.id = id;
                    t.tagName = tagname;
                    t.list_id = idList;
                    tp.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Tag actualizado correctamente";
                }
            }

        }
        [WebMethod]
        public string tagDelete(int id)
        {

            using (TPEntities tp = new TPEntities())
            {
                tag deleteTag = tp.tag.Find(id);

                if (deleteTag == null)
                {
                    return "El tag no existe o campos en blanco";
                }
                else
                {
                    tp.tag.Remove(deleteTag);
                    tp.SaveChanges();
                    return "Tag eliminado correctamente";
                }
            }
        }
        //tasktag
        public string taskTagCreate(int id, int tag_id, int task_id)
        {
            DataSet tag = tagReadById(tag_id);
            DataSet task = taskReadById(task_id);


            if (task == null || task.Tables[0].Rows.Count == 0)
            {
                return "No existe la tarea, por lo cual no se le puede agregar un taskTag";
            } else if (tagReadById(tag_id).Tables[0].Rows.Count == 0 )
            {
                return "No existe un tag, primero agreguelo para crear un tasktag ";
            }
            else
            {
                using (TPEntities tp = new TPEntities())
                {
                    var taskTag = new tasktag();

                    taskTag.id = id;
                    taskTag.tag_id = tag_id;
                    taskTag.task_id = task_id;
                    
                    tp.tasktag.Add(taskTag);
                    tp.SaveChanges();

                    return "Usuario agregado a la lista correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet taskTagReadById(int id)
        {
            DataSet ds = selectTP("tasktag", "*", $"id = '{id}'");
            return ds;
        }

    }
}
