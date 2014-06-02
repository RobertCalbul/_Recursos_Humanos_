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
    /// Interaction logic for interfaz__User_UserGroup.xaml
    /// </summary>
    public partial class interfaz__User_UserGroup : UserControl
    {
        public  ListBoxItem _dragged;
        List<User_Group> userGroup;
        MenuItem root;
        public interfaz__User_UserGroup()
        {
            InitializeComponent();
            userGroup = new User_Group().findAll();
            

            llenaComboUserGroup();
            llenaTreeView();
            LoadAllPrivilegios();
        }

        public void llenaTreeView()
        {
            root = new MenuItem() { Title = "User Group" }; 
            this.trvMenu.Items.Clear(); this.root.Items.Clear();
            List<User_Group> listaUser = new User_Group().findAll();
            List<Privilegio> listaPrivilegio;
            MenuItem grupo;
            for (int g = 0; g < listaUser.Count; g++)
            {
                grupo = new MenuItem() { Title = listaUser[g].name };
                listaPrivilegio = new Privilegio(listaUser[g].id).findByidGroup();
                for (int p = 0; p < listaPrivilegio.Count; p++)
                {
                    grupo.Items.Add(new MenuItem() { Title = listaPrivilegio[p].name });
                }
                root.Items.Add(grupo);
            }
            trvMenu.Items.Add(root);
        }
        public void llenaComboUserGroup()
        {
            foreach (User_Group list in userGroup)
                this.comboGrupos.Items.Add(list.name);
        }

        private void comboGrupos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                LoadPrivilegio_UserGroup();
        }
        private void LoadAllPrivilegios()
        {
            this.List1.Items.Clear();
            foreach (Privilegio list in new Privilegio().findAll())
                this.List1.Items.Add(list.name);
        }
        private void LoadPrivilegio_UserGroup() {
            this.List2.Items.Clear();
            foreach (Privilegio list in new Privilegio(new User_Group(this.comboGrupos.SelectedItem.ToString()).getIdByName()).findByidGroup())
                this.List2.Items.Add(new ListBoxItem() { Content = list.name });
        }
        //boton para agregar privilegio a userGroup
        private void btnAddPrivilegio_Click(object sender, MouseButtonEventArgs e)
        {
            if (this.comboGrupos.SelectedItem!=null)//si selecciono algun grupo de usuario
            {
                if (this.List2.Items.Count>0)//si ahi al menos un privilegio en la lista 2
                {
                    foreach (ListBoxItem privilegio in this.List2.Items)
                    {
                        Privilegio p = new Privilegio(new Privilegio(privilegio.Content.ToString()).getIdByName());
                        User_Group ug = new User_Group(this.comboGrupos.SelectedIndex + 1);
                        new User_Group_Privilegios(ug, p).save();
                    }
                    llenaTreeView();
                    LoadAllPrivilegios();
                    LoadPrivilegio_UserGroup();
                }
                else new Dialog("Agrege a lo menos un privilegio ").Show();
            }
            else new Dialog("Seleccione un Grupo de usuario").Show();
        }

        private void btnDeletePrivilegio_Click(object sender, MouseButtonEventArgs e)
        {
            try {
                if (this.comboGrupos.SelectedItem != null)//si selecciono algun grupo de usuario
                {
                    if (this.List2.Items.Count > 0)//si ahi al menos un privilegio en la lista 2
                    {
                        MessageBoxResult dialogResult = MessageBox.Show("Realmente desea eliminar este privilegio de " + this.comboGrupos.SelectedItem, "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            Console.WriteLine(((ListBoxItem)this.List2.Items[this.List2.SelectedIndex]).Content);
                            String NamePrivilegio = ((ListBoxItem)this.List2.Items[this.List2.SelectedIndex]).Content.ToString();
                            Privilegio p = new Privilegio(new Privilegio(NamePrivilegio).getIdByName());
                            if (new User_Group_Privilegios(p).deleteByIdPrivilegio() > 0)
                            {
                                Console.WriteLine("BORRADO PRIVILEGIO EXITOSO");
                                LoadPrivilegio_UserGroup();
                                llenaTreeView();
                            }
                            else { Console.WriteLine("BORRADO PRIVILEGIO ERRONEO"); }
                            LoadAllPrivilegios();
                            LoadPrivilegio_UserGroup();
                        }
                    }
                    else new Dialog("Seleccione un privilegio ").Show();
                }
                else new Dialog("Seleccione un Grupo de usuario").Show();
            }
            catch (ArgumentOutOfRangeException ex) {
                new Dialog("Seleccione un privilegio a elimnar").Show();
            }            
        }

        #region Drag and Drop ListBox
        private void List1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_dragged != null)
                return;

            UIElement element = List1.InputHitTest(e.GetPosition(List1)) as UIElement;

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
        private void List2_DragEnter(object sender, DragEventArgs e)
        {
            if (_dragged == null || e.Data.GetDataPresent(DataFormats.Text, true) == false)
                e.Effects = DragDropEffects.None;
            else
                e.Effects = DragDropEffects.All;
        }
        private void List2_Drop(object sender, DragEventArgs e)
        {
            List1.Items.Remove(_dragged);
            List2.Items.Add(_dragged);
        }
        #endregion
    }
}
