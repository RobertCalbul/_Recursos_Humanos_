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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recursos_Humanos_wpf.Interfaz
{
    /// <summary>
    /// Interaction logic for Administrador.xaml
    /// </summary>
    public partial class Administrador : UserControl
    {
        MainWindow main;
        interfazUserGeneral _infoUser;
        public Administrador(MainWindow main)
        {
            InitializeComponent();
            _infoUser = new interfazUserGeneral(main);
            this.main = main;
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            main.WorkSpace.Children.Clear();
            main.WorkSpace.Children.Add(_infoUser);
        }
    }
}
