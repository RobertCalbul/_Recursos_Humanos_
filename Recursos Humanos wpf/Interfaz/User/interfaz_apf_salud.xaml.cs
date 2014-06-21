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
using System.Windows.Media.Animation;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para interfaz_apf_salud.xaml
    /// </summary>
    public partial class interfaz_apf_salud : Window
    {
        int flag;
        MainWindow main;
        public interfaz_apf_salud(int flag,MainWindow main)
        {
            this.flag = flag;
            InitializeComponent();
            this.main = main;
            this.Title = flag==1?"Ingreso nueva prevision":"Ingreso nueva AFP"; 
        }

        private void btnInsert_Click(object sender, MouseButtonEventArgs e)
        {   
            try{
                string name = this.tNameIntitucion.Text.Trim();
                double desc = double.Parse(this.tDescuento.Text.Trim());

                if (this.flag == 0)//AFP
                {
                    if (new Clases.Afp(name, desc).save() > 0) new Dialog("Registro AFP Guardado Con Exito!!.", main).Show();
                    else new Dialog("No se pudo guardar el Registro AFP.", main).Show();
                }
                else if (this.flag == 1) 
                {
                    if (new Clases.Salud(name, desc).save() > 0) new Dialog("Registro de Salud Guardado Con Exito!!.", main).Show();
                    else new Dialog("No se pudo guardar el Registro de Salud.", main).Show();
                }
            }catch(Exception ex){
                Console.WriteLine("Interfaz_afp_salud.btnInsert_Click() " + ex.Message.ToString());
                new Dialog("Rellene todos los campos.", main).Show(); 
            }
        }

        private void btnCancel_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

    }
}
