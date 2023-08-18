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
    public partial class ListEdit : Window
    {
        public ListEdit(O.list l, O.user u)
        {
            InitializeComponent();
            this.list = l;
            this.user = u;
            refresh();
        }
        O.list list;
        O.user user;
        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();

        public void refresh()
        {
            lblLista.Content = $"Lista {this.list.listName}";
            txtActualizarNombreLista.Text = this.list.listName;
            DGMiembros.ItemsSource = members();
        }

        public List<listaccess_user> members()
        {
            List<listaccess> la = new ConvertRow().listtolistaccess(sw.listAccessReadByListID(this.list.id));
            List<listaccess_user> lau = new List<listaccess_user>();

            foreach(listaccess a in la)
            {
                listaccess_user l = new listaccess_user();
                l.user =new ConvertRow().user(sw.userReadById(a.user_id)).nickname;
                l.useraccess = $"{a.accesstype} - {new ConvertRow().user(sw.userReadById(a.user_id)).nickname}";
                lau.Add(l);
            }

            return lau;
        }

        private void btndelete_Click(object sender, RoutedEventArgs e)
        {
            sw.listDelete(this.list.id);
            new Landing(this.user).Show();
            this.Close();
        }

        private void btnupdate_Click(object sender, RoutedEventArgs e)
        {
            int id = this.list.id;
            string name = txtActualizarNombreLista.Text;

            string message = sw.listUpdate(id,name);
            MessageBox.Show(message);
            if ("Lista actualizada correctamente" == message)
            {
                new Landing(this.user).Show();
                this.Close();
            } else
            {
                txtActualizarNombreLista.Text = "";
            }
        }

        private void btnaddmember_Click(object sender, RoutedEventArgs e)
        {


            DataSet ds = sw.userReadByNickname(txtMiembro.Text);
           

            if (ds.Tables[0].Rows.Count == 0)
            {
                MessageBox.Show("El usuario no existe");
                txtMiembro.Text = "";
            }
            else
            {
                O.user u = new ConvertRow().user(ds);
                O.list l = this.list;
                int i = cmbTipoAcceso.SelectedIndex;
                string[] access = new string[] {"OWNER", "ADMIN", "MEMBER"};
                string acc = access[i];

                string mess = sw.listAccessCreateUpdate(u.id,l.id,acc);
                MessageBox.Show(mess);
                txtMiembro.Text = "";
                cmbTipoAcceso.SelectedIndex = 2;
                refresh();

            }
        }
        private void btn_deletemember_Click(object sender, RoutedEventArgs e)
        { 
            var u = (string)((Button)sender).CommandParameter;
            sw.listAccessDelete(new ConvertRow().user(sw.userReadByNickname(u)).id,this.list.id);
            refresh();

        }
    }
}
