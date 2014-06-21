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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para QuestionDialog.xaml
    /// </summary>
    public partial class QuestionDialog : Window
    {
        MainWindow main;
        public QuestionDialog(string pregunta, MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            lPregunta.Content = pregunta;
            Dispatcher.BeginInvoke(new Action(() => { addEfecto(); })); 
        }

        private void Yes(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            Dispatcher.BeginInvoke(new Action(() => { QuitarEfecto(); }));
            this.Close();
        }

        private void No(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            Dispatcher.BeginInvoke(new Action(() => { QuitarEfecto(); }));
            this.Close();
        }


        public void addEfecto() {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = 2;
            myBlurEffect.KernelType = KernelType.Box;
            main.BitmapEffect = myBlurEffect;
        }

        public void QuitarEfecto()
        {
            BlurBitmapEffect myBlurEffect = new BlurBitmapEffect();
            myBlurEffect.Radius = 0;
            myBlurEffect.KernelType = KernelType.Box;
            main.BitmapEffect = myBlurEffect;
        }
    }
}
