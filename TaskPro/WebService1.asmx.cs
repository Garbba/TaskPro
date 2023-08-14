using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
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
        #region DBconnection
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
        #endregion DBconnection
        #region Validations
        public bool validDate(string date)
        {
            try
            {
                DateTime stdate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ValidDateTime(string date)
        {
            if (DateTime.TryParseExact(date, "dd/MM/yyyy HH:mm:ss", null, DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate.TimeOfDay.TotalSeconds >= 0 && parsedDate.TimeOfDay.TotalSeconds < 86400;
            }
            return false;
        }
        public bool emailValidation(string email)
        {
            bool validEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase);
            return validEmail;
        }
        public bool linkValidation(string input)
        {
            // Expresión regular para validar el formato de un enlace (URL)
            string pattern = @"^(http|https|ftp)://[a-zA-Z0-9-.]+.[a-zA-Z]{2,3}(/[^/]*)*$";

            return Regex.IsMatch(input, pattern);
        }
        #endregion Validations
        #region User
        public string userValidation(string nickname, string username, string lastname, string email, string userpassword)
        {
            DataSet dsNickname = selectTP("userlist", "*", $"nickname = '{nickname}'");
            DataSet dsEmail = selectTP("userlist", "*", $"email = '{email}'");

            if (nickname == "" || username == "" || lastname == "" || userpassword == "")
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
        public DataSet userReadAll()
        {
            DataSet ds = selectTP("userlist", "*", null);
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
        #endregion User
        #region List
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
        public DataSet listReadByUserID(int id)
        {
            DataSet ds = selectTP("listacess la JOIN list l ON l.id = la.list_id", "list_id,accesstype,listName", $"user_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet listReadAll()
        {
            DataSet ds = selectTP("list", "*", null);
            return ds;
        }
        [WebMethod]
        public string listUpdate(int id, string listname)
        {
            DataSet dsList = listReadById(id);
            if (listname == null || listname == "")
            {
                return "La lista debe tener un nombre";
            } 
            else if (dsList.Tables[0].Rows.Count == 0)
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
        #endregion list
        #region ListAccess
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
        public DataSet listAccessReadAll()
        {
            DataSet ds = selectTP("listacess", "*", null);
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
        #endregion ListAccess
        #region Task
        public task ConvertRowToTask(DataRow row)
        {
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
        [WebMethod]
        public string taskCreate(string title, string taskdescription, string taskStatus, string isfavorite, string isonmyday, string startdate, string enddate, string taskPriority, int list_id)
        {
            if (!(isfavorite == "Y" || isfavorite == "N") || !(isonmyday == "Y" || isonmyday == "N") || isfavorite == "" || isonmyday == "")
            {
                return "isfavorite y isonmyday deben ser Y o N";
            } 
            else if (((startdate != "") && !validDate(startdate)) || ((enddate != "") && !validDate(enddate)))
            {
                return "La fecha de inicio o fin deben tener el formato dd/mm/yyyy";
            }

            byte isfav = 0;
            byte isomd = 0;

            if (isfavorite == "Y") isfav = 1;
            if (isonmyday == "Y") isomd = 1;

            DateTime? stdate = null;
            DateTime? eddate = null;
            if (startdate != "") stdate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            if (enddate != "") eddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

            DataSet list = listReadById(list_id);

            if (title == "" || taskStatus == "" || taskPriority == "")
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
                    t.startdate = stdate;
                    t.enddate = eddate;
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
        public DataSet taskReadByListId(int id)
        {
            DataSet ds = selectTP("task", "*", $"list_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet taskReadAll()
        {
            DataSet ds = selectTP("task", "*", null);
            return ds;
        }
        [WebMethod]
        public string taskUpdate(int id, string title, string taskdescription, string taskStatus, string isfavorite, string isonmyday, string startdate, string enddate, string taskPriority, int list_id)
        {
            if (taskReadById(id).Tables[0].Rows.Count == 0)
            {
                return "La tarea no existe.";
            } 
            else if (listReadById(list_id).Tables[0].Rows.Count == 0)
            {
                return "No existe la lista la cual agregarle una tarea.";
            }
            else if (!(isfavorite == "Y" || isfavorite == "N") || !(isonmyday == "Y" || isonmyday == "N") || isfavorite == "" || isonmyday == "")
            {
                return "isfavorite y isonmyday deben ser Y o N";
            }
            else if (((startdate != "") && !validDate(startdate)) || ((enddate != "") && !validDate(enddate)))
            {
                return "La fecha de inicio o fin deben tener el formato dd/mm/yyyy";
            }

            byte isfav = 0;
            byte isomd = 0;

            if (isfavorite == "Y") isfav = 1;
            if (isonmyday == "Y") isomd = 1;

            DateTime? stdate = null;
            DateTime? eddate = null;
            if (startdate != "") stdate = DateTime.ParseExact(startdate, "dd/MM/yyyy", null);
            if (enddate != "") eddate = DateTime.ParseExact(enddate, "dd/MM/yyyy", null);

            if (title == "" || taskStatus == "" || taskPriority == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
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
                task t = ConvertRowToTask(taskReadById(id).Tables[0].Rows[0]);

                using (TPEntities tp = new TPEntities())
                {
                    t.id = id;
                    t.title = title;
                    t.taskdescription = taskdescription;
                    t.taskStatus = taskStatus;
                    t.isfavorite = isfav;
                    t.isonmyday = isomd;
                    t.startdate = stdate;
                    t.enddate = eddate;
                    t.taskPriority = taskPriority;
                    t.list_id = list_id;

                    tp.Entry(t).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();
                    return "Tarea actualizada correctamente";
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
        #endregion
        #region Tag
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
                    tag.list_id = idList;
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
        public DataSet tagReadByListId(int id)
        {
            DataSet ds = selectTP("tag", "*", $"list_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet tagReadByTaskId(int id)
        {
            DataSet ds = selectTP("tag t JOIN tasktag tt ON tt.tag_id = t.id", "t.id, t.tagName,tt.task_id", $"task_id= '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet tagReadAll()
        {
            DataSet ds = selectTP("tag", "*", null);
            return ds;
        }
        [WebMethod]
        public string tagUpdate(int id, string tagname, int idList)
        {
            DataSet dsTag = tagReadById(id);

            if (tagname == null || tagname == "")
            {
                return "El tag debe tener un nombre";
            }
            else if (dsTag.Tables[0].Rows.Count == 0)
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
        #endregion
        #region TaskTag
        public tasktag ConvertRowToTaskTag(DataRow row)
        {
            tasktag tasktag = new tasktag
            {
                id = int.Parse(row["id"].ToString()),
                task_id = int.Parse(row["task_id"].ToString()),
                tag_id = int.Parse(row["tag_id"].ToString())

            };
            return tasktag;
        }
        [WebMethod]
        public string taskTagCreate(int tag_id, int task_id)
        {

            if (taskReadById(task_id).Tables[0].Rows.Count == 0)
            {
                return "No existe la tarea.";
            } 
            else if (tagReadById(tag_id).Tables[0].Rows.Count == 0 )
            {
                return "No existe un tag.";
            } 
            else if (taskTagReadById(tag_id, task_id).Tables[0].Rows.Count != 0)
            {
                return "El tag ya esta agregado a la tarea.";
            }
            else
            {
                using (TPEntities tp = new TPEntities())
                {
                    var taskTag = new tasktag();

                    taskTag.tag_id = tag_id;
                    taskTag.task_id = task_id;
                    
                    tp.tasktag.Add(taskTag);
                    tp.SaveChanges();

                    return "Tag agregado a la tarea correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet taskTagReadById(int tag_id, int task_id)
        {
            DataSet ds = selectTP("tasktag", "*", $"tag_id = '{tag_id}' and task_id = '{task_id}'");
            return ds;
        }
        [WebMethod]
        public DataSet taskTagReadAll()
        {
            DataSet ds = selectTP("tasktag", "*", null);
            return ds;
        }
        [WebMethod]
        public string taskTagDelete(int tag_id, int task_id)
        {
            
            using (TPEntities tp = new TPEntities())
            {
                tasktag deletetasktag = tp.tasktag.Find(ConvertRowToTaskTag(taskTagReadById(tag_id, task_id).Tables[0].Rows[0]).id);

                if (deletetasktag == null)
                {
                    return "El tag de la tarea no existe o hay campos en blanco";
                }
                else
                {
                    tp.tasktag.Remove(deletetasktag);
                    tp.SaveChanges();
                    return "El tag de la tarea fue eliminado correctamente";
                }
            }
        }
        #endregion
        #region Attachment
        public attachment ConvertRowToAttachment(DataRow row)
        {
            attachment attachment = new attachment
            {
                id = int.Parse(row["id"].ToString()),
                datefile = DateTime.ParseExact(row["datefile"].ToString(), "dd/MM/yyyy", null),
                attachmentFilename = row["attachmentFilename"].ToString(),
                attachmentLink = row["attachmentLink"].ToString(),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["list_id"].ToString()),
            };
            return attachment;
        }
        [WebMethod]
        public string attachmentCreate(string datefile, string attachmentFilename, string attachmentLink, int user_id, int task_id)
        {
             if (((datefile == "") || !validDate(datefile)))
            {
                return "La fecha para subir un archivo debe tener el formato dd/mm/yyyy";
            }

            DataSet user = userReadById(user_id);
            DataSet task = taskReadById(task_id);

            if (attachmentFilename == "" || attachmentLink == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
            }
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "El usuario no fue encontrado para agregar el archivo, revisa que el usuario exista.";
            }
            else if (task.Tables[0].Rows.Count == 0)
            {
                return "La tarea no fue encontrada para agregar el archivo, revisa que la tarea exista.";
            }
            else if (linkValidation(attachmentLink))
            {
                return "El link no tiene un formato correcto, intente con otro formato de nuevo.";
            }
            else
            {
                DateTime dtfile = DateTime.ParseExact(datefile, "dd/MM/yyyy", null);
                using (TPEntities tp = new TPEntities())
                {
                    var a = new attachment();
                    a.datefile = dtfile;
                    a.attachmentFilename = attachmentFilename;
                    a.attachmentLink = attachmentLink;
                    a.user_id = user_id;
                    a.task_id = task_id;
                    tp.attachment.Add(a);
                    tp.SaveChanges();

                    return "Archivo adjuntado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet attachmentReadById(int id)
        {
            DataSet ds = selectTP("task", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet attachmentReadByTaskId(int id)
        {
            DataSet ds = selectTP("attachment", "*", $"task_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet attachmentReadAll()
        {
            DataSet ds = selectTP("attachment", "*", null);
            return ds;
        }
        [WebMethod]
        public string attachmentUpdate(int id, string datefile, string attachmentFilename, string attachmentLink, int user_id, int task_id)
        {
            if (((datefile == "") || !validDate(datefile)))
            {
                return "La fecha para subir un archivo debe tener el formato dd/mm/yyyy";
            }

            DataSet user = userReadById(user_id);
            DataSet task = taskReadById(task_id);

            if (attachmentFilename == "" || attachmentLink == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
            }
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "El usuario no fue encontrado para agregar el archivo, revisa que el usuario exista.";
            }
            else if (task.Tables[0].Rows.Count == 0)
            {
                return "La tarea no fue encontrada para agregar el archivo, revisa que la tarea exista.";
            }
            else if (linkValidation(attachmentLink))
            {
                return "El link no tiene un formato correcto, intente con otro formato de nuevo.";
            }
            else
            {
                DateTime dtfile = DateTime.ParseExact(datefile, "dd/MM/yyyy", null);
                using (TPEntities tp = new TPEntities())
                {
                    var a = new attachment();
                    a.datefile = dtfile;
                    a.attachmentFilename = attachmentFilename;
                    a.attachmentLink = attachmentLink;
                    a.user_id = user_id;
                    a.task_id = task_id;
                    tp.Entry(a).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();

                    return "Archivo actualizado correctamente";
                }
            }
        }
        [WebMethod]
        public string attachmentDelete(int id)
        {

            using (TPEntities tp = new TPEntities())
            {
                attachment attachment = tp.attachment.Find(id);

                if (attachment == null)
                {
                    return "El archivo no existe";
                }
                else
                {
                    tp.attachment.Remove(attachment);
                    tp.SaveChanges();
                    return "El archivo se eliminó correctamente";
                }
            }
        }
        #endregion
        #region Comments
        public commentUser ConvertRowToCommentUser(DataRow row)
        {
            commentUser commentUser = new commentUser
            {
                id = int.Parse(row["id"].ToString()),
                datecomment = DateTime.ParseExact(row["datecomment"].ToString(), "dd/MM/yyyy", null),
                commentUser1 = row["commentUser"].ToString(),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["list_id"].ToString()),
            };
            return commentUser;
        }
        [WebMethod]
        public string commentCreate(string dateComment, string commentUser, int user_id, int task_id)
        {
            if (((dateComment == "") || !validDate(dateComment)))
            {
                return "La fecha para subir un archivo debe tener el formato dd/mm/yyyy";
            }

            DataSet user = userReadById(user_id);
            DataSet task = taskReadById(task_id);

            if (commentUser == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
            }
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "El usuario no fue encontrado para agregar el comentario, revisa que el usuario exista.";
            }
            else if (task.Tables[0].Rows.Count == 0)
            {
                return "La tarea no fue encontrada para agregar el comentario, revisa que la tarea exista.";
            }
            else
            {
                DateTime dtfile = DateTime.ParseExact(dateComment, "dd/MM/yyyy", null);
                using (TPEntities tp = new TPEntities())
                {
                    var c = new commentUser();
                    c.datecomment = dtfile;
                    c.commentUser1 = commentUser;
                    c.user_id = user_id;
                    c.task_id = task_id;
                    tp.commentUser.Add(c);
                    tp.SaveChanges();

                    return "Comentario agregado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet commentReadById(int id)
        {
            DataSet ds = selectTP("commentUser", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet commentReadByTaskId(int id)
        {
            DataSet ds = selectTP("commentUser", "*", $"task_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet commentReadAll()
        {
            DataSet ds = selectTP("commentUser", "*", null);
            return ds;
        }

        #endregion
        #region TimeTrack
        public timetrack ConvertRowToTimeTrack(DataRow row)
        {
            timetrack timetrack = new timetrack
            {
                id = int.Parse(row["id"].ToString()),
                starttime = DateTime.ParseExact(row["datefile"].ToString(), "dd/MM/yyyy HH:mm:ss", null),
                endtime = DateTime.ParseExact(row["datefile"].ToString(), "dd/MM/yyyy HH:mm:ss", null),
                attachmentFilename = row["attachmentFilename"].ToString(),
                attachmentLink = row["attachmentLink"].ToString(),
                user_id = int.Parse(row["user_id"].ToString()),
                task_id = int.Parse(row["list_id"].ToString()),
            };
            return timetrack;
        }
        [WebMethod]
        public string attachmentCreate(string datefile, string attachmentFilename, string attachmentLink, int user_id, int task_id)
        {
            if (((datefile == "") || !validDate(datefile)))
            {
                return "La fecha para subir un archivo debe tener el formato dd/mm/yyyy";
            }

            DataSet user = userReadById(user_id);
            DataSet task = taskReadById(task_id);

            if (attachmentFilename == "" || attachmentLink == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
            }
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "El usuario no fue encontrado para agregar el archivo, revisa que el usuario exista.";
            }
            else if (task.Tables[0].Rows.Count == 0)
            {
                return "La tarea no fue encontrada para agregar el archivo, revisa que la tarea exista.";
            }
            else if (linkValidation(attachmentLink))
            {
                return "El link no tiene un formato correcto, intente con otro formato de nuevo.";
            }
            else
            {
                DateTime dtfile = DateTime.ParseExact(datefile, "dd/MM/yyyy", null);
                using (TPEntities tp = new TPEntities())
                {
                    var a = new attachment();
                    a.datefile = dtfile;
                    a.attachmentFilename = attachmentFilename;
                    a.attachmentLink = attachmentLink;
                    a.user_id = user_id;
                    a.task_id = task_id;
                    tp.attachment.Add(a);
                    tp.SaveChanges();

                    return "Archivo adjuntado correctamente";
                }
            }
        }
        [WebMethod]
        public DataSet attachmentReadById(int id)
        {
            DataSet ds = selectTP("task", "*", $"id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet attachmentReadByTaskId(int id)
        {
            DataSet ds = selectTP("attachment", "*", $"task_id = '{id}'");
            return ds;
        }
        [WebMethod]
        public DataSet attachmentReadAll()
        {
            DataSet ds = selectTP("attachment", "*", null);
            return ds;
        }
        [WebMethod]
        public string attachmentUpdate(int id, string datefile, string attachmentFilename, string attachmentLink, int user_id, int task_id)
        {
            if (((datefile == "") || !validDate(datefile)))
            {
                return "La fecha para subir un archivo debe tener el formato dd/mm/yyyy";
            }

            DataSet user = userReadById(user_id);
            DataSet task = taskReadById(task_id);

            if (attachmentFilename == "" || attachmentLink == "")
            {
                return "Revisa que los campos obligatorios tengan informacion";
            }
            else if (user.Tables[0].Rows.Count == 0)
            {
                return "El usuario no fue encontrado para agregar el archivo, revisa que el usuario exista.";
            }
            else if (task.Tables[0].Rows.Count == 0)
            {
                return "La tarea no fue encontrada para agregar el archivo, revisa que la tarea exista.";
            }
            else if (linkValidation(attachmentLink))
            {
                return "El link no tiene un formato correcto, intente con otro formato de nuevo.";
            }
            else
            {
                DateTime dtfile = DateTime.ParseExact(datefile, "dd/MM/yyyy", null);
                using (TPEntities tp = new TPEntities())
                {
                    var a = new attachment();
                    a.datefile = dtfile;
                    a.attachmentFilename = attachmentFilename;
                    a.attachmentLink = attachmentLink;
                    a.user_id = user_id;
                    a.task_id = task_id;
                    tp.Entry(a).State = System.Data.Entity.EntityState.Modified;
                    tp.SaveChanges();

                    return "Archivo actualizado correctamente";
                }
            }
        }
        [WebMethod]
        public string attachmentDelete(int id)
        {

            using (TPEntities tp = new TPEntities())
            {
                attachment attachment = tp.attachment.Find(id);

                if (attachment == null)
                {
                    return "El archivo no existe";
                }
                else
                {
                    tp.attachment.Remove(attachment);
                    tp.SaveChanges();
                    return "El archivo se eliminó correctamente";
                }
            }
        }

        #endregion
    }
}
