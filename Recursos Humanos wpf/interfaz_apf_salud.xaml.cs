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
    /// Lógica de interacción para interfaz_apf_salud.xaml
    /// </summary>
    public partial class interfaz_apf_salud : Window
    {
        int flag;
        public interfaz_apf_salud(int flag)
        {
            this.flag = flag;
            InitializeComponent();
            this.Title = flag==1?"Ingreso nueva prevision":"Ingreso nueva AFP";

            
            
        }

        private void btnInsert_Click(object sender, MouseButtonEventArgs e)
        {   
            try{
                string name = this.tNameIntitucion.Text.Trim();
                double desc = double.Parse(this.tDescuento.Text.Trim());
         
                if (flag == 0)//AFP
                {
                    if (new Clases.Consultas().guardar_afp(name, desc) > 0)
                    {
                        MessageBox.Show("Registro AFP Guardado Con Exito!!", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el Registro AFP", "Fallo!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else if (flag == 1) 
                {
                    
                    if (new Clases.Consultas().guardar_salud(name, desc) > 0)
                    {
                     MessageBox.Show("Registro de Salud Guardado Con Exito!!", "Guardado", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo guardar el Registro de Salud", "Fallo!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }catch(Exception ex){
                Console.WriteLine("Interfaz_afp_salud.btnInsert_Click() " + ex.Message.ToString());
                MessageBox.Show("rellene todos los campos");
            }
        }

        private void btnCancel_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
