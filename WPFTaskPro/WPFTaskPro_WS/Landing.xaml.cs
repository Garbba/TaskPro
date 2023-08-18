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
            DGListPreCharge.ItemsSource = new ConvertRow().listprecharged();
            tb_newtask.IsReadOnly = true;
            btnewtask.IsEnabled = false;
            Refresh();
        }
        user user;
        list listselected;


        private void Refresh()
        {
            

            try
            {
                DGListas.ItemsSource = new ConvertRow().listtolist(sw.listReadByUserID(this.user.id));
            }
            catch
            {

            }
            if (listselected == null) {}
            else
            {
                DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByListId(this.listselected.id));
                titulo_tareas.Text =  $"Tareas de la lista {this.listselected.listName}";
            }
            
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
            string commenttask = sw.taskCreate(newtarea, "","NOT STARTED", "N","N","","","MEDIUM", listselected.id);

            MessageBox.Show(commenttask);

            tb_newtask.Text = "";
            Refresh();
            btnewtask.IsEnabled = false;
        }

        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            listselected = new ConvertRow().list(sw.listReadById(id));
            Refresh();
            tb_newtask.IsReadOnly = false;
            btnewtask.IsEnabled = true;

        }
        private void btn_editlist_Click(object sender, RoutedEventArgs e)
        {
            var id = (int)((Button)sender).CommandParameter;
            new ListEdit(new ConvertRow().list(sw.listReadById(id)),this.user).Show();
            this.Close();
        }
        private void btn_eliminarUsuario_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btn_listprecharged_Click(object sender, RoutedEventArgs e)
        {
            listselected = null;
            tb_newtask.IsReadOnly = true;
            var id = (int)((Button)sender).CommandParameter;
            switch (id)
            {
                case 0:
                    //On My day
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUserOnMyDay(this.user.id));
                    titulo_tareas.Text = $"Tareas de tu dia";
                    break;
                case 1:
                    //Favorite
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUserFavorite(this.user.id));
                    titulo_tareas.Text = $"Tareas favoritas";
                    break;
                case 2:
                    //Assigned to me
                    DGTasks.ItemsSource = new ConvertRow().listtotask(sw.taskReadByUser(this.user.id));
                    titulo_tareas.Text = $"Tareas asignadas a ti";
                    break;

            }


        }
        private void btn_task_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Visibility = Visibility.Visible;
            contentGrid.Width = 200;

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            contentGrid.Visibility = Visibility.Collapsed;
            contentGrid.Width = 0;
        }
        




    }
}
