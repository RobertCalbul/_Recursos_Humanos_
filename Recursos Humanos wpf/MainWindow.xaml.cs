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
using System.Windows.Media.Animation;


namespace Recursos_Humanos_wpf
{
    public partial class MainWindow : Window
    {
        public List<string> listAutocomplet = null;
        interfazUserGeneral _infoUser;
        interfazBienvenida _bienvenida;
    
        public MainWindow()
        {
            InitializeComponent();
            listAutocomplet = new Clases.Personal().findAll(0);
            _infoUser = new interfazUserGeneral(this);
            _bienvenida = new interfazBienvenida();
            this.WorkSpace.IsEnabled = false;
            this.rbRut.IsChecked = true;
            this.WorkSpace.Children.Clear();
            this.WorkSpace.Children.Add(_infoUser);
            this.Presentacion.Children.Clear();
            this.Presentacion.Children.Add(_bienvenida);
            this.tNombreUser.Focus();
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
            _infoUser.tDateNaci.IsEnabled = false;
            _infoUser.tRut.IsEnabled = true;
            _infoUser.iPerfil.IsEnabled = true;
            _infoUser.btnAddUser.IsEnabled = true;
            this.WorkSpace.IsEnabled = true;
            this.cBusqueda.IsEnabled = false;
            this.image1.IsEnabled = false;
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

        public void Logearse() {
            if (!tNombreUser.Text.Trim().Equals(""))
            {
                if (!tPasswordUser.Password.Trim().Equals(""))
                {
                    Login Resultado = new Login(tNombreUser.Text, tPasswordUser.Password).findBy();
                    if (Resultado != null)
                    {
                        this.animacionLogeo.Begin();
                        _bienvenida.animacionPresentacion.Begin();
                        this.cBusqueda.Focus();
                        //Muestra la imagen Bienvenido al Admin
                        Storyboard AnimacionDeBienvenida = (Storyboard)FindResource("AnimacionImaBienvenida");
                        AnimacionDeBienvenida.Begin();
                    }
                }
                else
                {
                    new Dialog("Por favor, ingresa la contraseña de usuario.").ShowDialog();
                    tPasswordUser.Focus();
                }
            }
            else
            {
                new Dialog("Por favor, ingresa el nombre de usuario.").ShowDialog();
                tNombreUser.Focus();
            }
        }


        private void bAcceder_Click(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => { Logearse(); }));
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

        //Implementado enter al logearse (Enter sobre el cuadro de password)
        private void tPasswordUser_KeyDown(object sender, KeyEventArgs e)
        {   
            if (e.Key == Key.Return)
            {
                Dispatcher.BeginInvoke(new Action(() => { Logearse(); }));
            }
        }

        private void btnAdministrativo_Click(object sender, MouseButtonEventArgs e)
        {
            this.WorkSpace.Children.Clear();
            this.WorkSpace.Children.Add(new Administrador(this,_infoUser));
            this.WorkSpace.IsEnabled = true;     
        }




        #region ESTILO VISUAL BOTONES
        public void styleVisualBtn(Label btn, Brush color, int borde)
        {
            btn.BorderBrush = color;
            btn.BorderThickness = new Thickness(borde, 0, 0, 0);
        }
        private void label27_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(label27, Brushes.Blue, 5);
        }

        private void label27_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(label27, null, 0);
        }

        #endregion
      
    }

}
