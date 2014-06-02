using Recursos_Humanos_wpf.Interfaz.Administrativo;
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
        interfazDepto _Departamento;
        interfazUser _User;
        public Administrador(MainWindow main, interfazUserGeneral infoUser)
        {
            InitializeComponent();
            _infoUser = infoUser;
            _Departamento = new interfazDepto();
            _User = new interfazUser();
            this.main = main;
        }

        private void back_click(object sender, RoutedEventArgs e)
        {
            main.WorkSpace.Children.Clear();
            main.WorkSpace.Children.Add(_infoUser);
            main.WorkSpace.IsEnabled = false;
        }

        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = new TreeViewItem();
            item.Header = "Gestion Usuarios";
            item.ItemsSource = new string[] { "Privilegios", "User Group", "Usuarios" };

            TreeViewItem item2 = new TreeViewItem();
            item2.Header = "Departamento";
            //item2.ItemsSource = new string[] { "Nuevo departamento", "Asignar jefe" };

            // ... Get TreeView reference and add both items.
            var tree = sender as TreeView;
            tree.Items.Add(item);
            tree.Items.Add(item2);
            ExpandRecursively(tree,true);
        }
        #region MANEJO CON EL TREEVIEW
        private static void ExpandRecursively(ItemsControl itemsControl, bool expand)
        {
            ItemContainerGenerator itemContainerGenerator = itemsControl.ItemContainerGenerator;

            for (int i = itemsControl.Items.Count - 1; i >= 0; --i)
            {
                ItemsControl childControl = itemContainerGenerator.ContainerFromIndex(i) as ItemsControl;
                if (childControl != null)
                    ExpandRecursively(childControl, expand);
            }
            TreeViewItem treeViewItem = itemsControl as TreeViewItem;
            if (treeViewItem != null)
                treeViewItem.IsExpanded = expand;
        }
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            if (tree.SelectedItem is TreeViewItem)
            {
                var item = tree.SelectedItem as TreeViewItem;
               // this.Title = "Selected header padre: " + item.Header.ToString();
                if (item.Header.ToString().Equals("Departamento"))
                {
                    this.WorkSpace.Children.Clear();
                    this.WorkSpace.Children.Add(_Departamento);
                }
            }
            else if (tree.SelectedItem is string)
            {
                if (tree.SelectedItem.ToString().Equals("User Group"))
                {
                    this.WorkSpace.Children.Clear();
                    this.WorkSpace.Children.Add(new interfaz__User_UserGroup());
                    
                }
                if (tree.SelectedItem.ToString().Equals("Privilegios"))
                {
                    this.WorkSpace.Children.Clear();
                    this.WorkSpace.Children.Add(new interfaz__User_Privilegio());
                }
                if (tree.SelectedItem.ToString().Equals("Usuarios"))
                {
                    this.WorkSpace.Children.Clear();
                    this.WorkSpace.Children.Add(_User);
                }
                //this.Title = "Selected hijo: " + tree.SelectedItem.ToString();
            }
        }
        #endregion
    }
}
