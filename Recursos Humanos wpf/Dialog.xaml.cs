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
using System.Windows.Shapes;
using System.Threading;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        String msg { get; set; }
        public Dialog(String msg)
        {
            
            InitializeComponent();
            label2.Content = msg;
            if (msg.Length < 200)
            {
                this.MinHeight = 190;
                this.MaxHeight = 190;
                this.grid.MinHeight = 170;
                this.grid.MaxHeight = 170;
                this.label2.MinHeight = 130;
                this.label2.MaxHeight = 130;
                this.label1.Margin = new Thickness(10, 10, 10,10);
                //116,0,104,12
            }

            
        }

        private void acept(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
