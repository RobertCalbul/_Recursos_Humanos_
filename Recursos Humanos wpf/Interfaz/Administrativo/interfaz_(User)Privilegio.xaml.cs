using Recursos_Humanos_wpf.Clases;
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

namespace Recursos_Humanos_wpf.Interfaz.Administrativo
{
    /// <summary>
    /// Lógica de interacción para interfaz__User_Privilegio.xaml
    /// </summary>
    public partial class interfaz__User_Privilegio : UserControl
    {
        public interfaz__User_Privilegio()
        {
            InitializeComponent();
            this.data.ItemsSource = new Privilegio().findAll();
            this.btnActualizar.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Hidden;
            this.btnCancelAdd.Visibility = Visibility.Hidden;
            this.tName.IsEnabled = false;
            this.btnDelete.IsEnabled = false;
        }

        private void btnUpdate_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Realmente desea modificar los datos?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (new Privilegio(int.Parse(this.tId.Text), this.tName.Text).update() > 0)
                {
                    this.data.ItemsSource = new Privilegio().findAll();
                }
            }
        }

        private void btnAdd_Click(object sender, MouseButtonEventArgs e)
        {
            if (!this.tName.Text.Equals(""))
            {
                new Privilegio(this.tName.Text).save();
                this.data.ItemsSource = new Privilegio().findAll();
                this.data.IsEnabled = true;
                this.btnActualizar.IsEnabled = true;
                this.btnActualizar.Visibility = Visibility.Hidden;
                this.btnCancelAdd.Visibility = Visibility.Hidden;
                this.btnAdd.Visibility = Visibility.Hidden;
            }
            else new Dialog("Ingrese un nombre al nuevo Privilegio.").Show();
        }

        private void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {
            this.data.IsEnabled = true;
            this.tName.IsEnabled = false;
            this.btnCancelAdd.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Hidden;
        }

        private void btnNewItem_Click(object sender, MouseButtonEventArgs e)
        {
            this.tName.IsEnabled = true;
            this.data.IsEnabled = false;
            this.btnActualizar.Visibility = Visibility.Hidden;
            this.btnAdd.Visibility = Visibility.Visible;
            this.btnCancelAdd.Visibility = Visibility.Visible;
            this.btnDelete.IsEnabled = false;

            limpaCampos();
            this.tName.Focus();
        }

        private void btnDelete_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Realmente desea eliminar este dato?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (new Privilegio(int.Parse(this.tId.Text)).deleteById() > 0)
                {
                    this.data.ItemsSource = new Privilegio().findAll();
                    limpaCampos();
                }
            }
        }

        private void limpaCampos()
        {
            this.tId.Text = "";
            this.tName.Text = "";
        }

        private void data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (object obj in this.data.SelectedItems)
            {
                this.tName.Text = ((Privilegio)obj).name;
                this.tId.Text = ((Privilegio)obj).id + "";
            }

            this.btnActualizar.IsEnabled = true;
            this.btnDelete.IsEnabled = true;
            this.btnAdd.Visibility = Visibility.Hidden;
            this.btnActualizar.Visibility = Visibility.Visible;
            this.tName.IsEnabled = true;
        }


        #region ESTILO VISUAL BOTONES
        public void styleVisualBtn(Label btn, Brush color, int borde)
        {
            btn.BorderBrush = color;
            btn.BorderThickness = new Thickness(borde, 0, 0, 0);
        }
        private void btnCancelAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnCancelAdd, Brushes.Red, 5);
        }

        private void btnCancelAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnCancelAdd, null, 0);
        }

        private void btnDelete_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnDelete, Brushes.Red, 5);
        }

        private void btnDelete_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnDelete, null, 0);
        }

        private void btnAdd_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnAdd, Brushes.Blue, 5);
        }

        private void btnAdd_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnAdd,null, 0);
        }

        private void btnNewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnNewItem, Brushes.Green, 5);
        }

        private void btnNewItem_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnNewItem, null, 0);
        }

        private void btnActualizar_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnActualizar, Brushes.Blue, 5);
        }

        private void btnActualizar_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnActualizar, null, 0);
        }
        #endregion
    }
}
