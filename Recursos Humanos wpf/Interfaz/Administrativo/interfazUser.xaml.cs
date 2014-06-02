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
            llenaTreeView();
        }

        public void llenaTreeView()
        {
            this.trvMenu.Items.Clear();
            root = new MenuItem() { Title = "User Group" };
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
        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;
            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is MenuItem)
            {
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as MenuItem;
                // this.Title = "Selected header padre: " + item.Header.ToString();
                Console.WriteLine("Selected header padre: " + item.Items.ToString());
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                //this.Title = "Selected hijo: " + tree.SelectedItem.ToString();
                Console.WriteLine("Selected hijo: " + tree.SelectedItem.ToString());
            }
        }   
    }

    public class MenuItem
    {
        public MenuItem()
        {
            this.Items = new ObservableCollection<MenuItem>();
        }

        public String Title { get; set; }

        public ObservableCollection<MenuItem> Items { get; set; }
    }
}
