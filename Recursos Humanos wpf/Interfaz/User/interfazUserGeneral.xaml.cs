﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using Recursos_Humanos_wpf.Clases;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Recursos_Humanos_wpf.Interfaz
{
    public partial class interfazUserGeneral : UserControl
    {
        int flagCalendar = -1;
        Validaciones validacion = new Validaciones();
        List<Afp> listAfp = null;
        List<Salud> listSalud = null;
        List<Departamento> listDpto = null;
        List<Cargo> listCargo = null;
        List<tipo_jornada> listJornada = null;
        List<TipoContrato> listTipoContrato = null;
        List<Regiones> listReg = null;
        List<Comunas> listCom = null;
        List<Banco> listBank = null;
        MainWindow main;
        public interfazUserGeneral(MainWindow main)
        {        
            InitializeComponent();
            this.main = main;
            this.tabControl1.SelectedIndex = 0;
        }


        #region CRUD EMPLEADO
        public void Search()
        {
            try
            {
                String busqueda = main.cBusqueda.Text.Trim().Equals("") ? this.tRut.Text.Trim() : main.cBusqueda.Text.Trim();
                if (!busqueda.Equals(""))
                {
                    if (main.rbRut.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "rut");
                    else if (main.rbName.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "nombre");
                    else if (main.rbSurname.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "apellido");
                    else if (main.rbPhone.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "telefono");
                    else if (main.rbAdress.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "direccion");
                    else if (main.rbEmail.IsChecked == true) cargarDatosPersonal(main.cBusqueda.Text.Trim(), "email");
                }
                else
                {
                    main.cBusqueda.Focus();//doy el foco al cuadro de busqueda
                    new Dialog("Ingrese un parametro de búsqueda.", main).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnBuscar_click() " + ex.Message.ToString());
            }
        }
        //CARGA DATOS PERSONALES BASICOS
        public void cargarDatosPersonal(String value, String paramSearch)
        {
            if (!value.Equals(""))
            {
                main.WorkSpace.IsEnabled = true;
                tDateNaci.IsEnabled = false;//solo editable por el calendario
                //btnDateNacimiento.Visibility = Visibility.Hidden;
                object[] arreglo = new Clases.Personal().findBy(value, paramSearch);
                List<String> resul = new Clases.Pdf().Leerpaises();
                Console.WriteLine(">>>"+arreglo.Length);
                if (arreglo[0]!=null)
                {
                    Console.WriteLine("ENTRO");
                    //Iniciales en mayusculas
                    string nombre = arreglo[1].ToString();
                    string apellido = arreglo[2].ToString();
                    string[] nombres = nombre.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    string[] apellidos = apellido.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries);
                    nombre = apellido = "";
                    foreach (string x in nombres) { nombre += x.Substring(0, 1).ToUpper() + x.Substring(1, x.Length - 1).ToLower() + " "; }
                    foreach (string x in apellidos) { apellido += x.Substring(0, 1).ToUpper() + x.Substring(1, x.Length - 1).ToLower() + " "; }

                    lName.Content = nombre + apellido;

                    BitmapImage imagenes = new BitmapImage();//Byte[] datas = (byte[])arreglo[0];
                    imagenes.BeginInit();
                    MemoryStream stri = new MemoryStream((byte[])arreglo[0], 0, ((byte[])arreglo[0]).Length, false, false);
                    imagenes.StreamSource = stri;
                    imagenes.EndInit();
                    iPerfil.Source = imagenes;
                    tName.Text = arreglo[1].ToString(); //nombre
                    tSurname.Text = arreglo[2].ToString(); //apellido
                    tRut.Text = arreglo[3].ToString(); //rut
                    tDateNaci.Text = arreglo[4].ToString(); //fecha_nacimiento
                    Tdireccion.Text = arreglo[5].ToString(); // direccion
                    Regi.Items.Clear();
                    Comu.Items.Clear();
                    cSalud.Items.Clear();
                    cDepto.Items.Clear();
                    cAfp.Items.Clear();
                    tBank.Items.Clear();
                    int i = 0;
                    int c = 0;
                    foreach (Salud salud in new Salud().findAll())
                    {
                        cSalud.Items.Add(salud.name_salud);
                        if (salud.name_salud.Equals(arreglo[7].ToString())) cSalud.SelectedIndex = i; //salud
                        i++;
                    }
                    i = 0;
                    foreach (Departamento dpto in new Departamento().findAll())
                    {
                        cDepto.Items.Add(dpto.name);
                        if (dpto.name.Equals(arreglo[8].ToString())) cDepto.SelectedIndex = i; //dpto
                        i++;
                    }
                    i = 0;
                    foreach (Afp afp in new Afp().findAll())
                    {
                        cAfp.Items.Add(afp.nombre_afp);
                        if (afp.nombre_afp.Equals(arreglo[9].ToString())) cAfp.SelectedIndex = i; //afp
                        i++;
                    }
                    i = 0;
                    foreach (Regiones region in new Regiones().findAll())
                    {
                        Regi.Items.Add(region.nombre);
                        //El region.id_region habia que pasarlo a string ;)
                        if (region.id_region.ToString().Equals(arreglo[11].ToString())) //region_residencia
                        {
                            Regi.SelectedIndex = i;
                            foreach (Comunas comuna in new Comunas().FindByidReg(region.id_region))
                            {
                                Comu.Items.Add(comuna.nombre_comuna);
                                if (comuna.id_comuna.ToString().Equals(arreglo[6].ToString())) Comu.SelectedIndex = c; //comuna
                                c++;//busqueda de comuna
                            }//fin buscar comuna
                        }
                        i++;//busqueda de region
                    }//fin buscar region
                    tYear.Text = arreglo[10].ToString(); //edad
                    tPhone.Text = arreglo[12].ToString(); //telefono
                    tEmail.Text = arreglo[13].ToString(); //email
                    i = 0;
                    foreach (String paises in resul)
                    {
                        tNacionalidad.Items.Add(paises);
                        if (paises.Equals(arreglo[14].ToString())) tNacionalidad.SelectedIndex = i; //nombre_banco
                        i++;
                    }
                    tCtaBancaria.Text = arreglo[15].ToString(); // cta_bancaria
                    i = 0;
                    foreach (Banco banco in new Banco().findAll())
                    {
                        tBank.Items.Add(banco.nombre);
                        if (banco.nombre.Equals(arreglo[16].ToString())) tBank.SelectedIndex = i; //nombre_banco
                        i++;
                    }
                    i = 0;

                }
                else main.WorkSpace.IsEnabled = false;
                loadDataContract(this.tRut.Text.Trim());
                llenaaHorario(this.tRut.Text.Trim());
            }
        }
        //INGRESA NUEVO USUARIO
        public void btnAddUser_Click(object sender, MouseButtonEventArgs e)
        {
            this.tRut.IsEnabled = true;
            main.iAddUser.IsEnabled = true;
            QuestionDialog pregunta = new QuestionDialog("Desea agregar a esta persona?", main);
            pregunta.ShowDialog();
            if (pregunta.DialogResult == true && validacionAddUser())
            {
                byte[] foto = File.ReadAllBytes(path.Content.ToString());
                listDpto = new Departamento().findAll();
                listAfp = new Afp().findAll();
                listSalud = new Salud().findAll();
                listReg = new Regiones().findAll();
                listCom = new Comunas().FindByidReg(this.Regi.SelectedIndex + 1);
                listBank = new Banco().findAll();


                Personal per = new Personal(this.tRut.Text.Trim(), this.tName.Text.Trim(), this.tSurname.Text.Trim(),
                                            int.Parse(this.tYear.Text.Trim()), foto, this.tPhone.Text.Trim(), this.Tdireccion.Text.Trim(),
                                            this.tEmail.Text.Trim(), this.tCtaBancaria.Text.Trim(), this.tNacionalidad.Text.Trim(),
                                            this.tDateNaci.Text.Trim(), listCom[this.Comu.SelectedIndex].id_comuna, listReg[this.Regi.SelectedIndex].id_region,
                                            listAfp[this.cAfp.SelectedIndex].id, listSalud[this.cSalud.SelectedIndex].id

                                            );

                if (per.Save() > 0)
                {
                    Personal_Departamento pd = new Personal_Departamento(new Personal(this.tRut.Text.Trim()).get_idPersonal(), listDpto[this.cDepto.SelectedIndex].id);
                    Banco_Personal bp = new Banco_Personal(listBank[this.tBank.SelectedIndex].id, new Personal(this.tRut.Text.Trim()).get_idPersonal(), tCtaBancaria.Text.Trim());
                    if (pd.save() > 0 && bp.save() > 0)
                    {
                        main.listAutocomplet = new Clases.Personal().findAll(0);
                        main.cBusqueda.IsEnabled = true;
                        this.iPerfil.IsEnabled = false;
                        this.btnCancelAdd.Visibility = Visibility.Hidden;
                        this.btnUpdateReg.Visibility = Visibility.Visible;
                        this.btnDeleteReg.Visibility = Visibility.Visible;
                        QuestionDialog pregunta2 = new QuestionDialog("¿Desea contratar a este nuevo personal?", main);
                        pregunta2.ShowDialog();
                        if (pregunta2.DialogResult == true)                        
                        {
                            this.tabControl1.SelectedIndex = 1;
                            cargarDatosPersonal(this.tRut.Text, "rut");
                        }
                        else cargarDatosPersonal(this.tRut.Text, "rut");
                    }
                }
                else
                {
                    new Dialog("Personal no pudo ser ingresado", main).ShowDialog();
                }
            }
        }
        //ACTUALIZA DATOS EMPLEADO
        public void btnUpdateReg_Click(object sender, MouseButtonEventArgs e)
        {
            this.listAfp = new Afp().findAll();
            this.listSalud = new Salud().findAll();
            this.listDpto = new Departamento().findAll();
            this.listReg = new Regiones().findAll();
            this.listCom = new Comunas().FindByidReg(this.Regi.SelectedIndex + 1);
            this.listBank = new Banco().findAll();
            if (validacion.validaFecha(this.tDateNaci.Text.Trim()))
            {

                Personal per = new Personal(this.tRut.Text.Trim(), this.tName.Text.Trim(), this.tSurname.Text.Trim(),
                                                int.Parse(this.tYear.Text.Trim()), this.tPhone.Text.Trim(), this.Tdireccion.Text.Trim(),
                                                this.tEmail.Text.Trim(), this.tCtaBancaria.Text.Trim(), this.tNacionalidad.Text.Trim(),
                                                this.tDateNaci.Text.Trim(), listCom[this.Comu.SelectedIndex].id_comuna, listReg[this.Regi.SelectedIndex].id_region,
                                                listAfp[this.cAfp.SelectedIndex].id, listSalud[this.cSalud.SelectedIndex].id, listBank[this.tBank.SelectedIndex].id

                                                );

                if (per.Update() > 0)
                {
                    this.Search();
                    new Dialog("Datos actualizados correctamente.", main).ShowDialog();
                }
                else new Dialog("Ocurrio un error al actualizar los datos", main).ShowDialog();
            }
            else new Dialog("Ingrese formato fecha nacimiento 'YYYY-MM-DD'", main).ShowDialog();
        }
        //CANCELA INGRESO EMPLEADO
        public void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {
            Label[] labels = {this.btnAddUser, this.btnCancelAdd };
            foreach (Label x in labels) x.Visibility = Visibility.Hidden;
            main.WorkSpace.IsEnabled = false;
            main.cBusqueda.IsEnabled = true;
            this.btnUpdateReg.Visibility = Visibility.Visible;
            this.btnDeleteReg.Visibility = Visibility.Visible;
            main.image1.IsEnabled = true;
            this.calendar2.Visibility = Visibility.Hidden;
            limpiarTexbox();
        }
        //ELIMINA UN EMPLEADO
        public void btnDeleteReg_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.tabControl1.SelectedIndex = 0;
                String rut_per = this.tRut.Text.Trim();
                QuestionDialog pregunta = new QuestionDialog("Desea borrar el personal con rut: " + rut_per + " ?", main);
                pregunta.ShowDialog();
                if (pregunta.DialogResult == true)                
                {
                    if (new Clases.Personal(rut_per).DeleteByRrut() > 0)
                    {
                        this.limpiarTexbox();
                        main.cBusqueda.Text = "";
                        main.cBusqueda.Focus();
                        main.WorkSpace.IsEnabled = false;
                        new Dialog("El empleado con rut " + rut_per + " fue eliminado satisfactoriamente.", main).ShowDialog();
                    }
                    else new Dialog("Ocurrio algo inesperado al eliminar al empleado con rut " + rut_per + ".", main).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en eliminar personal" + ex.Message);
            }
        }
        #endregion
        #region CRUD CONTRATO
        //ELIMINA UN CONTRATO ASOCIADO A UN EMPLEADO
        private void btnEndContract_Click(object sender, MouseButtonEventArgs e)
        {
            string rut_per = this.tRut.Text.Trim();
            QuestionDialog pregunta = new QuestionDialog("Desea borrar el contrato asociado al rut: " + rut_per + " ?", main);
            pregunta.ShowDialog();
            if (pregunta.DialogResult == true)
            {
                if (new Clases.Contratos().DeleteByRut(new Personal(rut_per)) > 0)
                {
                    this.limpiarTexbox();
                    this.loadDataContract(rut_per);
                    this.cargarDatosPersonal(rut_per, "rut");
                    new Dialog("Se cancelo el contrato a empleado con rut " + rut_per + ".", main).ShowDialog(); //MessageBox.Show("Se elimino el contrato con exito"); 
                }
            }
        }
        //CARGA CONTRATOS DE UN EMPLEADO ESPECIFICO
        public  void loadDataContract(string value)
        {
            ClearContract();
            this.tabItem2.IsEnabled = !string.IsNullOrEmpty(this.tRut.Text.Trim()) == true ? true : false;
            List<String> estados = new List<String>();
            estados.Add("VIGENTE");
            estados.Add("NO VIGENTE");
            string sql = "SELECT e.fecha_inicio,e.fecha_termino,e.estado,"
             + " (SELECT c.tipo AS tipo_contrato"
             + " FROM personal_contrato AS pc"
             + " INNER JOIN personal AS p ON(p.id_personal = pc.id_personal)"
             + " INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             + " INNER JOIN tipo_contrato AS c ON(e.tipo_contrato_id_tipo_contrato=c.id_tipo_contrato)"
             + " WHERE p.rut ='" + value + "') AS tipo_contrato,"
             + " IFNULL((SELECT f.cargo AS nombre_cargo"
             + " FROM personal_contrato AS pc"
             + " INNER JOIN personal AS p ON(p.id_personal = pc.id_personal)"
             + " INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             + " INNER JOIN cargo AS f ON(f.id_cargo = e.cargo_id_cargo)"
             + " WHERE p.rut = '" + value + "'),'1')AS cargo"
             + " FROM personal AS p"
             + " INNER JOIN personal_contrato AS pc ON(p.id_personal = pc.id_personal)"
             + " INNER JOIN contrato AS e ON (e.id_contrato = pc.id_contrato)"
             + " INNER JOIN cargo AS f ON(f.id_cargo = e.cargo_id_cargo)"
             + " WHERE p.nombre = '" + value + "' OR p.apellido ='" + value + "' OR p.rut = '" + value + "'";
            string interfaces = "";
            foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
            {
                this.lPuesto.Content = dtRow["cargo"].ToString();
                interfaces = dtRow["cargo"].ToString();
                this.tDateInit.Text = validacion.DateFormat(dtRow["fecha_inicio"].ToString());
                this.tDateEnd.Text = validacion.DateFormat(dtRow["fecha_termino"].ToString());
                int i = 0;
                foreach (String es in estados)
                {
                    this.tStat.Items.Add(es);
                    if (es.Equals(dtRow["estado"].ToString())) this.tStat.SelectedIndex = i; //nombre_banco
                    i++;
                }
                i = 0;
                this.cTypeContract.Items.Add(dtRow["tipo_contrato"].ToString());
                this.cTypeContract.SelectedItem = dtRow["tipo_contrato"].ToString();
                this.cCargo.Items.Add(dtRow["cargo"].ToString());
                this.cCargo.SelectedItem = dtRow["cargo"].ToString();
            }
            Label[] labelVisible = { this.label15, this.label16, this.label17, this.label18, this.label19, this.label20 };
            Label[] btnVisible = { this.btnShowContract, this.btnInsertNewContract, this.btnCancelNewContract };
            //cambia color en caso de que no existe contrato 
            this.lDescription.Background = interfaces == "1" || interfaces == "" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd4337")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4185f4"));
            this.lDescription.Content = interfaces == "1" || interfaces == "" ? "Usuario sin contrato" : this.cCargo.Text;
            this.tDateInit.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.tDateEnd.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.tStat.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.cTypeContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.cCargo.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.cJornada.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;

            this.tDateInit.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.tDateEnd.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.tStat.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.cTypeContract.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.cCargo.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.cJornada.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;

            this.btnEndContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.btnNewContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Visible : Visibility.Hidden;

            foreach (Label x in labelVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            foreach (Label x in btnVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Hidden;
            this.btnDateInitCalendar.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Hidden;
            this.btnDateEndCalendar.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Hidden;
        }
        //CREA INTERFAZ PARA AGREGAR CONTRATO
        private void btnNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            this.lDescription.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#129d5a"));
            this.lDescription.Content = "Agregar Nuevo contrato";
            Label[] labels = { this.label15, this.label16, this.label17, this.label18, this.label19, this.label20, this.btnShowContract, this.btnInsertNewContract, this.btnCancelNewContract };
            foreach (Label x in labels) x.Visibility = Visibility.Visible;
            this.tDateInit.IsEnabled = false;
            this.tDateEnd.IsEnabled = false;
            this.tDateInit.Visibility = Visibility.Visible;
            this.tDateEnd.Visibility = Visibility.Visible;
            this.tStat.Visibility = Visibility.Visible;
            this.cTypeContract.Visibility = Visibility.Visible;
            this.cCargo.Visibility = Visibility.Visible;
            this.cJornada.Visibility = Visibility.Visible;
            this.btnEndContract.Visibility = Visibility.Hidden;
            this.btnNewContract.Visibility = Visibility.Hidden;
            this.btnDateInitCalendar.Visibility = Visibility.Visible;
            this.btnDateEndCalendar.Visibility = Visibility.Visible;
        }
        //CANCELA INGRESO CONTRATO (RECARGA INTERFAZ CONTRATO)
        private void btnCancelNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            this.loadDataContract(this.tRut.Text.Trim());
        }
        //MUESTRA PREVISUALIZACION CONTRATO
        private void btnShowContract_Click(object sender, MouseButtonEventArgs e)
        {
            bool crearcarp = new Clases.Pdf().CrearCarpetaXml("contratos");
            if (crearcarp)
            {
                Boolean flag = true;
                try
                {
                    Console.WriteLine("init");
                    ThreadPool.QueueUserWorkItem(o =>
                    {
                        while (flag)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                this.label.Content = "CARGANDO";
                            }));
                            Thread.Sleep(100);
                        }
                    });
                    listCargo = new Cargo().findAll(this.cTypeContract.SelectedIndex + 1);
                    listTipoContrato = new TipoContrato().findAll();
                    listJornada = new tipo_jornada().findforCargo(this.cCargo.Text);
                    Clases.Contratos contrato = new Clases.Contratos(this.tRut.Text, this.tDateInit.Text, this.tName.Text + " " + this.tSurname.Text, this.Tdireccion.Text,
                                                            listCargo[this.cCargo.SelectedIndex].cargo, this.cDepto.Text.Trim(),
                                                            listTipoContrato[this.cTypeContract.SelectedIndex].tipo, 
                                                            new Contratos().get_sueldo(listCargo[this.cCargo.SelectedIndex].id.ToString(), listJornada[this.cJornada.SelectedIndex].id_tipo_jornada.ToString()),
                                                            this.tDateEnd.Text,
                                                            this.cAfp.Text.Trim(), this.cSalud.Text.Trim()
                        );
                   
                    new Clases.Pdf().CrearArchivoXml("contratos/contract.xml",
                    contrato.rut, contrato.fInicio, contrato.nombre_completo, contrato.direccion, contrato.Cargo, contrato.depto, contrato.tContrato,
                    contrato.SueldoBase, contrato.fTermino, contrato.afp, contrato.salud);
                    Document document = new Document();

                    List<String> resul = new Clases.Pdf().Leer("administrativo");

                    PdfWriter.GetInstance(document, new FileStream("contrato.pdf", FileMode.OpenOrCreate));
                    document.Open();
                    Chunk chunk = new Chunk(resul[0], FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.UNDERLINE));

                    document.Add(new iTextSharp.text.Paragraph(chunk));
                    for (int i = 1; i < resul.Count; i++)
                    {
                        document.Add(new iTextSharp.text.Paragraph(resul[i]));
                    }
                    document.Close();

                    new Dialog("Contrato generado con exito.", main).ShowDialog();
                    flag = false;
                    System.Diagnostics.Process.Start("contrato.pdf");
                    this.label.Content = "";
                }
                catch (Exception ex)
                {
                    flag = false;
                    new Dialog("Ocurrio un error al generar el contrato.", main).ShowDialog();
                    this.label.Content = "";
                    Console.WriteLine("MainWindow.btnShowContract_Click() " + ex.Message.ToString());
                }
            }
        }
        //INGRESA UN CONTRATO A UN EMPLEADO
        private void btnInsertNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                String rut_per = this.tRut.Text.Trim();
                QuestionDialog pregunta = new QuestionDialog("Desea asignar el contrato al rut: " + rut_per + " ?", main);
                pregunta.ShowDialog();
                if (pregunta.DialogResult == true && validacionAddContract())
                {
                    listCargo = new Cargo().findAll(this.cTypeContract.SelectedIndex + 1);
                    listTipoContrato = new TipoContrato().findAll();
                    listJornada = new tipo_jornada().findforCargo(this.cCargo.Text);
                    Clases.Contratos contrato = new Contratos(rut_per, this.tDateInit.Text, this.tDateEnd.Text, this.tStat.Text.ToUpper(),
                                                    listTipoContrato[this.cTypeContract.SelectedIndex].id.ToString(), listCargo[this.cCargo.SelectedIndex].id.ToString(),
                                                    listJornada[this.cJornada.SelectedIndex].id_tipo_jornada.ToString());
                    if (contrato.save() > 0)
                    {
                        loadDataContract(rut_per);
                        new Dialog("Se ingreso contrato a empleado con rut " + rut_per + ".", main).ShowDialog(); //MessageBox.Show("Contrato ingresado exitosamente.");
                        this.tDateInit.IsEnabled = false;
                        this.tDateEnd.IsEnabled = false;
                        this.tStat.IsEnabled = false;
                        this.cTypeContract.IsEnabled = false;
                        this.cCargo.IsEnabled = false;

                    }
                    else new Dialog("Ocurrio un error al ingresar contrato a persona con rut " + rut_per + ".", main).Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MainWindow.btnInsertNewContract_Click() " + ex.Message.ToString());
            }

        }
        #endregion

        /*>>>>CALENDARIOS >>>>>>*/
        #region Calendarios
        //ABRE EL CALENDARIO
        private void btnDateInitCalendar_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 0;
            this.calendar1.Visibility = Visibility.Visible;
            this.calendar1.Margin = this.btnDateInitCalendar.Margin;// new Thickness(0, 130, 0, 0);
        }
        //SETEA LA FECHA DE INICIO O TERMINO DE COTRATO
        private void selectedDateChanges_Clikc(object sender, SelectionChangedEventArgs e)
        {
            this.tDateInit.Text = flagCalendar == 0 ? this.calendar1.SelectedDate.Value.ToString("yyyy-MM-dd") : this.tDateInit.Text;
            this.tDateEnd.Text = flagCalendar == 1 ? this.calendar1.SelectedDate.Value.ToString("yyyy-MM-dd") : this.tDateEnd.Text;
            this.calendar1.Visibility = Visibility.Hidden;
            flagCalendar = -1;
        }
        //ABRE EL CALENDARIO
        private void btnDateEndCalendar_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 1;
            this.calendar1.Visibility = Visibility.Visible;
            this.calendar1.Margin = this.btnDateEndCalendar.Margin;// new Thickness(0, 151, 0, 0);
        }
        private void btnDateNacimiento_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 2;
            this.calendar2.Focus();
            this.calendar2.Visibility = Visibility.Visible;
            this.calendar2.DisplayMode = CalendarMode.Decade;
            this.calendar2.Margin = this.btnDateNacimiento.Margin;
        }
        private void calendar2_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            this.tDateNaci.Text = flagCalendar == 2 ? this.calendar2.SelectedDate.Value.ToString("yyyy-MM-dd") : this.tDateNaci.Text;
            this.tYear.Text = (2014 - int.Parse(this.tDateNaci.Text.Substring(0, 4))).ToString();
            this.calendar2.Visibility = Visibility.Hidden;
        }
        #endregion fin Calendarios


        //CARGA IMAGEN A CONTENDOR IMAGEN PERFIL
        private void iPerfil_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(new Action(() => { addEfecto(); }));
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Imagenes jpg(*.jpg)|*.jpg";
                if (ofd.ShowDialog() == true)
                {
                    this.path.Content = ofd.FileName;
                    using (Stream stream = ofd.OpenFile())
                    {
                        BitmapDecoder bitdecoder = BitmapDecoder.Create(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                        this.iPerfil.Source = bitdecoder.Frames[0];
                    }
                } Dispatcher.BeginInvoke(new Action(() => { QuitarEfecto(); }));
            }
            catch (Exception ex)
            {
                Console.Write("error: " + ex.Message);
                new Dialog("Seleccione una imagen mas pequeña.", main).ShowDialog();// MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido" + ex.Message);
            }
        }

        #region CARGA COMBOBOX
        //CARGA DATOS DE AFP EN COMBOBOX
        private void cAfp_Click(object sender, MouseButtonEventArgs e)
        {
            this.cAfp.Items.Clear();
            foreach (Afp afp in new Afp().findAll()) this.cAfp.Items.Add(afp.nombre_afp);
        }
        //CARGA DATOS DE SALUD EN COMBOBOX
        private void cSalud_Click(object sender, MouseButtonEventArgs e)
        {
            this.cSalud.Items.Clear();
            foreach (Salud salud in new Salud().findAll()) this.cSalud.Items.Add(salud.name_salud);
        }
        //CARGA DATOS DE DEPARTAMENTO EN COMBOBOX
        private void cDepto_Click(object sender, MouseButtonEventArgs e)
        {
            this.cDepto.Items.Clear();
            foreach (Departamento dpto in new Departamento().findAll()) this.cDepto.Items.Add(dpto.name);
        }
        //CARGA LOS TIPOS DE CONTRATOS
        private void cTypeContract_Click(object sender, MouseButtonEventArgs e)
        {
            this.cTypeContract.Items.Clear();
            foreach (TipoContrato tContrato in new TipoContrato().findAll()) this.cTypeContract.Items.Add(tContrato.tipo);
        }
        //CARGA LOS DIFERENTE CARGOS
        private void cCargo_Click(object sender, MouseButtonEventArgs e)
        {
            this.cCargo.Items.Clear();
            int busqueda = this.cTypeContract.SelectedIndex + 1;
            foreach (Cargo cargo in new Cargo().findAll(busqueda)) this.cCargo.Items.Add(cargo.cargo);
        }
        // CARGAR LAS COMUNAS
        private void Loadcom_Click(object sender, MouseButtonEventArgs e)
        {
            this.Comu.Items.Clear();
            int busqueda = this.Regi.SelectedIndex + 1;
            foreach (Comunas comuna in new Comunas().FindByidReg(busqueda)) this.Comu.Items.Add(comuna.nombre_comuna);
        }
        //CARGA LAS REGIONES
        private void Loadreg_Click(object sender, MouseButtonEventArgs e)
        {
            this.Regi.Items.Clear();
            foreach (Regiones regiones in new Regiones().findAll()) this.Regi.Items.Add(regiones.nombre);
        }
        private void cargarBanco(object sender, MouseButtonEventArgs e)
        {
            this.tBank.Items.Clear();
            foreach (Banco banco in new Banco().findAll()) this.tBank.Items.Add(banco.nombre);
        }
        private void verpaises(object sender, MouseButtonEventArgs e)
        {
            List<String> resul = new Clases.Pdf().Leerpaises();
            foreach (String pais in resul) this.tNacionalidad.Items.Add(pais.Trim());
        }
        private void tStat_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.tStat.Items.Clear();
            List<String> estados = new List<String>();
            estados.Add("VIGENTE");
            estados.Add("NO VIGENTE");
            foreach (String es in estados) this.tStat.Items.Add(es);
        }
        private void cTypeContract_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Al cambiar el tipo de contrato, reseteo el combobox del cargo
            foreach (Cargo cargo in new Cargo().findAll(cTypeContract.SelectedIndex))
            {
                cCargo.Items.Clear();
                Comu.Items.Add(cargo.cargo);
            }
        }
        #endregion FIN CARGA COMBOBOX

        #region VALIDACIONES
        public Boolean validacionAddUser()
        {
            String concadenacion = string.IsNullOrEmpty(this.tRut.Text.Trim()) ? "*Ingrese el rut para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tName.Text.Trim()) ? "*Ingrese el nombre para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tSurname.Text.Trim()) ? "*Ingrese el apellido para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tYear.Text.Trim()) ? "*Ingrese la edad para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tPhone.Text.Trim()) ? "*Ingrese el nro. telefonico para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.Tdireccion.Text.Trim()) ? "*Ingrese la direccion para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tCtaBancaria.Text.Trim()) ? "*ingrese el n. de cta bancaria para completar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.cAfp.Text) ? "*Ingrese una afp del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.cSalud.Text) ? "*Ingrese un registro de salud del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.cDepto.Text) ? "*Ingrese un dpto del menu desplegable para continuar" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tNacionalidad.Text.Trim()) ? "*Ingrese la nacionalidad del personal" + System.Environment.NewLine : "";
            concadenacion += path.Content.ToString().Equals("1") ? "*Ingrese una foto de perfil para continuar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tEmail.Text.Trim()) ? "*Ingrese el correo electronico para completar el registro" + System.Environment.NewLine : "";
            concadenacion += validacion.validaFecha(this.tDateNaci.Text.Trim()) ? "" : "*Formato de fecha nacimiento invalido." + System.Environment.NewLine;
            concadenacion += string.IsNullOrEmpty(this.tDateNaci.Text.Trim()) ? "*Ingrese fecha nacimiento para completar el registro." + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.Regi.Text.Trim()) ? "*Seleccione una region de residencia." + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.Comu.Text.Trim()) ? "*Seleccione una comuna de residencia." + System.Environment.NewLine : "";
            validacion.validaRut(this.tRut.Text, this.tRut);
            //concadenacion += Rutok == true ? "" : "*Ingrese un rut valido." + System.Environment.NewLine;
            concadenacion += tEmail.Text.Trim().Length > 0 ? validacion.validaEmail(tEmail.Text.Trim()) ? "" : "*Correo electronico mal escrito, verifiquelo para continuar." + System.Environment.NewLine : "*Ingrese el correo electronico para completar el registro." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0)
            {
                new Dialog(concadenacion, main).ShowDialog();
                ok = false;
            }
            return ok;
        }
        public Boolean validacionAddContract()
        {
            String concadenacion = validacion.validaFecha(this.tDateInit.Text.Trim()) ? "" : "*Formato fecha de inicio no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tDateInit.Text.Trim()) == true ? "" : "*Ingrese una fecha de inicio." + System.Environment.NewLine;
            concadenacion += validacion.validaFecha(this.tDateEnd.Text.Trim()) ? "" : "*Formato de termino no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tDateEnd.Text.Trim()) == true ? "" : "*Ingrese una fecha de termino." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tStat.Text.Trim()) == true && (this.tStat.Text.Trim().ToUpper().Equals("VIGENTE") || this.tStat.Text.Trim().ToUpper().Equals("NO VIGENTE")) ? "" : "*Ingrese un estado de contrato VIGENTE/NO VIGENTE." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.cTypeContract.Text) == true ? "" : "*Seleccione un tipo de contrato." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.cCargo.Text) == true ? "" : "*Ingrese un cargo." + System.Environment.NewLine;
            concadenacion += DateTime.Compare(Convert.ToDateTime(this.tDateInit.Text), Convert.ToDateTime(this.tDateEnd.Text)) == -1 ? "" : "*Verifique que las fechas sean correctas." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0)
            {
                new Dialog(concadenacion, main).ShowDialog();
                ok = false;
            }
            return ok;
        }

        private void tName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validacion.validaString(e);
        }
        private void tSurname_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validacion.validaString(e);
        }
        private void tYear_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //validacion.validaNumeros(e);
        }
        private void tPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validacion.validaNumeros(e);
        }
        private void tRut_LostFocus(object sender, RoutedEventArgs e)
        {
            this.tRut.Text = validacion.validaRut(this.tRut.Text, this.tRut);
        }
        #endregion FIN VALIDACIONES
        

        private void Tdireccion_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            validacion.validaString(e);
        }

        private void Regi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Al cambiar la region, reseteo el combobox de las comunas
            foreach (Comunas comuna in new Comunas().FindByidReg(Regi.SelectedIndex))
            {
                Comu.Items.Clear();
                Comu.Items.Add(comuna.nombre_comuna);
            }
        }

        public void llenaaHorario(String rut)
        {

            this.gHorario.ItemsSource = new Registro_Horario(new Personal(rut).get_idPersonal()).findAll();
        }
        public void ClearContract()
        {
            this.tDateInit.Text = "";
            this.tDateEnd.Text = "";
            this.tStat.Text = "";
            this.cTypeContract.Items.Clear();
            this.cCargo.Items.Clear();
        }

        public void limpiarTexbox()
        {
            this.lPuesto.Content = "";
            this.lName.Content = "";
            TextBox[] campos = { this.tRut, this.tName, this.tSurname, this.tYear, this.tPhone, this.Tdireccion,this.tEmail, this.tCtaBancaria, 
                                 this.tDateEnd, this.tDateInit, this.tDateNaci,  };
            ComboBox[] combos = { this.cAfp, this.cDepto, this.cSalud, this.cTypeContract, this.cCargo, this.Regi, this.Comu, this.tBank, this.tNacionalidad, this.tStat };
            foreach (TextBox x in campos) x.Text = "";
            foreach (ComboBox x in combos) x.Items.Clear();
            this.iPerfil.Source = new BitmapImage(new Uri("pack://application:,,,/Images/icono.png"));
        }

        private void calendar2_MouseLeave(object sender, MouseEventArgs e)
        {
            this.calendar2.Visibility = Visibility.Hidden;
        }

        private void cJornada_Click(object sender, MouseButtonEventArgs e)
        {
            this.cJornada.Items.Clear();
            int busqueda = this.cTypeContract.SelectedIndex + 1;
            foreach (tipo_jornada jorn in new tipo_jornada().findforCargo(cCargo.Text)) this.cJornada.Items.Add(jorn.nombre);

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
    }
}
