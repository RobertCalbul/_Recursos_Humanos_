using System;
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
        List<Regiones> listReg = null;
        List<Comunas> listCom = null;
        List<Banco> listBank = null;
        Validaciones validacion = new Validaciones();
               
        public MainWindow()
        {
            InitializeComponent();
            listAutocomplet = new Clases.Personal().findAll(0);
            this.tabControl1.SelectedIndex = 0;
            this.grid5.IsEnabled = false;
            this.rbRut.IsChecked = true;

        }
        /*>>>>INICIO OPERACIONES CRUD EMPLEADOS<<<<<*/
        //BUSCA DATOS EMPLEADOS POR FILTRO
        
        private void Search(){
          try
            {
                String busqueda = cBusqueda.Text.Trim().Equals("") ? tRut.Text.Trim() : cBusqueda.Text.Trim();
                if (!busqueda.Equals(""))
                {
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
            this.calendar1.Visibility = Visibility.Hidden;
        }
        //CARGA DATOS PERSONALES BASICOS
        private void cargarDatosPersonal(String value, String paramSearch)
        {
            if (!value.Equals(""))
            {
                grid5.IsEnabled = true;
                tDateNaci.IsEnabled = false;//solo editable por el calendario
                //btnDateNacimiento.Visibility = Visibility.Hidden;
                object[] arreglo = new Clases.Personal().findBy(value, paramSearch);
                List<String> resul = new Clases.PDF().leerpaises();
                if (arreglo != null)
                {
                    lName.Content = arreglo[1].ToString() + " " + arreglo[2].ToString(); 
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
                        if (afp.nombre_afp.Equals(arreglo[9].ToString()))cAfp.SelectedIndex = i; //afp
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
                    foreach (String paises in  resul)
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
                    
                } loadDataContract(tRut.Text.Trim());
            }
        }
        // click en agregar usuario
        private void iAddUser_Click(object sender, MouseButtonEventArgs e)
        {

            limpiarTexbox();
            this.grid5.IsEnabled = true;
            this.tDateNaci.IsEnabled = false;
            this.tRut.IsEnabled = true;
            this.iPerfil.IsEnabled = true;
            this.btnAddUser.IsEnabled = true;
            this.cBusqueda.IsEnabled = false;
            this.image1.IsEnabled = false;
<<<<<<< HEAD
            //this.btnAddAfp.Visibility = Visibility.Visible;
            this.btnAddUser.Visibility = Visibility.Visible;
            this.btnCancelAdd.Visibility = Visibility.Visible;
            //this.btnAddSalud.Visibility = Visibility.Visible;
=======
     //     this.btnAddAfp.Visibility = Visibility.Visible;
            this.btnAddUser.Visibility = Visibility.Visible;
            this.btnCancelAdd.Visibility = Visibility.Visible;
   //       this.btnAddSalud.Visibility = Visibility.Visible;
>>>>>>> e43155ed15d093a86b7d401e4bb2586d9706979a
            this.btnDateNacimiento.Visibility = Visibility.Visible;
            this.btnUpdateReg.Visibility = Visibility.Hidden;
            this.btnDeleteReg.Visibility = Visibility.Hidden;
            this.tabControl1.SelectedIndex = 0;
            this.tName.Focus();
            this.cBusqueda.Text = "";
            loadDataContract("");
        }
        //INGRESA NUEVO USUARIO
        private void btnAddUser_Click(object sender, MouseButtonEventArgs e)
        {   this.tRut.IsEnabled = true;
            this.iAddUser.IsEnabled = true;
                MessageBoxResult dialogResult = MessageBox.Show("Desea agregar a esta persona?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes && validacionAddUser())
                {
                    byte[] foto = File.ReadAllBytes(path.Content.ToString());
                    listDpto = new Departamento().findAll();
                    listAfp = new Afp().findAll();
                    listSalud = new Salud().findAll();
                    listReg = new Regiones().findAll();
                    listCom = new Comunas().FindByidReg(this.Regi.SelectedIndex+1);
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
                        Banco_Personal bp = new Banco_Personal (listBank[this.tBank.SelectedIndex].id,new Personal(this.tRut.Text.Trim()).get_idPersonal(),tCtaBancaria.Text.Trim());
                        if (pd.save() > 0 && bp.save() > 0)
                        {
                            listAutocomplet = new Clases.Personal().findAll(0);
                            this.cBusqueda.IsEnabled = true;
                            this.iPerfil.IsEnabled = false;
                            this.btnAddUser.Visibility = Visibility.Hidden;
            //              this.btnAddAfp.Visibility = Visibility.Hidden;
          //                this.btnAddSalud.Visibility = Visibility.Hidden;
                            this.btnCancelAdd.Visibility = Visibility.Hidden;
                            this.btnUpdateReg.Visibility = Visibility.Visible;
                            this.btnDeleteReg.Visibility = Visibility.Visible;
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
        }
        
        //ACTUALIZA DATOS EMPLEADO
        private void btnUpdateReg_Click(object sender, MouseButtonEventArgs e)
        {
            listAfp = new Afp().findAll();
            listSalud = new Salud().findAll();
            listDpto = new Departamento().findAll();
            listReg = new Regiones().findAll();
            listCom = new Comunas().FindByidReg(this.Regi.SelectedIndex + 1);
            listBank = new Banco().findAll();
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
                    Search();
                    new Dialog("Datos actualizados correctamente.").Show();
                }
                else new Dialog("Ocurrio un error al actualizar los datos").Show();
            }
            else new Dialog("Ingrese formato fecha nacimiento 'YYYY-MM-DD'").Show();
        }
        //CANCELA INGRESO EMPLEADO
        private void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {


            Label[] labels = {/* this.btnAddAfp, this.btnAddSalud,*/ this.btnAddUser, this.btnCancelAdd };
            foreach (Label x in labels) x.Visibility = Visibility.Hidden;
            this.grid5.IsEnabled = false;
            this.cBusqueda.IsEnabled = true;
            this.btnUpdateReg.Visibility = Visibility.Visible;
            this.btnDeleteReg.Visibility = Visibility.Visible;
            this.image1.IsEnabled = true;
            this.calendar2.Visibility = Visibility.Hidden;
            limpiarTexbox();
        }
        //ELIMINA UN EMPLEADO
        private void btnDeleteReg_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.tabControl1.SelectedIndex = 0;
                String rut_per = this.tRut.Text.Trim();
                MessageBoxResult dialogResult = MessageBox.Show("Desea borrar el personal con rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    if (new Clases.Personal(rut_per).DeleteByRrut() > 0)
                    {
                        limpiarTexbox();
                        this.cBusqueda.Text = "";
                        this.cBusqueda.Focus();
                        this.grid5.IsEnabled = false;
                        new Dialog("El empleado con rut " + rut_per + " fue eliminado satisfactoriamente.").Show();
                    }
                    else  new Dialog("Ocurrio algo inesperado al eliminar al empleado con rut " + rut_per+".").Show();
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
            string rut_per = this.tRut.Text.Trim();
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
            this.tabItem2.IsEnabled = !string.IsNullOrEmpty(this.tRut.Text.Trim()) == true ? true : false;
            List<String> estados = new List<String>();  
            estados.Add("VIGENTE");
            estados.Add("NO VIGENTE");
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
                this.lPuesto.Content = dtRow["cargo"].ToString();
                interfaces = dtRow["cargo"].ToString();
                this.tDateInit.Text = validacion.DateFormat(dtRow["fecha_inicio"].ToString());
                this.tDateEnd.Text = validacion.DateFormat(dtRow["fecha_termino"].ToString());
                //this.tStat.Text = dtRow["estado"].ToString();
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
            Label[] btnVisible = { this.btnShowContract, this.btnInsertNewContract, this.btnCancelNewContract, this.btnDateEndCalendar, this.btnDateInitCalendar };
            //cambia color en caso de que no existe contrato PD: ROBERT QLO xd
            this.lDescription.Background = interfaces == "1" || interfaces == "" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd4337")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4185f4"));
            this.lDescription.Content = interfaces == "1" || interfaces == "" ? "Usuario sin contrato" : this.cCargo.Text;
            this.tDateInit.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.tDateEnd.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.tStat.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.cTypeContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.cCargo.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;

            this.tDateInit.IsEnabled = interfaces == "1" || interfaces == "" ?  true: false;
            this.tDateEnd.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.tStat.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.cTypeContract.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;
            this.cCargo.IsEnabled = interfaces == "1" || interfaces == "" ? true : false;

            this.btnEndContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            this.btnNewContract.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Visible : Visibility.Hidden;

            foreach(Label x in labelVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Visible;
            foreach (Label x in btnVisible) x.Visibility = interfaces == "1" || interfaces == "" ? Visibility.Hidden : Visibility.Hidden;
        }

        //CREA INTERFAZ PARA AGREGAR CONTRATO
        private void btnNewContract_Click(object sender, MouseButtonEventArgs e)
        {
            this.lDescription.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#129d5a"));
            this.lDescription.Content = "Agregar Nuevo contrato";
            Label[] labels = { this.label15, this.label16, this.label17, this.label18, this.label19, this.label20, this.btnShowContract, this.btnInsertNewContract, this.btnCancelNewContract };
            foreach (Label x in labels) x.Visibility = Visibility.Visible;
            this.tDateInit.Visibility = Visibility.Visible;
            this.tDateEnd.Visibility = Visibility.Visible;
            this.tStat.Visibility = Visibility.Visible;
            this.cTypeContract.Visibility = Visibility.Visible;
            this.cCargo.Visibility = Visibility.Visible;
            this.btnEndContract.Visibility = Visibility.Hidden;
            this.btnNewContract.Visibility = Visibility.Hidden;
            this.btnDateInitCalendar.Visibility = Visibility.Visible;
            this.btnDateEndCalendar.Visibility = Visibility.Visible;
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
                    listCargo = new Cargo().findAll(this.cTypeContract.SelectedIndex + 1);
                    listTipoContrato = new TipoContrato().findAll();
                    Clases.Contratos contrato = new Clases.Contratos(this.tRut.Text, this.tDateInit.Text, this.tName.Text + " " + this.tSurname.Text, this.Tdireccion.Text,
                                                            listCargo[this.cCargo.SelectedIndex].cargo, this.cDepto.Text.Trim(),
                                                            listTipoContrato[this.cTypeContract.SelectedIndex].tipo, 25000, this.tDateEnd.Text,
                                                            this.cAfp.Text.Trim(), this.cSalud.Text.Trim()
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
                String rut_per = this.tRut.Text.Trim();
                MessageBoxResult dialogResult = MessageBox.Show("Desea asignar el contrato al rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes && validacionAddContract())
                {
                    listCargo = new Cargo().findAll(this.cTypeContract.SelectedIndex + 1);
                    listTipoContrato = new TipoContrato().findAll();
                    Clases.Contratos contrato = new Contratos(rut_per, this.tDateInit.Text, this.tDateEnd.Text, this.tStat.Text.ToUpper(),
                                                    250000, listTipoContrato[this.cTypeContract.SelectedIndex].id.ToString(), listCargo[this.cCargo.SelectedIndex].id.ToString());
                    if (contrato.save() > 0)
                    {
                        loadDataContract(rut_per);
                        new Dialog("Se ingreso contrato a empleado con rut " + rut_per + ".").Show(); //MessageBox.Show("Contrato ingresado exitosamente.");
                        this.tDateInit.IsEnabled = false;
                        this.tDateEnd.IsEnabled = false;
                        this.tStat.IsEnabled = false;
                        this.cTypeContract.IsEnabled = false;
                        this.cCargo.IsEnabled = false;

                    }
                    else new Dialog("Ocurrio un error al ingresar contrato a persona con rut " + rut_per + ".").Show();
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
 /*FIN CALENDARIO<<<<<*/
/*>>>>AUTOCOMPLETE<<<<<*/
        private void cBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typed = this.cBusqueda.Text.Trim();

            //this.lAutoComplete = new AutoComplete().llenaList(typed);
            List<string> autoList = new List<string>();
            autoList.Clear();
            foreach (string item in listAutocomplet)
            {
                if (!string.IsNullOrEmpty(this.cBusqueda.Text.Trim()))
                {
                    if (item.StartsWith(typed))
                    {
                        autoList.Add(item);
                    }
                }
            }

            if (autoList.Count > 0)
            {
                this.lAutoComplete.ItemsSource = autoList;
                this.lAutoComplete.Visibility = Visibility.Visible;
            }
            else if (this.cBusqueda.Text.Equals(""))
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
            else
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.lAutoComplete.ItemsSource = null;
            }
        }
        private void lAutoComplete_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lAutoComplete.ItemsSource != null)
            {
                this.lAutoComplete.Visibility = Visibility.Collapsed;
                this.cBusqueda.TextChanged -= new TextChangedEventHandler(this.cBusqueda_TextChanged);
                if (this.lAutoComplete.SelectedIndex != -1)
                {
                    this.cBusqueda.Text = this.lAutoComplete.SelectedItem.ToString();
                }
                this.cBusqueda.TextChanged += new TextChangedEventHandler(this.cBusqueda_TextChanged);
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
        //FILTRO SEGUN INFORMACION PERSONAL
        private void RadioButtonSearch(object sender, RoutedEventArgs e)
        {
            int parametroSearch = 0;
            if (this.rbRut.IsChecked == true) parametroSearch = 0;
            if (this.rbName.IsChecked == true) parametroSearch = 1;
            if (this.rbSurname.IsChecked == true) parametroSearch = 2;
            if (this.rbPhone.IsChecked == true) parametroSearch = 3;
            if (this.rbAdress.IsChecked == true) parametroSearch = 4;
            if (this.rbEmail.IsChecked == true) parametroSearch = 5;
            this.listAutocomplet = new Clases.Personal().findAll(parametroSearch);
        }

 /*>>>>VALIDACIONES<<<<<*/
        public void ClearContract() {
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
            concadenacion += path.Content.ToString().Equals("1")? "*Ingrese una foto de perfil para continuar el registro" + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.tEmail.Text.Trim()) ? "*Ingrese el correo electronico para completar el registro" + System.Environment.NewLine : "";
            concadenacion += validacion.validaFecha(this.tDateNaci.Text.Trim()) ? "" : "*Formato de fecha nacimiento invalido." + System.Environment.NewLine;
            concadenacion += string.IsNullOrEmpty(this.tDateNaci.Text.Trim()) ? "*Ingrese fecha nacimiento para completar el registro." + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.Regi.Text.Trim()) ? "*Seleccione una region de residencia." + System.Environment.NewLine : "";
            concadenacion += string.IsNullOrEmpty(this.Comu.Text.Trim()) ? "*Seleccione una comuna de residencia." + System.Environment.NewLine : "";
            validacion.validaRut(this.tRut.Text, this.tRut);
            //concadenacion += Rutok == true ? "" : "*Ingrese un rut valido." + System.Environment.NewLine;
            concadenacion += tEmail.Text.Trim().Length > 0 ? validacion.validaEmail(tEmail.Text.Trim()) ? "" : "*Correo electronico mal escrito, verifiquelo para continuar." + System.Environment.NewLine : "*Ingrese el correo electronico para completar el registro." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0){
                new Dialog(concadenacion).Show();
                ok = false;
            }
            return ok;
        }
        public Boolean validacionAddContract() {
            String concadenacion = validacion.validaFecha(this.tDateInit.Text.Trim()) ? "" : "*Formato fecha de inicio no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tDateInit.Text.Trim()) == true ? "" : "*Ingrese una fecha de inicio." + System.Environment.NewLine;
            concadenacion += validacion.validaFecha(this.tDateEnd.Text.Trim()) ? "" : "*Formato de termino no valida." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tDateEnd.Text.Trim()) == true ? "" : "*Ingrese una fecha de termino." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.tStat.Text.Trim()) == true && (this.tStat.Text.Trim().ToUpper().Equals("VIGENTE") || this.tStat.Text.Trim().ToUpper().Equals("NO VIGENTE")) ? "" : "*Ingrese un estado de contrato VIGENTE/NO VIGENTE." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.cTypeContract.Text) == true ? "" : "*Seleccione un tipo de contrato." + System.Environment.NewLine;
            concadenacion += !string.IsNullOrEmpty(this.cCargo.Text) == true ? "" : "*Ingrese un cargo." + System.Environment.NewLine;
            concadenacion += DateTime.Compare(Convert.ToDateTime(this.tDateInit.Text), Convert.ToDateTime(this.tDateEnd.Text)) == -1 ? "" : "*Verifique que las fechas sean correctas." + System.Environment.NewLine;
            Boolean ok = true;
            if (concadenacion.Length > 0) { 
                new Dialog(concadenacion).Show();
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

/*>>>>>RELACIONADA CON LA VENTANA (MOVIMIENTOS, EVENTOS)>>>>*/
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
            this.image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/CloseRed.png"));
        }              
        private void ColorNormal(object sender, MouseEventArgs e)
        {
            this.image2.Source = new BitmapImage(new Uri("pack://application:,,,/Images/Close2.png"));
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
            foreach(Regiones regiones in new Regiones().findAll()) this.Regi.Items.Add(regiones.nombre);
        }

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

        private void bAcceder_Click(object sender, RoutedEventArgs e)
        {
            animacionLogeo.Begin();
            animacionPresentacion.Begin();
        }

        private void cargarBanco(object sender, MouseButtonEventArgs e)
        {
            this.tBank.Items.Clear();
            foreach (Banco banco in new Banco().findAll()) this.tBank.Items.Add(banco.nombre);
        }

        private void Comu_QueryCursor(object sender, QueryCursorEventArgs e)
        {

        }

        private void verpaises(object sender, MouseButtonEventArgs e)
        {
            List<String> resul = new Clases.PDF().leerpaises();
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

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {








        }

 
        /*>>>>>FIN RELACIONADA CON LA VENTANA (MOVIMIENTOS, EVENTOS)>>>>*/

        /*>>>>FIN VALIDACIONES<<<<<*/
    }

}
