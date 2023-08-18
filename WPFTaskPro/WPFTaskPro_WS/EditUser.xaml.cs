using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public EditUser(O.user u)
        {
            InitializeComponent();
            this.user = u;
            refresh();
        }

        public void refresh()
        {
            txtNickname.Text = user.nickname;
            txtUsername.Text = user.username; 
            txtLastname.Text = user.lastname; 
            txtEmail.Text = user.email; 
            txtPassword.Password = user.userpassword;
        }
        O.user user;

        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();
        private void UserUpdate_Click(object sender, RoutedEventArgs e)
        {
            string mess = sw.userUpdate(this.user.id,txtNickname.Text, txtUsername.Text, txtLastname.Text, txtEmail.Text, txtPassword.Password);
            MessageBox.Show(mess);
            if ("Usuario actualizado correctamente" == mess)
            {
                new Landing(new ConvertRow().user(sw.userReadById(this.user.id))).Show();
                this.Close();
            }
        }

        private void UserDelete_Click(object sender, RoutedEventArgs e)
        {
            string mess = sw.userDelete(this.user.id);
            MessageBox.Show(mess);
            new MainWindow().Show();
            this.Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hasta pronto");
            new MainWindow().Show();
            this.Close();
        }
    }
}
