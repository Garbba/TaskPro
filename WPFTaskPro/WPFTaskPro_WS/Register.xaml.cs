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

namespace WPFTaskPro_WS
{
    /// <summary>
    /// Lógica de interacción para Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }
        SWRef.WebService1SoapClient sw = new SWRef.WebService1SoapClient();
        private void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {

            string mess = sw.userCreate(txtNickname.Text,txtUsername.Text, txtLastname.Text, txtEmail.Text, txtPassword.Password) ;
            MessageBox.Show(mess); 
            
            if ("Usuario agregado correctamente" == mess)
            {
                new MainWindow().Show();
                this.Close();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
