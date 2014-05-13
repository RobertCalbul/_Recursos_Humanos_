using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, MouseButtonEventArgs e)
        {
           if (tUser.Text.Equals("1") && tPassword.Text.Equals("1"))
            {
                new MainWindow().Show();
                this.Close();
            }
            else { 
               new Dialog("Usuario: 1 \nPassword: 1").Show();
            }
        }

        private void arrastrar(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
