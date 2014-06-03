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

using System.Collections.ObjectModel;
namespace Recursos_Humanos_wpf.Interfaz.Administrativo
{
    /// <summary>
    /// Interaction logic for Administrativo_interfazUser.xaml
    /// </summary>
    public partial class interfazUser : UserControl
    {
        MenuItem root;
        public interfazUser()
        {
            InitializeComponent();

        }

        #region ESTILO VISUAL BOTONES
        public void styleVisualBtn(Label btn, Brush color,int borde) {
            btn.BorderBrush = color;
            btn.BorderThickness = new Thickness(borde, 0, 0, 0);
        }
        private void btnAdduser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnAdduser, Brushes.Green, 5);
        }

        private void btnAdduser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnAdduser, null, 0);
        }

        private void btnDeleteUserGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUserGroup, Brushes.Red, 5);
        }

        private void btnDeleteUserGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUserGroup, null, 0);
        }
        

        private void btnAddUserGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnAddUserGroup, Brushes.Green,5);
        }

        private void btnAddUserGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnAddUserGroup, null, 0);
        }

        private void btnFilter_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnFilter, Brushes.Blue, 5);
        }

        private void btnFilter_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnFilter, null, 0);
        }

        private void btnDeleteUser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUser, Brushes.Red, 5);
        }

        private void btnDeleteUser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUser, null, 0);
        }

        private void btnNewUser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnNewUser, Brushes.Green, 5);
        }

        private void btnNewUser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnNewUser, null, 0);
        }
        #endregion
    }
}
