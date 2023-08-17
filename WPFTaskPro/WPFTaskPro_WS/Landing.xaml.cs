using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFTaskPro_WS.O;

namespace WPFTaskPro_WS
{
    /// <summary>
    /// Lógica de interacción para Landing.xaml
    /// </summary>
    public partial class Landing : Window
    {
        public Landing(user u)
        {
            InitializeComponent();
            tbuser.Text = u.username;
            listselected = null;
            this.user = u;
            Refresh();
        }
        user user;
        list listselected = new list();


        private void Refresh()
        {


            DGListas.ItemsSource = new ConvertRow().listtolist(sw.listReadByUserID(this.user.id));
        }



        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();
        private void btn_newlist(object sender, RoutedEventArgs e)
        {
            string newlist = tb_newlist.Text;
            string commentlist = sw.listCreate(newlist);

            MessageBox.Show(commentlist);

            if (commentlist == "Lista agregada correctamente")
            {
                int idlastlist = sw.listReadAll().Tables[0].Rows.Count - 1;
                DataRow lastlist = sw.listReadAll().Tables[0].Rows[idlastlist];
                int listid = int.Parse(lastlist["id"].ToString());

                sw.listAccessCreateUpdate(this.user.id, listid, "OWNER");
            }

            tb_newlist.Text = "";
            Refresh();
        }

        private void btn_newtask(object sender, RoutedEventArgs e)
        {
            string newtarea = tb_newtask.Text;
            string commenttask = sw.taskCreate(newtarea, "","NOT STARTED", "N","N","","","MEDIUM", 1/*listselected.id*/);

            MessageBox.Show(commenttask);

            tb_newtask.Text = "";
        }

        private void btn_eliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
        }

        




    }
}
