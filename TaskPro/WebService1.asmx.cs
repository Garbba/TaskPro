using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
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

        [WebMethod]
        public bool emailValidation(string email)
        {
            bool validEmail = Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$", RegexOptions.IgnoreCase);
            return validEmail;
        }

        [WebMethod]
        public string userCreate(string nickname, string username, string lastname, string email, string userpassword)
        {

            DataSet dsNickname = selectTP("userlist", "*", "nickname = '" + nickname + "'");
            DataSet dsEmail = selectTP("userlist", "*", "email = '" + email + "'" );

            if (((nickname ?? username ?? lastname ?? email ?? userpassword) == null) || ((nickname ?? username ?? lastname ?? email ?? userpassword) == ""))
            {
                return "Valide los campos, alguno se encuentra incompleto";
            } else if (dsNickname.Tables[0].Rows.Count == 1)
            {
                return "El usuario ya existe, debes cambiarlo.";
            } else if (dsEmail.Tables[0].Rows.Count == 1 || !emailValidation(email))
            {
                return "El correo ya existe o no tiene el formato correcto, debes cambiarlo.";
            }
            else
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
        


    }
}
