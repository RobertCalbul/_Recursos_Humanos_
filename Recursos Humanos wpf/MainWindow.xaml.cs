using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Recursos_Humanos_wpf.Clases;
using System.Text.RegularExpressions;
using Recursos_Humanos_wpf.Interfaz;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<string> listAutocomplet = null;

        Validaciones validacion = new Validaciones();
        interfazUserGeneral _infoUser;
        interfazBienvenida _bienbenida;
        public MainWindow()
        {
            InitializeComponent();
            listAutocomplet = new Clases.Personal().findAll(0);
            _infoUser = new interfazUserGeneral(this);
            _bienbenida = new interfazBienvenida();
            this.WorkSpace.IsEnabled = false;
            this.rbRut.IsChecked = true;
            this.WorkSpace.Children.Clear();
            this.WorkSpace.Children.Add(_infoUser);
            this.Presentacion.Children.Clear();
            this.Presentacion.Children.Add(_bienbenida);
        }
        /*>>>>INICIO OPERACIONES CRUD EMPLEADOS<<<<<*/
        //BUSCA DATOS EMPLEADOS POR FILTRO



        private void btnBuscar_click(object sender, MouseButtonEventArgs e)
        {
           _infoUser.Search();
            _infoUser.calendar1.Visibility = Visibility.Hidden;
        }

        // click en agregar usuario
        private void iAddUser_Click(object sender, MouseButtonEventArgs e)
        {

            _infoUser.limpiarTexbox();
            this.WorkSpace.IsEnabled = true;
            _infoUser.tDateNaci.IsEnabled = false;
            _infoUser.tRut.IsEnabled = true;
            _infoUser.iPerfil.IsEnabled = true;
            _infoUser.btnAddUser.IsEnabled = true;
            this.cBusqueda.IsEnabled = false;
            this.image1.IsEnabled = false;
            //this.btnAddAfp.Visibility = Visibility.Visible;
            _infoUser.btnAddUser.Visibility = Visibility.Visible;
            _infoUser.btnCancelAdd.Visibility = Visibility.Visible;
            _infoUser.btnAddUser.Visibility = Visibility.Visible;
            _infoUser.btnCancelAdd.Visibility = Visibility.Visible;
            _infoUser.btnUpdateReg.Visibility = Visibility.Hidden;
            _infoUser.btnDeleteReg.Visibility = Visibility.Hidden;
            _infoUser.tabControl1.SelectedIndex = 0;
            _infoUser.tName.Focus();
            this.cBusqueda.Text = "";
            _infoUser.loadDataContract("");
        }
        
/*>>>>AUTOCOMPLETE<<<<<*/
        private void cBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typed = this.cBusqueda.Text.Trim();

            //this.lAutoComplete = new AutoComplete().llenaList(typed);
            List<string> autoList = new List<string>();
            autoList.Clear();
            foreach (string item in listAutocomplet)
            {
                if (!string.IsNullOrEmpty(this.cBusqueda.Text.Trim()))
                {
                    if (item.StartsWith(typed))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                this.lAutoComplete.ItemsSource = autoList;
                this.lAutoComplete.Visibility = Visibility.Visible;
            }
            else if (this.cBusqueda.Text.Equals(""))
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
            else
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
        }
        private void lAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lAutoComplete.ItemsSource != null)
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.cBusqueda.TextChanged -= new TextChangedEventHandler(this.cBusqueda_TextChanged);
                if (this.lAutoComplete.SelectedIndex != -1)
                {
                    this.cBusqueda.Text = this.lAutoComplete.SelectedItem.ToString();
                }
                this.cBusqueda.TextChanged += new TextChangedEventHandler(this.cBusqueda_TextChanged);
                _infoUser.Search();
            }
        }
/*>>>>FIN SOLO AUTOCOMPLETE<<<<<*/

        //FILTRO SEGUN INFORMACION PERSONAL
        private void RadioButtonSearch(object sender, RoutedEventArgs e)
        {
            int parametroSearch = 0;
            if (this.rbRut.IsChecked == true) parametroSearch = 0;
            if (this.rbName.IsChecked == true) parametroSearch = 1;
            if (this.rbSurname.IsChecked == true) parametroSearch = 2;
            if (this.rbPhone.IsChecked == true) parametroSearch = 3;
            if (this.rbAdress.IsChecked == true) parametroSearch = 4;
            if (this.rbEmail.IsChecked == true) parametroSearch = 5;
            this.listAutocomplet = new Clases.Personal().findAll(parametroSearch);
        }

        private void bAcceder_Click(object sender, RoutedEventArgs e)
        {
            animacionLogeo.Begin();
            _bienbenida.animacionPresentacion.Begin();
        }

        private void bAcceder_Click(object sender, MouseButtonEventArgs e)
        {
            Login login = new Login();
            object[] Resultado = login.findBy(tNombreUser.Text, tPasswordUser.Password);
            if (Resultado != null)
            {
                animacionLogeo.Begin();
                _bienbenida.animacionPresentacion.Begin();
                
            }
        }
/*>>>>>RELACIONADA CON LA VENTANA (MOVIMIENTOS, EVENTOS)>>>>*/
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
       private void minimize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }         
        //EVENTOS para el cambio de color de la imagen cerrar X
        private void CambiaColor(object sender, MouseEventArgs e)
        {
            this.image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/CloseRed.png"));
        }              
        private void ColorNormal(object sender, MouseEventArgs e)
        {
            this.image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Close2.png"));
        }
 
        /*>>>>>FIN RELACIONADA CON LA VENTANA (MOVIMIENTOS, EVENTOS)>>>>*/

    }

}
