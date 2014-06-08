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

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para QuestionDialog.xaml
    /// </summary>
    public partial class QuestionDialog : Window
    {
        public QuestionDialog(string pregunta)
        {
            InitializeComponent();
            lPregunta.Content = pregunta;
        }

        private void Yes(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }

        private void No(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }
    }
}
