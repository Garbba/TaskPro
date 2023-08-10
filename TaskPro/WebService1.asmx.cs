using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        /* CREATE EXAMPLE
          
          using (DAL.dbshopadealEntities db = new DAL.dbshopadealEntities())
                        {
                            var usuario = new DAL.usuario();

                            usuario.nombreusuario = this.usuario;
                            usuario.clave = this.clave;

                            db.usuario.Add(usuario);
                            db.SaveChanges();
                        }

            READ/RETRIEVE EXAMPLE

            string us;
             using (DAL.dbshopadealEntities db = new DAL.dbshopadealEntities())
                { us = db.usuario.Find(u.cedula); }      

            
            UPDATE EXAMPLE
            
             using (DAL.dbshopadealEntities db = new DAL.dbshopadealEntities())
                    {
                        DAL.usuario usuario = db.usuario.Find(this.usuario.cedula);

                        usuario.nombreusuario = nombreusuario;
                        usuario.clave = clave;

                        db.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                     }

             DELETE EXAMPLE

            using (DAL.dbshopadealEntities db = new DAL.dbshopadealEntities())
                {
                    var user = db.usuario.Find(this.usuario.cedula);
                    db.usuario.Remove(user);
                    db.SaveChanges();
                }
         
         */

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

        public string userValidation (string nickname, string username, string lastname, string email, string userpassword)
        {
            DataSet dsNickname = selectTP("userlist", "*", $"nickname = '{nickname}'");
            DataSet dsEmail = selectTP("userlist", "*", $"email = '{email}'");

            if (((nickname ?? username ?? lastname ?? email ?? userpassword) == null) || ((nickname ?? username ?? lastname ?? email ?? userpassword) == ""))
            {
                return "Valide los campos, alguno se encuentra incompleto";
            } else if (dsNickname.Tables[0].Rows.Count == 1)
            {
                return "El usuario ya existe, debes cambiarlo.";
            } else if (dsEmail.Tables[0].Rows.Count == 1 || !emailValidation(email))
            {
                return "El correo ya existe o no tiene el formato correcto, debes cambiarlo.";
            } else {
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

        public list ConvertRowToList(DataRow row)
        {
            list list = new list
            {
                id = int.Parse(row["id"].ToString()),
                listName = row["listName"].ToString(),
            };
            return list;
        }

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
            userlist u = ConvertRowToUsuario(dsUser.Tables[0].Rows[0]);

            if (dsUser.Tables[0].Rows.Count == 0)
            {
                return "El usuario no existe";
            } else if (((nickname ?? username ?? lastname ?? email ?? userpassword) == null) || ((nickname ?? username ?? lastname ?? email ?? userpassword) == ""))
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
            if ((nicknameOrEmail == null) || (nicknameOrEmail == ""))
            {
                return null;
            }
            else if (email.Tables[0].Rows.Count != 0)
            {
                return email;
            } 
            else if (nickname.Tables[0].Rows.Count != 0)
            {
                return nickname;
            }
            else
            {
                return null;
            }
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
        public string listDeletet(int id)
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





    }
}
