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
        private ListBoxItem _dragged;
        List<Login> _listLogin;
        List<User_Group> _listUserGroup;
        public interfazUser()
        {
            InitializeComponent();
            
            _listUserGroup = new User_Group().findAll();
            llenaListLogin();
            llenaListUserGroup();
            this.workSpace.IsEnabled = false;
            this.btnActualizarUser.Visibility = Visibility.Hidden;
        }
        #region Drag and Drop ListBox
        private void lListUserGroup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_dragged != null)
                return;

            UIElement element = this.lListUserGroup.InputHitTest(e.GetPosition(this.lListUserGroup)) as UIElement;

            while (element != null)
            {
                if (element is ListBoxItem)
                {
                    _dragged = (ListBoxItem)element;
                    break;
                }
                element = VisualTreeHelper.GetParent(element) as UIElement;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragged == null)
                return;
            if (e.LeftButton == MouseButtonState.Released)
            {
                _dragged = null;
                return;
            }

            DataObject obj = new DataObject(DataFormats.Text, _dragged.ToString());
            DragDrop.DoDragDrop(_dragged, obj, DragDropEffects.All);
        }
        private void lListDestinoUserGroup_DragEnter(object sender, DragEventArgs e)
        {
            if (_dragged == null || e.Data.GetDataPresent(DataFormats.Text, true) == false)
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.All;
        }

        private void lListDestinoUserGroup_Drop(object sender, DragEventArgs e)
        {
            this.lListUserGroup.Items.Remove(_dragged);
            this.lListDestinoUserGroup.Items.Add(_dragged);
        }

        #endregion

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

        private void btnFilter_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnFilter, Brushes.Blue, 5);
        }

        private void btnFilter_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnFilter, null, 0);
        }
        private void btnActualizarUser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnActualizarUser, Brushes.Blue, 5);
        }

        private void btnActualizarUser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnActualizarUser, null, 0);
        }
        private void btnDeleteUser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUser, Brushes.Red, 5);
        }

        private void btnDeleteUser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnDeleteUser, null, 0);
        }
        private void btnCancelAddUser_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnCancelAddUser, Brushes.Red, 5);
        }

        private void btnCancelAddUser_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(this.btnCancelAddUser, null, 0);
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
        #region llenado automatico (INICIO)
        private void llenaListLogin() {
            this.lListLogin.Items.Clear();
            _listLogin = new Login().findAll();
            foreach(Login users in _listLogin)
                this.lListLogin.Items.Add(users.nombre);

        }
        private void llenaListUserGroup()
        {
            this.lListUserGroup.Items.Clear();
            foreach (User_Group userGroups in _listUserGroup)
                lListUserGroup.Items.Add(new ListBoxItem() { Content = userGroups.name });
        }
        #endregion
       
        private void Clear()
        {
            this.tUserName.Text = "";
            this.tPassword.Text = "";
            this.lListDestinoUserGroup.Items.Clear();
        }

        //Cuando seleccione uno
        private void lListLogin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.lListDestinoUserGroup.Items.Clear();
            int index = this.lListLogin.SelectedIndex;            
            this.tUserName.Text = this._listLogin[index].nombre;
            this.tPassword.Text = this._listLogin[index].password;
            User_Group ug = new User_Group(this._listLogin[index].UserGroup.id).findById();
            this.lListDestinoUserGroup.Items.Add(new ListBoxItem() { Content = ug.name});

            this.btnAdduser.Visibility = Visibility.Hidden;
            this.btnActualizarUser.Visibility = Visibility.Visible;
            this.workSpace.IsEnabled = true;

        }

        private void btnNewUser_Click(object sender, MouseButtonEventArgs e)
        {
            this.Clear();
            this.workSpace.IsEnabled = true;
            this.btnActualizarUser.Visibility = Visibility.Hidden;
            this.btnAdduser.Visibility = Visibility.Visible;
        }

        private void btnDeleteUser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.lListLogin.Items.Count > 0)//si ahi al menos un privilegio en la lista 2
                {
                    String userName = this.lListLogin.Items[this.lListLogin.SelectedIndex].ToString();
                    Console.WriteLine(userName);
                    if (userName != null)//si no selecciono ningun privilegio a eliminar
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Realmente desea eliminar el Usuario " + userName, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            Login l = new Login(new Login(userName).getIdByName().id);
                            if (l.deleteById() > 0)
                            {
                                this.Clear();
                                this.llenaListLogin();
                            }
                        }
                    }
                    else new Dialog("Seleccione un Usuario a eliminar.").ShowDialog();
                }
                else new Dialog("No hay ningun usuario en la lista.").ShowDialog();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                this.llenaListLogin();
                Console.WriteLine(ex.Message);
            }             
        }

        private void btnCancelAddUser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Clear();
            this.workSpace.IsEnabled = false;
        }

        private void btnAdduser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.tUserName.Text.Trim().Equals(""))
            {
                if (!this.tPassword.Text.Trim().Equals(""))
                {
                    if (this.lListDestinoUserGroup.Items.Count > 0) {
                        MessageBoxResult dialogResult = MessageBox.Show("Realmente desea Agregar estos privilegios a " + this.tUserName.Text, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            foreach (ListBoxItem UserGroups in this.lListDestinoUserGroup.Items)
                            {
                                User_Group ug = new User_Group(new User_Group(UserGroups.Content.ToString()).getIdByName());
                                if (new Login(this.tUserName.Text.Trim(),this.tPassword.Text.Trim(),ug).save()>0)//si no existe el privilegio
                                {
                                    this.llenaListLogin();
                                }
                            }
                        }                  
                    } else new Dialog("Seleccione un grupo de usuario a asignar.").ShowDialog();
                }
                else new Dialog("Rellene campo Password").ShowDialog();
            }
            else new Dialog("Rellene campo User Name").ShowDialog();
        }

        private void btnDeleteUserGroup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*try
            {

                    if (this.lListDestinoUserGroup.Items.Count > 0)//si ahi al menos un privilegio en la lista 2
                    {
                        String NameUserGroup = ((ListBoxItem)this.lListDestinoUserGroup.Items[this.lListDestinoUserGroup.SelectedIndex]).Content.ToString();
                        if (NameUserGroup != null)//si no selecciono ningun privilegio a eliminar
                        {
                            MessageBoxResult dialogResult = MessageBox.Show("Realmente desea eliminar " + NameUserGroup+" de "+this.tUserName, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                            if (dialogResult == MessageBoxResult.Yes)
                            {
                                User_Group_Privilegios p = new User_Group_Privilegios(new Privilegio(NameUserGroup).getIdByName());
                                if (new User_Group_Privilegios(p).deleteByIdPrivilegio() > 0)
                                {

                                }

                            }
                        }
                        else new Dialog("Seleccione un privilegio a eliminar.").ShowDialog();
                    }
                    else new Dialog("No hay ningun privilegio asignado previamente.").ShowDialog();
                
            }
            catch (ArgumentOutOfRangeException ex)
            {
                new Dialog("Seleccione un privilegio a elimnar").ShowDialog();
            }   */
        }

        

    }
}
