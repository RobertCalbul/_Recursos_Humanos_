using Recursos_Humanos_wpf.Clases;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Recursos_Humanos_wpf.Interfaz.Administrativo
{
    /// <summary>
    /// Interaction logic for Administrativo_interfazDepto.xaml
    /// </summary>
    public partial class interfazDepto : UserControl
    {
        List<Departamento> _Departamento;
        public interfazDepto()
        {
            InitializeComponent();

            this.data.ItemsSource = new Departamento().findAll_administrativo();

            this.btnActualizar.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Hidden;
            this.btnCancelAdd.Visibility = Visibility.Hidden;
            this.tName.IsEnabled = false;
            this.tRut.IsEnabled = false;
        }
        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object obj in this.data.SelectedItems)
            {
                this.tName.Text = ((Departamento)obj).name;
                this.tId.Text = ((Departamento)obj).id + "";
                this.tRut.Text = ((Departamento)obj).rut_jefe;
            }

            this.btnActualizar.IsEnabled = true;
            this.btnDelete.IsEnabled = true;
            this.btnAdd.Visibility = Visibility.Hidden;
            this.btnActualizar.Visibility = Visibility.Visible;
            this.tRut.IsEnabled = true;
            this.tName.IsEnabled = true;
        }

        private void btnUpdate_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Realmente desea modificar los datos?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (new Departamento(int.Parse(this.tId.Text), this.tName.Text, this.tRut.Text).update() > 0)
                {
                    _Departamento = new Departamento().findAll_administrativo();
                    this.data.ItemsSource = _Departamento;

                }
            }
        }

        private void btnAdd_Click(object sender, MouseButtonEventArgs e)
        {
            if (!this.tName.Text.Equals(""))
            {
                new Departamento(this.tName.Text, this.tRut.Text != "" ? this.tRut.Text : "0").save();
                _Departamento = new Departamento().findAll_administrativo();

                this.data.ItemsSource = _Departamento;
                this.data.IsEnabled = true;
                this.btnActualizar.IsEnabled = true;
                this.btnActualizar.Visibility = Visibility.Hidden;
                this.btnCancelAdd.Visibility = Visibility.Hidden;
                this.btnAdd.Visibility = Visibility.Hidden;
            }
            else new Dialog("Ingrese un nombre al nuevo departamento").Show();
        }

        private void btnDelete_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Realmente desea eliminar este dato?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (new Departamento(int.Parse(this.tId.Text)).Delete() > 0)
                {
                    _Departamento = new Departamento().findAll_administrativo();
                    this.data.ItemsSource = _Departamento;
                }
            }
        }

        private void btnNewItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.tName.IsEnabled = true;
            this.tRut.IsEnabled = true;
            this.data.IsEnabled = false;
            this.btnActualizar.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Visible;
            this.btnCancelAdd.Visibility = Visibility.Visible;
            this.btnDelete.IsEnabled = false;

            this.tRut.Text = "";
            this.tId.Text = "";
            this.tName.Text = "";
            this.tName.Focus();
        }

        private void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {
            this.data.IsEnabled = true;
            this.tRut.IsEnabled = false;
            this.tName.IsEnabled = false;
            this.btnCancelAdd.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Hidden;
        }
    }
}
