using Recursos_Humanos_wpf.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Recursos_Humanos_wpf.Interfaz.Administrativo
{
    /// <summary>
    /// Interaction logic for interfaz__User_UserGroup.xaml
    /// </summary>
    public partial class interfaz__User_UserGroup : UserControl
    {
        private  ListBoxItem _dragged;
        List<User_Group> userGroup;
        MenuItem root;
        MainWindow main;
        public interfaz__User_UserGroup(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
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
            userGroup = new User_Group().findAll();
            this.comboGrupos.Items.Clear();
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
            {
                this.List1.Items.Add(list.name);  
            }
        }
        private void LoadPrivilegio_UserGroup() {
            this.List2.Items.Clear();
            if (this.comboGrupos.SelectedItem != null)
            {
                foreach (Privilegio list in new Privilegio(new User_Group(this.comboGrupos.SelectedItem.ToString()).getIdByName()).findByidGroup())
                {
                    this.List2.Items.Add(new ListBoxItem() { Content = list.name });
                }
            }
        }
        //boton para agregar privilegio a userGroup
        private void btnAddPrivilegio_Click(object sender, MouseButtonEventArgs e)
        {
            if (this.comboGrupos.SelectedItem!=null)//si selecciono algun grupo de usuario
            {
                if (this.List2.Items.Count>0)//si ahi al menos un privilegio en la lista 2
                {
                    QuestionDialog pregunta = new QuestionDialog("Realmente desea Agregar estos privilegios a " + this.comboGrupos.SelectedItem, main);
                    pregunta.ShowDialog();
                    if (pregunta.DialogResult == true)
                    {
                        foreach (ListBoxItem privilegio in this.List2.Items)
                        {
                            Privilegio p = new Privilegio(new Privilegio(privilegio.Content.ToString()).getIdByName());
                            User_Group ug = new User_Group(this.comboGrupos.SelectedIndex + 1);
                            if (new User_Group_Privilegios(ug, p).ifExistPrivilegio() < 1)//si no existe el privilegio
                            {
                                new User_Group_Privilegios(ug, p).save();
                            }
                            Thread.Sleep(100);
                        }
                        LoadAllPrivilegios();
                        LoadPrivilegio_UserGroup();
                        llenaTreeView();
                    }                  
                }
                else new Dialog("Agrege a lo menos un privilegio ", main).ShowDialog();
            }
            else new Dialog("Seleccione un Grupo de usuario", main).ShowDialog();
        }

        private void btnDeletePrivilegio_Click(object sender, MouseButtonEventArgs e)
        {
            try {
                if (this.comboGrupos.SelectedItem != null)//si selecciono algun grupo de usuario
                {
                    if (this.List2.Items.Count > 0)//si ahi al menos un privilegio en la lista 2
                    {
                        String NamePrivilegio = ((ListBoxItem)this.List2.Items[this.List2.SelectedIndex]).Content.ToString();
                        if (NamePrivilegio != null)//si no selecciono ningun privilegio a eliminar
                        {
                            QuestionDialog pregunta = new QuestionDialog("Realmente desea eliminar este privilegio de " + this.comboGrupos.SelectedItem, main);
                            pregunta.ShowDialog();
                            if (pregunta.DialogResult == true)
                            {
                                Privilegio p = new Privilegio(new Privilegio(NamePrivilegio).getIdByName());
                                if (new User_Group_Privilegios(p).deleteByIdPrivilegio() > 0)
                                {
                                    Thread.Sleep(100);
                                    LoadPrivilegio_UserGroup();
                                    llenaTreeView();
                                }
                                Thread.Sleep(100);
                                LoadAllPrivilegios();
                                LoadPrivilegio_UserGroup();
                            }
                        }
                        else new Dialog("Seleccione un privilegio a eliminar.", main).ShowDialog();
                    }
                    else new Dialog("No hay ningun privilegio asignado previamente.", main).ShowDialog();
                }
                else new Dialog("Seleccione un Grupo de usuario", main).ShowDialog();
            }
            catch (ArgumentOutOfRangeException ex) {
                new Dialog("Seleccione un privilegio a elimnar", main).ShowDialog();
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
            List2.Items.Add(_dragged); 
            List1.Items.Remove(_dragged);
        }
        #endregion

        #region ESTILO VISUAL BOTONES
        public void styleVisualBtn(Label btn, Brush color, int borde)
        {
            btn.BorderBrush = color;
            btn.BorderThickness = new Thickness(borde, 0, 0, 0);
        }
        private void btnAddPrivilegio_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnAddPrivilegio, Brushes.Green, 5);
        }

        private void btnAddPrivilegio_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnAddPrivilegio, null, 0);
        }

        private void btnDeletePrivilegio_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnDeletePrivilegio, Brushes.Red, 5);
        }

        private void btnDeletePrivilegio_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnDeletePrivilegio, null, 0);
        }


        private void btnaddGroup_MouseEnter(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnaddGroup, Brushes.Green, 5);
        }

        private void btnaddGroup_MouseLeave(object sender, MouseEventArgs e)
        {
            styleVisualBtn(btnaddGroup, null, 0);
        }

        #endregion

        private void btnaddGroup_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new addGroup_User(main).ShowDialog();
            llenaTreeView();
            this.comboGrupos.Items.Clear();
            llenaComboUserGroup();
        }



    }
    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public String Title { get; set; }

        public System.Collections.ObjectModel.ObservableCollection<MenuItem> Items { get; set; }
    }
}
