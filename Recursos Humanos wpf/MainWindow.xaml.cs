﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Recursos_Humanos_wpf.Clases;
using System.Text.RegularExpressions;

namespace Recursos_Humanos_wpf
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Boolean Rutok = false;
        int flagCalendar = -1;
        List<string> listAutocomplet = null;
        List<Afp> listAfp = null;
        List<Salud> listSalud = null;
        List<Departamento> listDpto = null;
        List<Cargo> listCargo = null;
        List<TipoContrato> listTipoContrato = null;
        public MainWindow()
        {
            InitializeComponent();
            listAutocomplet = new Clases.Personal().findAll(0);
            this.tabControl1.SelectedIndex = 0;
            grid5.IsEnabled = false;
            rbRut.IsChecked = true;

        }
        /*>>>>INICIO OPERACIONES CRUD EMPLEADOS<<<<<*/
        //BUSCA DATOS EMPLEADOS POR FILTRO
        
        private void Search(){
          try
            {
                String busqueda = cBusqueda.Text.Trim().Equals("") ? tRut.Text.Trim() : cBusqueda.Text.Trim();
                if (!busqueda.Equals(""))
                {
                    //limpiarTexbox();
                    if (rbRut.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "rut");
                    else if (rbName.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "nombre");
                    else if (rbSurname.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "apellido");
                    else if (rbPhone.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "telefono");
                    else if (rbAdress.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "direccion");
                    else if (rbEmail.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "email");
                }else
                {
                    cBusqueda.Focus();//doy el foco al cuadro de busqueda
                    new Dialog("Ingrese un parametro de búsqueda.").Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnBuscar_click() " + ex.Message.ToString());
            }
        }

        private void btnBuscar_click(object sender, MouseButtonEventArgs e)
        {
            Search();
            calendar1.Visibility = Visibility.Hidden;
        }
        //CARGA DATOS PERSONALES BASICOS
        private void cargarDatosPersonal(String value, String paramSearch)
        {
            if (!value.Equals(""))
            {
                grid5.IsEnabled = true;
                //btnDateNacimiento.Visibility = Visibility.Hidden;
                object[] arreglo = new Clases.Personal().findBy(value, paramSearch);
                if (arreglo != null)
                {
                    lName.Content = arreglo[1].ToString() + " " + arreglo[2].ToString(); 
                    BitmapImage imagenes = new BitmapImage();//Byte[] datas = (byte[])arreglo[0];
                    imagenes.BeginInit();
                    MemoryStream stri = new MemoryStream((byte[])arreglo[0], 0, ((byte[])arreglo[0]).Length, false, false);
                    imagenes.StreamSource = stri;
                    imagenes.EndInit();
                    iPerfil.Source = imagenes;
                    tName.Text = arreglo[1].ToString();
                    tSurname.Text = arreglo[2].ToString();
                    tRut.Text = arreglo[3].ToString();
                    tDateNaci.Text = arreglo[4].ToString();
                    tAdress.Text = arreglo[5].ToString();
                    tComuna.Text = arreglo[6].ToString();
                    cSalud.Items.Clear();
                    cDepto.Items.Clear();
                    cAfp.Items.Clear();
                    int i = 0;
                    foreach (Salud salud in new Salud().findAll())
                    {
                        cSalud.Items.Add(salud.name_salud);
                        if (salud.name_salud.Equals(arreglo[7].ToString())) cSalud.SelectedIndex = i;
                        i++;
                    }
                    i = 0;
                    foreach (Departamento dpto in new Departamento().findAll())
                    {
                        cDepto.Items.Add(dpto.name);
                        if (dpto.name.Equals(arreglo[8].ToString())) cDepto.SelectedIndex = i;
                        i++;
                    }
                    i = 0;
                    foreach (Afp afp in new Afp().findAll()) 
                    {
                        cAfp.Items.Add(afp.nombre_afp);
                        if (afp.nombre_afp.Equals(arreglo[9].ToString()))cAfp.SelectedIndex = i;
                        i++;
                    }
                    
                    tYear.Text = arreglo[10].ToString();
                    tRegion.Text = arreglo[11].ToString();
                    tPhone.Text = arreglo[12].ToString();
                    tEmail.Text = arreglo[13].ToString();
                    tCtaBancaria.Text = arreglo[14].ToString();
                    tNacionalidad.Text = arreglo[15].ToString();
                } loadDataContract(tRut.Text.Trim());
            }
        }
        //INGRESA NUEVO USUARIO
        private void iAddUser_Click(object sender, MouseButtonEventArgs e)
        {

            grid5.IsEnabled = true;
            limpiarTexbox();
            this.tName.Focus();
            this.cBusqueda.Text = "";
            this.cBusqueda.IsEnabled = false;
            this.tRut.IsEnabled = true;
            this.iPerfil.IsEnabled = true;
            this.btnAddUser.IsEnabled = true;
            this.image1.IsEnabled = false;
            this.btnAddAfp.Visibility = Visibility.Visible;
            this.btnUpdateReg.Visibility = Visibility.Hidden;
            this.btnDeleteReg.Visibility = Visibility.Hidden;
            this.btnAddSalud.Visibility = Visibility.Visible;
            this.btnAddUser.Visibility = Visibility.Visible;
            this.btnCancelAdd.Visibility = Visibility.Visible;
            this.btnDateNacimiento.Visibility = Visibility.Visible;
            this.tabControl1.SelectedIndex = 0;
            loadDataContract("");
        }
        private void btnAddUser_Click(object sender, MouseButtonEventArgs e)
        {   tRut.IsEnabled = true;
            iAddUser.IsEnabled = true;
            // if  (validacionAddUser())
            //{
                MessageBoxResult dialogResult = MessageBox.Show("Desea agregar a esta persona?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes && validacionAddUser())
                {
                    byte[] foto = File.ReadAllBytes(path.Content.ToString());
                    listDpto = new Departamento().findAll();
                    listAfp = new Afp().findAll();
                    listSalud = new Salud().findAll();
                    Personal per = new Personal(tRut.Text.Trim(), tName.Text.Trim(), tSurname.Text.Trim(),
                                                int.Parse(tYear.Text.Trim()), foto, tPhone.Text.Trim(),tAdress.Text.Trim(),
                                                tEmail.Text.Trim(), tCtaBancaria.Text.Trim(),tNacionalidad.Text.Trim(),
                                                tDateNaci.Text.Trim(),tComuna.Text.Trim(),tRegion.Text.Trim(),
                                                listAfp[cAfp.SelectedIndex].id, listSalud[cSalud.SelectedIndex].id
                                                );
                    
                    if (per.Save() > 0)
                    {
                        Personal_Departamento pd = new Personal_Departamento(new Personal(tRut.Text.Trim()).get_idPersonal(), listDpto[cDepto.SelectedIndex].id);
                        if (pd.save() > 0)
                        {
                            //Actualizo lista del autocomplete
                            listAutocomplet = new Clases.Personal().findAll(0);
                            //new Dialog("Personal guardado con exito.").Show();
                            //MessageBox.Show("Personal guardado con exito", "Registro agregado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                            this.cBusqueda.IsEnabled = true;
                            this.iPerfil.IsEnabled = false;
                            this.btnAddUser.Visibility = Visibility.Hidden;
                            this.btnAddAfp.Visibility = Visibility.Hidden;
                            this.btnAddSalud.Visibility = Visibility.Hidden;
                            this.btnCancelAdd.Visibility = Visibility.Hidden;
                            this.btnUpdateReg.Visibility = Visibility.Visible;
                            this.btnDeleteReg.Visibility = Visibility.Visible;

                            //PREGUNTO SI ESTA SEGURO
                            MessageBoxResult pregunta = MessageBox.Show("¿Desea contratar a este nuevo personal?", "Pregunta:", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                            if (pregunta == MessageBoxResult.Yes)
                            {
                                this.tabControl1.SelectedIndex = 1;
                                cargarDatosPersonal(this.tRut.Text, "rut");
                            }
                            else cargarDatosPersonal(this.tRut.Text, "rut");
                        }
                    }
                    else {
                        new Dialog("Personal no pudo ser ingresado").Show();
                    }
                }
            //}
        }
        //ACTUALIZA DATOS EMPLEADO
        private void btnUpdateReg_Click(object sender, MouseButtonEventArgs e)
        {
            listAfp = new Afp().findAll();
            listSalud = new Salud().findAll();
            listDpto = new Departamento().findAll();
            if (validaFecha(tDateNaci.Text.Trim()))
            {
                Personal per = new Personal(tRut.Text.Trim(), tName.Text.Trim(), tSurname.Text.Trim(),
                                                    int.Parse(tYear.Text.Trim()), tPhone.Text.Trim(), tAdress.Text.Trim(),
                                                    tEmail.Text.Trim(), tCtaBancaria.Text.Trim(), tNacionalidad.Text.Trim(),
                                                    tDateNaci.Text.Trim(), tComuna.Text.Trim(), tRegion.Text.Trim(),
                                                    listAfp[cAfp.SelectedIndex].id, listSalud[cSalud.SelectedIndex].id
                                                    );
                if (per.Update() > 0)
                {
                    Search();
                    new Dialog("Datos actualizados correctamente.").Show();
                }
            }
            else new Dialog("Ingrese formato fecha nacimiento 'YYYY-MM-DD'").Show();
        }
        //CANCELA INGRESO EMPLEADO
        private void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {
            Label[] labels = { this.btnAddAfp, this.btnAddSalud, this.btnAddUser, this.btnCancelAdd };
            foreach (Label x in labels) x.Visibility = Visibility.Hidden;
            this.grid5.IsEnabled = false;
            this.cBusqueda.IsEnabled = true;
            this.btnUpdateReg.Visibility = Visibility.Visible;
            this.btnDeleteReg.Visibility = Visibility.Visible;
            this.image1.IsEnabled = true;
            calendar2.Visibility = Visibility.Hidden;
            limpiarTexbox();
        }
        //ELIMINA UN EMPLEADO
        private void btnDeleteReg_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.tabControl1.SelectedIndex = 0;
                String rut_per = tRut.Text.Trim();
                MessageBoxResult dialogResult = MessageBox.Show("Desea borrar el personal con rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    if (new Clases.Personal(rut_per).DeleteByRrut() > 0)
                    {
                        limpiarTexbox();
                        cBusqueda.Text = "";
                        cBusqueda.Focus();
                        grid5.IsEnabled = false;
                        new Dialog("El empleado con rut " + rut_per + " fue eliminado satisfactoriamente.").Show();
                    }
                    else {
                         new Dialog("Ocurrio algo inesperado al eliminar al empleado con rut " + rut_per+".").Show();
                    } 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en eliminar personal"+ex.Message );
            }
        }
/*>>>>FIN OPERACIONES CRUD EMPLEADOS<<<<<*/

/*>>>>INICIO CRUD CONTRATOS<<<<<*/
        //ELIMINA UN CONTRATO ASOCIADO A UN EMPLEADO
        private void btnEndContract_Click(object sender, MouseButtonEventArgs e)
        {
            string rut_per = tRut.Text.Trim();
            MessageBoxResult dialogResult = MessageBox.Show("Desea borrar el contrato asociado al rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (dialogResult == MessageBoxResult.Yes)
            {
                if (new Clases.Contratos().DeleteByRut(new Personal(rut_per)) > 0)
                {
                    limpiarTexbox();
                    loadDataContract(rut_per);
                    cargarDatosPersonal(rut_per, "rut");
                    new Dialog("Se cancelo el contrato a empleado con rut " + rut_per + ".").Show(); //MessageBox.Show("Se elimino el contrato con exito"); 
                }
            }
        }
        //CARGA CONTRATOS DE UN EMPLEADO ESPECIFICO
        private void loadDataContract(string value)
        {
            ClearContract();
            tabItem2.IsEnabled = !string.IsNullOrEmpty(tRut.Text.Trim()) == true ? true : false;
            string sql ="SELECT e.fecha_inicio,e.fecha_termino,e.estado,"
             +" (SELECT c.tipo AS tipo_contrato"
             +" FROM personal_contrato AS pc"
			 +" INNER JOIN personal AS p ON(p.id_personal = pc.id_personal)"
             +" INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             +" INNER JOIN tipo_contrato AS c ON(e.tipo_contrato_id_tipo_contrato=c.id_tipo_contrato)"
             +" WHERE p.rut ='" + value + "') AS tipo_contrato,"
             +" IFNULL((SELECT f.cargo AS nombre_cargo"
             +" FROM personal_contrato AS pc"
			 +" INNER JOIN personal AS p ON(p.id_personal = pc.id_personal)"
             +" INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             +" INNER JOIN cargo AS f ON(f.id_cargo = e.cargo_id_cargo)"
             +" WHERE p.rut = '" + value + "'),'1')AS cargo" 
             +" FROM personal AS p"
             +" INNER JOIN personal_contrato AS pc ON(p.id_personal = pc.id_personal)"
             +" INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             +" INNER JOIN cargo AS f ON(f.id_cargo = e.cargo_id_cargo)"
             +" WHERE p.nombre = '" + value + "' OR p.apellido ='" + value + "' OR p.rut = '" + value + "'";
            string interfaces = "";
            foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
            {
                lPuesto.Content = dtRow["cargo"].ToString();
                interfaces = dtRow["cargo"].ToString();
                tDateInit.Text = DateFormat(dtRow["fecha_inicio"].ToString());
                tDateEnd.Text = DateFormat(dtRow["fecha_termino"].ToString());
                tStat.Text = dtRow["estado"].ToString();
                cTypeContract.Items.Add(dtRow["tipo_contrato"].ToString());
                cTypeContract.SelectedItem = dtRow["tipo_contrato"].ToString();
                cCargo.Items.Add(dtRow["cargo"].ToString());
                cCargo.SelectedItem = dtRow["cargo"].ToString();
            }
            Label [] labelVisible = { label15, label16, label17, label18, label19, label20 };
            Label[] btnVisible = { btnShowContract, btnInsertNewContract, btnCancelNewContract, btnDateEndCalendar, btnDateInitCalendar };
            //cambia color en caso de que no existe contrato PD: ROBERT QLO xd
            lDescription.Background = interfaces == "1" || interfaces == "" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd4337")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4185f4"));
            //cambia el texto del titulo en caso de que no exista contrato
            lDescription.Content = interfaces == "1" || interfaces == "" ? "Usuario sin contrato" : cCargo.Text;

            tDateInit.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            tDateEnd.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            tStat.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            cTypeContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            cCargo.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            btnEndContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            btnNewContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Visible : Visibility.Hidden;

            foreach(Label x in labelVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            foreach (Label x in btnVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Hidden;
        }
        //CREA INTERFAZ PARA AGREGAR CONTRATO
        private void btnNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            lDescription.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#129d5a"));
            lDescription.Content = "Agregar Nuevo contrato";
            Label[] labels = { label15, label16, label17, label18, label19, label20, btnShowContract, btnInsertNewContract, btnCancelNewContract };
            foreach (Label x in labels) x.Visibility = Visibility.Visible; 
            tDateInit.Visibility = Visibility.Visible;
            tDateEnd.Visibility = Visibility.Visible;
            tStat.Visibility = Visibility.Visible;
            cTypeContract.Visibility = Visibility.Visible;
            cCargo.Visibility = Visibility.Visible;
            btnEndContract.Visibility = Visibility.Hidden;
            btnNewContract.Visibility = Visibility.Hidden;
            btnDateInitCalendar.Visibility = Visibility.Visible;
            btnDateEndCalendar.Visibility = Visibility.Visible;
        }
        //CANCELA INGRESO CONTRATO (RECARGA INTERFAZ CONTRATO)
        private void btnCancelNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            loadDataContract(this.tRut.Text.Trim());
        }
        //MUESTRA PREVISUALIZACION CONTRATO
        private void btnShowContract_Click(object sender, MouseButtonEventArgs e)
        {
            bool crearcarp = new Clases.PDF().CrearCarpetaXml("contratos");
            
            if (crearcarp)
            {
                try
                {
                    listCargo = new Cargo().findAll();
                    listTipoContrato = new TipoContrato().findAll();
                    Clases.Contratos contrato = new Clases.Contratos(tRut.Text, tDateInit.Text, tName.Text + " " + tSurname.Text, tAdress.Text,
                                                            listCargo[cCargo.SelectedIndex].cargo, cDepto.Text.Trim(),
                                                            listTipoContrato[cTypeContract.SelectedIndex].tipo, 25000, tDateEnd.Text,
                                                            cAfp.Text.Trim(), cSalud.Text.Trim()
                        );

                    
                    new Clases.PDF().CrearArchivoXML("contratos/contract.xml",
                    contrato.rut, contrato.fInicio, contrato.nombre_completo, contrato.direccion, contrato.Cargo, contrato.depto, contrato.tContrato,
                    contrato.SueldoBase, contrato.fTermino, contrato.afp, contrato.salud);
                    Document document = new Document();

                    List<String> resul = new Clases.PDF().leer("administrativo");

                    PdfWriter.GetInstance(document, new FileStream("contrato.pdf", FileMode.OpenOrCreate));
                    document.Open();
                    Chunk chunk = new Chunk(resul[0], FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.UNDERLINE));

                    document.Add(new iTextSharp.text.Paragraph(chunk));
                    for (int i = 1; i < resul.Count; i++)
                    {
                        document.Add(new iTextSharp.text.Paragraph(resul[i]));
                    }
                    document.Close();

                    new Dialog("Contrato generado con exito.");
                    System.Diagnostics.Process.Start("contrato.pdf");
                }
                catch (Exception ex)
                {
                    new Dialog("Ocurrio un error al generar el contrato.").Show();
                    Console.WriteLine("MainWindow.btnShowContract_Click() " + ex.Message.ToString());
                }
            }
        }
        //INGRESA UN CONTRATO A UN EMPLEADO
        private void btnInsertNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                String rut_per = tRut.Text.Trim();
                MessageBoxResult dialogResult = MessageBox.Show("Desea asignar el contrato al rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes && validacionAddContract())
                {

                        listCargo = new Cargo().findAll();
                        listTipoContrato = new TipoContrato().findAll();
                        Clases.Contratos contrato = new Contratos(rut_per, tDateInit.Text, tDateEnd.Text, tStat.Text.ToUpper(),
                                                    250000, listTipoContrato[cTypeContract.SelectedIndex].id.ToString(), listCargo[cCargo.SelectedIndex].id.ToString());
                        if (contrato.save() > 0)
                        {
                            loadDataContract(rut_per);
                            new Dialog("Se ingreso contrato a empleado con rut " + rut_per + ".").Show(); //MessageBox.Show("Contrato ingresado exitosamente.");
                        }
                        else
                        {
                            new Dialog("Ocurrio un error al ingresar contrato a persona con rut " + rut_per + ".").Show();
                        }
                

                }
            }catch (Exception ex)
            {
                Trace.WriteLine("MainWindow.btnInsertNewContract_Click() " + ex.Message.ToString());
            }

        }
 /*>>>>FIN CRUD CONTRATOS<<<<<*/

 /*>>>>CALENDARIOS >>>>>>*/

        //ABRE EL CALENDARIO
        private void btnDateInitCalendar_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 0;
            calendar1.Visibility = Visibility.Visible;
            calendar1.Margin = btnDateInitCalendar.Margin;// new Thickness(0, 130, 0, 0);

        }
        //SETEA LA FECHA DE INICIO O TERMINO DE COTRATO
        private void selectedDateChanges_Clikc(object sender, SelectionChangedEventArgs e)
        {
            tDateInit.Text = flagCalendar == 0 ? calendar1.SelectedDate.Value.ToString("yyyy-MM-dd") : tDateInit.Text;
            tDateEnd.Text = flagCalendar == 1 ? calendar1.SelectedDate.Value.ToString("yyyy-MM-dd") : tDateEnd.Text;
            calendar1.Visibility = Visibility.Hidden;
            flagCalendar = -1;
        }
        //ABRE EL CALENDARIO
        private void btnDateEndCalendar_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 1;
            calendar1.Visibility = Visibility.Visible;
            calendar1.Margin = btnDateEndCalendar.Margin;// new Thickness(0, 151, 0, 0);
        }
        private void btnDateNacimiento_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 2;
            calendar2.Visibility = Visibility.Visible;
            calendar2.DisplayMode = CalendarMode.Decade;
            calendar2.Margin = btnDateNacimiento.Margin;
        }
        private void calendar2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            tDateNaci.Text = flagCalendar == 2 ? calendar2.SelectedDate.Value.ToString("yyyy-MM-dd") : tDateNaci.Text;
            tYear.Text = (2014 - int.Parse(tDateNaci.Text.Substring(0, 4))).ToString();
            calendar2.Visibility = Visibility.Hidden;
        }
 /*FIN CALENDARIO<<<<<*/
/*>>>>AUTOCOMPLETE<<<<<*/
        private void cBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typed = cBusqueda.Text.Trim();
            List<string> autoList = new List<string>();
            autoList.Clear();

            foreach (string item in listAutocomplet)
            {
                if (!string.IsNullOrEmpty(cBusqueda.Text.Trim()))
                {
                    if (item.StartsWith(typed))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                lAutoComplete.ItemsSource = autoList;
                lAutoComplete.Visibility = Visibility.Visible;
            }
            else if (cBusqueda.Text.Equals(""))
            {
                lAutoComplete.Visibility = Visibility.Collapsed;
                lAutoComplete.ItemsSource = null;
            }
            else
            {
                lAutoComplete.Visibility = Visibility.Collapsed;
                lAutoComplete.ItemsSource = null;
            }
        }
        private void lAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lAutoComplete.ItemsSource != null)
            {
                lAutoComplete.Visibility = Visibility.Collapsed;
                cBusqueda.TextChanged -= new TextChangedEventHandler(cBusqueda_TextChanged);
                if (lAutoComplete.SelectedIndex != -1)
                {
                    cBusqueda.Text = lAutoComplete.SelectedItem.ToString();
                }
                cBusqueda.TextChanged += new TextChangedEventHandler(cBusqueda_TextChanged);
                Search();
            }
        }
/*>>>>FIN SOLO AUTOCOMPLETE<<<<<*/

        //CARGA IMAGEN A CONTENDOR IMAGEN PERFIL
        private void iPerfil_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                //ofd.Filter = "Imagenes jpg(*.jpg)|*.jpg";
                if (ofd.ShowDialog() == true)
                {
                    this.path.Content = ofd.FileName;
                    using (Stream stream = ofd.OpenFile())
                    {   
                        BitmapDecoder bitdecoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        this.iPerfil.Source = bitdecoder.Frames[0];
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write("error: " + ex.Message);
                new Dialog("Seleccione una imagen mas pequeña.").Show();// MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido" + ex.Message);
            }
        }
        //CARGA DATOS DE AFP EN COMBOBOX
        private void cAfp_Click(object sender, MouseButtonEventArgs e)
        {
            cAfp.Items.Clear();
            foreach (Afp afp in new Afp().findAll()) cAfp.Items.Add(afp.nombre_afp);
        }
        //CARGA DATOS DE SALUD EN COMBOBOX
        private void cSalud_Click(object sender, MouseButtonEventArgs e)
        {
            cSalud.Items.Clear();
            foreach (Salud salud in new Salud().findAll()) cSalud.Items.Add(salud.name_salud);
        }
        //CARGA DATOS DE DEPARTAMENTO EN COMBOBOX
        private void cDepto_Click(object sender, MouseButtonEventArgs e)
        {
            cDepto.Items.Clear();
            foreach (Departamento dpto in new Departamento().findAll()) cDepto.Items.Add(dpto.name);          
        }
        //CARGA LOS TIPOS DE CONTRATOS
        private void cTypeContract_Click(object sender, MouseButtonEventArgs e)
        {
            cTypeContract.Items.Clear();
            foreach (TipoContrato tContrato in new TipoContrato().findAll()) cTypeContract.Items.Add(tContrato.tipo);
        }
        //CARGA LOS DIFERENTE CARGOS
        private void cCargo_Click(object sender, MouseButtonEventArgs e)
        {
            cCargo.Items.Clear();
            foreach (Cargo cargo in new Cargo().findAll()) cCargo.Items.Add(cargo.cargo);
        }
        //FILTRO SEGUN INFORMACION PERSONAL
        private void RadioButtonSearch(object sender, RoutedEventArgs e)
        {
            int parametroSearch = 0;
            if (rbRut.IsChecked == true) parametroSearch = 0;
            if (rbName.IsChecked == true) parametroSearch = 1;
            if (rbSurname.IsChecked == true) parametroSearch = 2;
            if (rbPhone.IsChecked == true) parametroSearch = 3;
            if (rbAdress.IsChecked == true) parametroSearch = 4;
            if (rbEmail.IsChecked == true) parametroSearch = 5;
            listAutocomplet = new Clases.Personal().findAll(parametroSearch);
        }

 /*>>>>VALIDACIONES<<<<<*/
        public void ClearContract() {
            this.tDateInit.Text = "";
            this.tDateEnd.Text = "";
            this.tStat.Text = "";
            this.cTypeContract.Items.Clear();
            this.cCargo.Items.Clear();
        }
        public Boolean validaFecha(string fecha)
        {
            string expresion = "^\\d{4}-\\d{2}-\\d{2}";
            return Regex.IsMatch(fecha, expresion) ? Regex.Replace(fecha, expresion, String.Empty).Length == 0 ? true : false : false;
        }
        public void limpiarTexbox()
        {
            lPuesto.Content = "";
            lName.Content = "";
            TextBox[] campos = { tRut, tName, tSurname, tYear, tPhone, tAdress, tEmail, tCtaBancaria, tDateEnd, tDateInit, tStat, tDateNaci,tRegion, tNacionalidad,tComuna};
            ComboBox[] combos = { cAfp, cDepto, cSalud, cTypeContract, cCargo };
            foreach (TextBox x in campos) x.Text = "";
            foreach (ComboBox x in combos) x.Items.Clear();
            this.iPerfil.Source = new BitmapImage(new Uri("pack://application:,,,/Images/icono.png"));
        }
        public void validaNumeros(TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 48 && ascci <= 57? false : true;
        }
        public String DateFormat(String value){
            return value.Substring(6, 4) + "-"+value.Substring(3, 2) + "-"+value.Substring(0, 2);
        }
        public void validaString(TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 ? false : true;
        }
        public string valida_Rut(string value)
        {
            string rut = value.Replace(" ", "").Replace(".", "").Replace("-", "").ToUpper();
            int[] multiplos = { 2, 3, 4, 5, 6, 7, 2, 3 };
            int suma = 0;
            string digito = "";
            int lengthRut = rut.Length - 1;
            int contador = 0;
            tRut.Foreground = rut.Length == 9 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DD4337"));
            Rutok = rut.Length == 9 ?  true:false;
            if (rut.Length == 9)
            {
                for (int i = lengthRut - 1; i >= 0; i--)
                {
                    suma += multiplos[contador] * int.Parse(rut[i].ToString());
                    contador += 1;
                }
                int resto = 11 - (suma % 11);
                digito = resto == 10 ? "K" : resto == 11 ? "0" : resto.ToString();
                return rut[lengthRut].ToString() == digito ? "" + agregaPuntoGuion(rut) : "" + agregaPuntoGuion(rut);
            } return rut;
        }
        public string agregaPuntoGuion(string value)
        {
            int lengthRut = value.Length - 1;
            if (lengthRut <= 7)
            {
                value = "0" + value; //agregar un 0 antes de todos los digito
                lengthRut += 1; //aumentar el largo de la cadena(rut)
            }
            string rut = value.Substring(0, 2) + "." + value.Substring(2, 3) + "." + value.Substring(5, 3) + "-" + value[lengthRut];
            return value[0].ToString() == "0" ? rut.Substring(1, lengthRut) : rut;
        }
        private void btnAddAfp_Click(object sender, MouseButtonEventArgs e)
        {
            interfaz_apf_salud afp = new interfaz_apf_salud(0);
            afp.ShowDialog();
        }
        private void btnSalud_Click(object sender, MouseButtonEventArgs e)
        {
            interfaz_apf_salud afp = new interfaz_apf_salud(1);
            afp.ShowDialog();
        }
        public Boolean validacionAddUser()
        {
            String concadenacion = string.IsNullOrEmpty(tRut.Text.Trim()) ? "*Ingrese el rut para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tName.Text.Trim()) ? "*Ingrese el nombre para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tSurname.Text.Trim()) ? "*Ingrese el apellido para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tYear.Text.Trim()) ? "*Ingrese la edad para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tPhone.Text.Trim()) ? "*Ingrese el nro. telefonico para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tAdress.Text.Trim()) ? "*Ingrese la direccion para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tCtaBancaria.Text.Trim()) ? "*ingrese el n. de cta bancaria para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(cAfp.Text) ? "*Ingrese una afp del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(cSalud.Text) ? "*Ingrese un registro de salud del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(cDepto.Text) ? "*Ingrese un dpto del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tNacionalidad.Text.Trim()) ? "*Ingrese la nacionalidad del personal" + System.Environment.NewLine : "";
            concadenacion += path.Content.ToString().Equals("1")? "*Ingrese una foto de perfil para continuar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(tEmail.Text.Trim()) ? "*Ingrese el correo electronico para completar el registro" + System.Environment.NewLine : "";
            concadenacion += validaFecha(tDateNaci.Text.Trim()) ? "" : "*Formato de fecha nacimiento invalido." + System.Environment.NewLine;
            concadenacion += string.IsNullOrEmpty(tDateNaci.Text.Trim()) ? "*Ingrese fecha nacimiento para completar el  registro." + System.Environment.NewLine : "";
            valida_Rut(tRut.Text);
            concadenacion += Rutok == true ? "" : "*Ingrese un rut valido." + System.Environment.NewLine;
            concadenacion += tEmail.Text.Trim().Length > 0 ? email_bien_escrito(tEmail.Text.Trim()) ? "" : "*Correo electronico mal escrito, verifiquelo para continuar." + System.Environment.NewLine : "*Ingrese el correo electronico para completar el registro." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0){
                new Dialog(concadenacion).Show();
                ok = false;
            }
            return ok;
        }
        public Boolean validacionAddContract() {
            String concadenacion = validaFecha(tDateInit.Text.Trim()) ? "" : "*Formato fecha de inicio no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(tDateInit.Text.Trim()) == true ? "" : "*Ingrese una fecha de inicio." + System.Environment.NewLine;
            concadenacion += validaFecha(tDateEnd.Text.Trim()) ? "" : "*Formato de termino no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(tDateEnd.Text.Trim()) == true ? "" : "*Ingrese una fecha de termino." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(tStat.Text.Trim()) == true && (tStat.Text.Trim().ToUpper().Equals("VIGENTE") || tStat.Text.Trim().ToUpper().Equals("NO VIGENTE")) ? "" : "*Ingrese un estado de contrato VIGENTE/NO VIGENTE." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(cTypeContract.Text) == true ? "" : "*Seleccione un tipo de contrato." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(cCargo.Text) == true ? "" : "*Ingrese un cargo." + System.Environment.NewLine;
            concadenacion += DateTime.Compare(Convert.ToDateTime(tDateInit.Text), Convert.ToDateTime(tDateEnd.Text)) == -1 ? "" : "*Verifique que las fechas sean correctas." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0) { 
                new Dialog(concadenacion).Show();
                ok = false;
            }
            return ok;
        }
        public static bool email_bien_escrito(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            return Regex.IsMatch(email, expresion) ? Regex.Replace(email, expresion, String.Empty).Length == 0 ? true : false : false;
        }
        private void tName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaString(e);
        }
        private void tSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaString(e);
        }
        private void tYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaNumeros(e);
        }
        private void tPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validaNumeros(e);
        }
        private void tRut_LostFocus(object sender, RoutedEventArgs e)
        {
            tRut.Text =  valida_Rut(tRut.Text);
        }
        private void moveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void close_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
       private void minimize_Click(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }         
        //EVENTOS para el cambio de color de la imagen cerrar X
        private void CambiaColor(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/CloseRed.png"));
        }              
        private void ColorNormal(object sender, MouseEventArgs e)
        {
            image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Close2.png"));
        }

        private void iAddUser_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

/*>>>>FIN VALIDACIONES<<<<<*/
    }

}
