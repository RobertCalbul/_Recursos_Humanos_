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
using System.Windows.Media.Effects;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        String msg { get; set; }
        MainWindow main;

        public Dialog(String msg,MainWindow main)
        {
            
            InitializeComponent();
            this.main = main;
            this.label2.Content = msg;
            if (msg.Length < 200)
            {
                this.MinHeight = 190;
                this.MaxHeight = 190;
                this.grid.MinHeight = 170;
                this.grid.MaxHeight = 170;
                this.label2.MinHeight = 130;
                this.label2.MaxHeight = 130;
                this.label1.Margin = new Thickness(10, 10, 10,10);
            }
            Dispatcher.BeginInvoke(new Action(() => { addEfecto(); })); 
        }

        private void acept(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => { QuitarEfecto(); }));
            this.Close();
        }
        public void addEfecto()
        {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = 2;
            myBlurEffect.KernelType = KernelType.Box;
            this.main.BitmapEffect = myBlurEffect;
        }

        public void QuitarEfecto()
        {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = 0;
            myBlurEffect.KernelType = KernelType.Box;
            this.main.BitmapEffect = myBlurEffect;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) { 
                Dispatcher.BeginInvoke(new Action(() => { QuitarEfecto(); }));
                this.Close();
            }
        }
    }
}
