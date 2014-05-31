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
                Console.WriteLine(listaUser[g].name);
                grupo = new MenuItem() { Title = listaUser[g].name };
                listaPrivilegio = new Privilegio(listaUser[g].id).findByidGroup();
                for (int p = 0; p < listaPrivilegio.Count; p++)
                {
                    Console.WriteLine(listaPrivilegio[p].name);
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
            List<Privilegio> Privilegio = new Privilegio().findAll();
            this.List1.Items.Clear();
            this.List2.Items.Clear();
            foreach (Privilegio list in Privilegio)
                this.List1.Items.Add(list.name);
        }
/*Drag and Drop*/
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
        /*Drag and Drop*/
        //boton para agregar privilegio a userGroup
        private void btnAddPrivilegio_Click(object sender, MouseButtonEventArgs e)
        {

            foreach (ListBoxItem privilegio in this.List2.Items)
            {
                Console.WriteLine(privilegio.Content + " " + privilegio.Content.GetType());
                Privilegio p = new Privilegio(new Privilegio(privilegio.Content.ToString()).getIdByName());
                User_Group ug = new User_Group(this.comboGrupos.SelectedIndex+1);
                new User_Group_Privilegios(ug,p).save();
            }
            llenaTreeView();
        }
    }
}
