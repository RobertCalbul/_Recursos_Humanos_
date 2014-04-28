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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Win32;
using System.IO;
using Recursos_Humanos_wpf.Clases;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using Microsoft.VisualStudio.DebuggerVisualizers;


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
        public MainWindow()
        {
            InitializeComponent();
            listAutocomplet = new Clases.Personal().findAll(0);
            pInfo.IsEnabled = false;
            rbRut.IsChecked = true;
        }
/*>>>>INICIO OPERACIONES CRUD EMPLEADOS<<<<<*/
        //BUSCA DATOS EMPLEADOS POR FILTRO
        private void btnBuscar_click(object sender, MouseButtonEventArgs e)
        {
            Search();
        }
        private void Search(){
          try
            {
                String busqueda = cBusqueda.Text.Trim().Equals("") ? tRut.Text.Trim() : cBusqueda.Text.Trim();
                if (!busqueda.Equals(""))
                {
                    limpiarTexbox();
                    if (rbRut.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "rut");
                    else if (rbName.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "nombre");
                    else if (rbSurname.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "apellido");
                    else if (rbPhone.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "telefono");
                    else if (rbAdress.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "direccion");
                    else if (rbEmail.IsChecked == true) cargarDatosPersonal(cBusqueda.Text.Trim(), "email");
                }else
                {
                    cBusqueda.Focus();//doy el foco al cuadro de busqueda
                    MessageBox.Show("Ingrese un parametro de búsqueda");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnBuscar_click() " + ex.Message.ToString());
            }
        }
        //CARGA DATOS PERSONALES BASICOS
        private void cargarDatosPersonal(String value, String paramSearch)
        {
            if (!value.Equals(""))
            {
                pInfo.IsEnabled = true;
                object[] arreglo = new Clases.Personal().findBy(value, paramSearch);
                if (arreglo != null)
                {
                    tRut.Text = valida_Rut(arreglo[1].ToString());
                    lName.Content = arreglo[2].ToString();
                    tName.Text = arreglo[2].ToString();
                    tSurname.Text = arreglo[3].ToString();
                    tYear.Text = arreglo[4].ToString();
                    tPhone.Text = arreglo[5].ToString();
                    tAdress.Text = arreglo[6].ToString();
                    tEmail.Text = arreglo[7].ToString();
                    tCtaBancaria.Text = arreglo[8].ToString();
                    cAfp.Items.Add(arreglo[9].ToString());
                    cSalud.Items.Add(arreglo[10].ToString());
                    cDepto.Items.Add(arreglo[11].ToString());
                    cAfp.SelectedItem = arreglo[9].ToString();
                    cSalud.SelectedItem = arreglo[10].ToString();
                    cDepto.SelectedItem = arreglo[11].ToString();
                } loadDataContract(tRut.Text.Trim());
            }
        }
        //INGRESA NUEVO USUARIO
        private void btnAddUser_Click(object sender, MouseButtonEventArgs e)
        {
            tRut.IsEnabled = true;
            iAddUser.IsEnabled = true;
             if  (validacionAddUser())
            {
                MessageBoxResult dialogResult = MessageBox.Show("Desea agregar a esta persona?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    byte[] foto = File.ReadAllBytes(path.Content.ToString());

                    Clases.Personal per = new Clases.Personal(tRut.Text.Trim(), tName.Text.Trim(), tSurname.Text.Trim(), 
                                            int.Parse(tYear.Text.Trim()), foto, tPhone.Text.Trim(),tAdress.Text.Trim(),
                                            tEmail.Text.Trim(), tCtaBancaria.Text.Trim(), int.Parse(cAfp.Text.Split(':')[0]),
                                            int.Parse(cSalud.Text.Split(':')[0]), int.Parse(cDepto.Text.Split(':')[0]));
                    if (per.Save() > 0)
                    {   
                        MessageBox.Show("Personal guardado con exito", "Registro agregado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
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
                    }
                    else { MessageBox.Show("no guardo"); }
                }
            }
        }
        //ACTUALIZA DATOS EMPLEADO
        private void btnUpdateReg_Click(object sender, MouseButtonEventArgs e)
        {
            Clases.Personal personal = new Clases.Personal(tRut.Text.Trim(), tName.Text.Trim(), 
                                       tSurname.Text.Trim(), int.Parse(tYear.Text.Trim()),
                                       tPhone.Text.Trim(),tAdress.Text.Trim(), tEmail.Text.Trim(),
                                       tCtaBancaria.Text.Trim(), int.Parse(cAfp.Text.Split(':')[0]), 
                                       int.Parse(cSalud.Text.Split(':')[0]), int.Parse(cDepto.Text.Split(':')[0]));

            if (personal.Update() > 0) {
                Search();
                MessageBox.Show("actualizacion ok"); }
        }
        //CANCELA INGRESO EMPLEADO
        private void btnCancelAdd_Click(object sender, MouseButtonEventArgs e)
        {
            Label[] labels = { this.btnAddAfp, this.btnAddSalud, this.btnAddUser, this.btnCancelAdd };
            foreach (Label x in labels) x.Visibility = Visibility.Hidden;
            this.pInfo.IsEnabled = false;
            this.cBusqueda.IsEnabled = true;
            this.btnUpdateReg.Visibility = Visibility.Visible;
            this.btnDeleteReg.Visibility = Visibility.Visible;
            this.image1.IsEnabled = true;
            limpiarTexbox();
        }
        //ELIMINA UN EMPLEADO
        private void btnDeleteReg_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                String rut_per = tRut.Text.Trim();
                MessageBoxResult dialogResult = MessageBox.Show("Desea borrar el personal con rut: " + rut_per + " ?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    if (new Clases.Personal(rut_per).DeleteByRrut() > 0)
                    {
                        limpiarTexbox();
                        cBusqueda.Text = "";
                        MessageBox.Show("El personal borrado correctamente", "Registro eliminado", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    }
                    else MessageBox.Show("que paso");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en eliminar personal" );
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
                    MessageBox.Show("Se elimino el contrato con exito"); 
                }
            }
        }
        //CARGA CONTRATOS DE UN EMPLEADO ESPECIFICO
        private void loadDataContract(string value)
        {
            string sql = "SELECT e.fecha_inicio,e.fecha_termino,e.estado,"
            + " (SELECT c.tipo AS tipo_contrato"
            + " FROM personal AS p"
            + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
            + " INNER JOIN tipo_contrato AS c ON(e.tipo_contrato_id_tipo_contrato=c.id_tipo_contrato)"
            + " WHERE p.rut ='" + value + "') AS tipo_contrato, "
            + " IFNULL((SELECT f.cargo AS nombre_cargo"
            + " FROM personal AS p"
            + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
            + " INNER JOIN cargo AS f ON(f.id_cargo=e.cargo_id_cargo)"
            + " WHERE p.rut ='" + value + "'),'1')AS cargo "
            + " FROM personal AS p"
            + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
            + " INNER JOIN cargo AS f ON(f.id_cargo=e.cargo_id_cargo)"
            + " WHERE nombre = '" + value + "' OR apellido ='" + value + "' OR rut ='" + value + "'";
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
            lDescription.Background = interfaces == "1" || interfaces == "" ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#dd4337")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4185f4"));
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
                    Clases.Contratos contrato = new Clases.Contratos(tRut.Text, tDateInit.Text, tName.Text + " " + tSurname.Text, tAdress.Text,
                                                            cCargo.Text.Trim().Split(':')[1], cDepto.Text.Trim().Split(':')[1],
                                                            cTypeContract.Text.Trim().Split(':')[1], 25000, tDateEnd.Text,
                                                            cAfp.Text.Split(':')[1], cSalud.Text.Split(':')[1]
                        );
                    MessageBox.Show(contrato.rut + contrato.fInicio + contrato.nombre_completo + contrato.direccion + contrato.Cargo + contrato.depto + contrato.tContrato +
                    contrato.SueldoBase + contrato.fTermino + contrato.afp + contrato.salud);
                    new Clases.PDF().CrearArchivoXML("contratos/contract.xml",
                    contrato.rut, contrato.fInicio, contrato.nombre_completo, contrato.direccion, contrato.Cargo, contrato.depto, contrato.tContrato,
                    contrato.SueldoBase, contrato.fTermino, contrato.afp, contrato.salud);
                    Document document = new Document();

                    List<String> resul = new Clases.PDF().leer(contrato.tContrato);

                    PdfWriter.GetInstance(document, new FileStream("contrato.pdf", FileMode.OpenOrCreate));
                    document.Open();
                    Chunk chunk = new Chunk(resul[0], FontFactory.GetFont("ARIAL", 12, iTextSharp.text.Font.UNDERLINE));

                    document.Add(new iTextSharp.text.Paragraph(chunk));
                    for (int i = 1; i < resul.Count; i++)
                    {
                        document.Add(new iTextSharp.text.Paragraph(resul[i]));
                    }
                    document.Close();

                    MessageBox.Show("Contrato generado con exito");
                    System.Diagnostics.Process.Start("contrato.pdf");
                }
                catch (Exception ex)
                {
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
                if (dialogResult == MessageBoxResult.Yes)
                {
                    Clases.Contratos contrato = new Clases.Contratos(rut_per, tDateInit.Text, tDateEnd.Text, tStat.Text,
                                                250000, cTypeContract.Text.Split(':')[0], cCargo.Text.Split(':')[0]);
                    if ( contrato.save() > 0)
                    {
                        loadDataContract(rut_per);
                        MessageBox.Show("Contrato ingresado exitosamente.");
                    }
                    else MessageBox.Show("Ocurrio un error al ingresar contrato");
                }
            }catch (Exception ex)
            {
                Trace.WriteLine("MainWindow.btnInsertNewContract_Click() " + ex.Message.ToString());
            }

        }
        //ABRE EL CALENDARIO
        private void btnDateInitCalendar_Click(object sender, MouseButtonEventArgs e)
        {
            flagCalendar = 0;
            calendar1.Visibility = Visibility.Visible;
            calendar1.Margin = new Thickness(259, 61, 0, 0);

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
            calendar1.Margin = new Thickness(259, 91, 0, 0);
        }
 /*>>>>FIN CRUD CONTRATOS<<<<<*/

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
                //else iPerfil.Source = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("El archivo seleccionado no es un tipo de imagen válido" + ex.Message);
            }
        }
        //CARGA DATOS DE AFP EN COMBOBOX
        private void cAfp_Click(object sender, MouseButtonEventArgs e)
        {
            cAfp.Items.Clear();
            String sql = "select id_afp,nombre from afp";
            DataTable dataTable = new Clases.Consultas().QueryDB(sql);
            foreach (DataRow dtRow in dataTable.Rows)
            {
                string rowz = string.Format("{0}:{1}", dtRow["id_afp"], dtRow["nombre"]);
                cAfp.Items.Add(rowz);
            }
        }
        //CARGA DATOS DE SALUD EN COMBOBOX
        private void cSalud_Click(object sender, MouseButtonEventArgs e)
        {
            cSalud.Items.Clear();
            String sql = "select id_salud,nombre from salud";
            DataTable dataTable = new Clases.Consultas().QueryDB(sql);
            foreach (DataRow dtRow in dataTable.Rows)
            {
                string rowz = string.Format("{0}:{1}", dtRow["id_salud"], dtRow["nombre"]);
                cSalud.Items.Add(rowz);
            }
        }
        //CARGA DATOS DE DEPARTAMENTO EN COMBOBOX
        private void cDepto_Click(object sender, MouseButtonEventArgs e)
        {
            cDepto.Items.Clear();
            String sql = "select id_departamento,nombre from departamento";
            DataTable dataTable = new Clases.Consultas().QueryDB(sql);
            foreach (DataRow dtRow in dataTable.Rows)
            {
                string rowz = string.Format("{0}:{1}", dtRow["id_departamento"], dtRow["nombre"]);
                cDepto.Items.Add(rowz);
                cDepto.SelectedIndex = 0;
            }
            
        }
        //CARGA LOS TIPOS DE CONTRATOS
        private void cTypeContract_Click(object sender, MouseButtonEventArgs e)
        {
            cTypeContract.Items.Clear();
            String sql = "select id_tipo_contrato,tipo from tipo_contrato";
            DataTable dataTable = new Clases.Consultas().QueryDB(sql);
            foreach (DataRow dtRow in dataTable.Rows)
            {
                string rowz = string.Format("{0}:{1}", dtRow["id_tipo_contrato"], dtRow["tipo"]);
                cTypeContract.Items.Add(rowz);
            }
        }
        //CARGA LOS DIFERENTE CARGOS
        private void cCargo_Click(object sender, MouseButtonEventArgs e)
        {
            cCargo.Items.Clear();
            String sql = "select id_cargo,cargo from cargo";
            DataTable dataTable = new Clases.Consultas().QueryDB(sql);
            foreach (DataRow dtRow in dataTable.Rows)
            {
                string rowz = string.Format("{0}:{1}", dtRow["id_cargo"], dtRow["cargo"]);
                cCargo.Items.Add(rowz);
            }
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
        //RESETEA LOS CAMPOS PARA INGRESAR UN NUEVO USUARIO
        private void iAddUser_Click(object sender, MouseButtonEventArgs e)
        {

            pInfo.IsEnabled = true;
            limpiarTexbox();
            this.tRut.Focus();
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
            loadDataContract("");
            //this.dDatos.ItemsSource = null;
        }


 /*>>>>VALIDACIONES<<<<<*/
        public void limpiarTexbox()
        {
            lPuesto.Content = "";
            lName.Content = "";
            TextBox[] campos = { tRut, tName, tSurname, tYear, tPhone, tAdress, tEmail, tCtaBancaria, tDateEnd, tDateInit, tStat };
            ComboBox[] combos = { cAfp, cDepto, cSalud, cTypeContract, cCargo };
            foreach (TextBox x in campos) x.Text = "";
            foreach (ComboBox x in combos) x.Items.Clear();
            
        }

        public void validaNumeros(TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 48 && ascci <= 57? false : true;
           // salida.Content = ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 ? "" : "Ingrese solo numeros";
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

        public Boolean validacionAddUser() {
            //verficamos si hay foto en el picturebox
            Boolean lleno = false;
            String concadenacion = "";
            String[] campos = { tRut.Text.Trim(), tName.Text.Trim(), tSurname.Text.Trim(), tYear.Text.Trim(), tPhone.Text.Trim(), tAdress.Text.Trim(), tEmail.Text.Trim(), tCtaBancaria.Text.Trim(), cAfp.Text, cSalud.Text, cDepto.Text };
            concadenacion += string.IsNullOrEmpty(tRut.Text.Trim())?"*Ingrese el rut para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tName.Text.Trim())?"*Ingrese el nombre para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tSurname.Text.Trim())?"*Ingrese el apellido para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tYear.Text.Trim())?"*Ingrese la edad para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tPhone.Text.Trim())?"*Ingrese el nro. telefonico para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tAdress.Text.Trim())?"*Ingrese la direccion para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tEmail.Text.Trim())?"*Ingrese el correo electronico para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(tCtaBancaria.Text.Trim())?"*ingrese el n. de cta bancaria para completar el registro" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(cAfp.Text)?"*Ingrese una afp del menu desplegable para continuar" + System.Environment.NewLine:""; 
            concadenacion += string.IsNullOrEmpty(cSalud.Text)?"*Ingrese un registro de salud del menu desplegable para continuar" + System.Environment.NewLine:"";
            concadenacion += string.IsNullOrEmpty(cDepto.Text)?"*Ingrese un dpto del menu desplegable para continuar" + System.Environment.NewLine:"";           
            concadenacion += path.Content.ToString().Equals("1")?"*Ingrese una foto de perfil para continuar el registro" + System.Environment.NewLine:"";
            valida_Rut(tRut.Text);

            foreach (string x in campos) lleno = string.IsNullOrEmpty(x) || path.Content.ToString().Equals("1") ? false : true;

            lleno = lleno && Rutok;
            concadenacion += Rutok == true ? "" : "Ingrese un rut valido";
            if(!concadenacion.Equals("")) MessageBox.Show(concadenacion);
            return lleno;
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



        



/*>>>>FIN VALIDACIONES<<<<<*/
  
    }

}
