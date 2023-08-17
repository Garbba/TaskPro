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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFTaskPro_WS.O;

namespace WPFTaskPro_WS
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtuser.Text = "";
            txtpass.Password = "";
        }

        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string user = txtuser.Text;
            string pass = txtpass.Password;
            DataSet ds = sw.userLogin(user, pass);
            
            if (ds != null)
            {
                new Landing(new ConvertRow().user(ds)).Show();
                this.Close();


            }
            else
            {
                MessageBox.Show("Usuario o contrasena incorrecta");
                txtuser.Text = "";
                txtpass.Password = "";
            }

        }
    }
}
