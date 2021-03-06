﻿using Recursos_Humanos_wpf.Clases;
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
using Recursos_Humanos_wpf;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para Dialog.xaml
    /// </summary>
    public partial class addGroup_User : Window
    {
        MainWindow main;
        public addGroup_User(MainWindow main)
        {            
            InitializeComponent();
            this.main = main;
            this.bAddGroup.Margin = new Thickness(10, 10, 10,10);
            this.bCancelGroup.Margin = new Thickness(10, 10, 10, 10);
        }

        private void acept(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void insertar(object sender, MouseButtonEventArgs e)
        {
            String nombre = this.tAddGroup.Text;
            User_Group ug = new User_Group(nombre);
            if (ug.ifExist() < 1) { 
                if(ug.save() != 0){
                    this.tAddGroup.Text = "";
                    new Dialog("Grupo agregado correctamente", main).ShowDialog();
                }
                else
                {
                    new Dialog("Error en agregar grupo", main).ShowDialog();
                }
            }
            else
            {
                this.tAddGroup.Text = "";
                new Dialog("El Grupo ya existe, ingrese otro nombre.", main).ShowDialog();
            }
        }
        
    }
}
