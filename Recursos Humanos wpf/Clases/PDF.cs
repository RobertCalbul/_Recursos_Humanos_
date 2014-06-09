using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Xml;

//Para el manejo de Archivos
using System.IO;

//Clases necesarias de iTextSharp
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Windows;


namespace Recursos_Humanos_wpf.Clases
{
    class PDF
    {
        public bool CrearCarpetaXml(string Ruta)
        {
            {
                bool Respuesta = false;
                try
                {
                    if (Directory.Exists(Ruta))
                    {
                        Respuesta = true;
                    }
                    else
                    {
                        Directory.CreateDirectory(Ruta);
                        Respuesta = true;
                    }
                    return Respuesta;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en CrearCarpetaXml, ClaseXml:" + ex.Message);
                    Console.Write("Error en CrearCarpetaXml, ClaseXml:" + ex.Message);
                    return Respuesta;
                    //No fue posible crear el directorio...
                }

            }
        }// fin del metodo CrearCarpetaXml

        public string enletras(string num)
        {

            string res, dec = "";

            Int64 entero;

            int decimales;

            double nro;

            try
            {

                nro = Convert.ToDouble(num);

            }

            catch
            {

                return "";

            }

            entero = Convert.ToInt64(Math.Truncate(nro));

            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

            if (decimales > 0)
            {

                dec = " CON " + decimales.ToString() + "/100";

            }

            res = toText(Convert.ToDouble(entero)) + dec;

            return res;

        }

        private string toText(double value)
        {

            string Num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) Num2Text = "CERO";

            else if (value == 1) Num2Text = "UNO";

            else if (value == 2) Num2Text = "DOS";

            else if (value == 3) Num2Text = "TRES";

            else if (value == 4) Num2Text = "CUATRO";

            else if (value == 5) Num2Text = "CINCO";

            else if (value == 6) Num2Text = "SEIS";

            else if (value == 7) Num2Text = "SIETE";

            else if (value == 8) Num2Text = "OCHO";

            else if (value == 9) Num2Text = "NUEVE";

            else if (value == 10) Num2Text = "DIEZ";

            else if (value == 11) Num2Text = "ONCE";

            else if (value == 12) Num2Text = "DOCE";

            else if (value == 13) Num2Text = "TRECE";

            else if (value == 14) Num2Text = "CATORCE";

            else if (value == 15) Num2Text = "QUINCE";

            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);

            else if (value == 20) Num2Text = "VEINTE";

            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);

            else if (value == 30) Num2Text = "TREINTA";

            else if (value == 40) Num2Text = "CUARENTA";

            else if (value == 50) Num2Text = "CINCUENTA";

            else if (value == 60) Num2Text = "SESENTA";

            else if (value == 70) Num2Text = "SETENTA";

            else if (value == 80) Num2Text = "OCHENTA";

            else if (value == 90) Num2Text = "NOVENTA";

            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);

            else if (value == 100) Num2Text = "CIEN";

            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";

            else if (value == 500) Num2Text = "QUINIENTOS";

            else if (value == 700) Num2Text = "SETECIENTOS";

            else if (value == 900) Num2Text = "NOVECIENTOS";

            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);

            else if (value == 1000) Num2Text = "MIL";

            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);

            else if (value < 1000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);

            }

            else if (value == 1000000) Num2Text = "UN MILLON";

            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);

            else if (value < 1000000000000)
            {

                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);

            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";

            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {

                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            }

            return Num2Text;

        }




        public void CrearArchivoXML(String Ruta, String rut, String fecha_ini, String nombre,
            String dire, String cargo, String departamento, String tipo_contrato, int sueldo,
            String fecha_ter, String afp, String salud)
        {

            {
                try
                {
                    
                    DateTime thisDay = DateTime.Today;
                    System.Xml.Linq.XDocument miXML = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("datos de contrato"),
                    new XElement("Tipos_de_contrato",
                           new XElement("administrativo",
                               new XAttribute("Titulo", "Contrato de trabajo"),
                               new XAttribute("primer_parrafo",
                               "En TEMUCO a " + thisDay.ToString("D") + ", entre don(a)"
                                + " Tiendas de musica azalea ltda. Con domicilio en LAS NIEVES 766 en"
                                + " la ciudad de TEMUCO, representada legalmente por don (a) JORGE"
                                + " HUENCHUÑIR DIAZ, R.U.T. 7.839.780-2 y don(a) " + nombre + " "
                                + " Con rut " + rut + " de nacionalidad Chilena "
                                + "domiciliado en " + dire + " se ha convenido el siguiente"
                                + " CONTRATO DE TRABAJO, para cuyos efectos las partes convienen"
                                + " denominarse, respectivamente, EMPLEADOR Y TRABAJADOR."),
                               new XAttribute("segundo_parrafo",
                               "1- El Trabajador se compromete a ejecutar el trabajo de"
                               + " " + cargo + " en el establecimiento u departamento"
                               + " " + departamento + " bajo el tipo de contrato " + tipo_contrato + " "
                               + " en conocimiento que puede ser trasladado a"
                               + " otro domicilio o labores similares, dentro de la ciudad por causa"
                               + " justificada, sin que ello importe menoscabo para el Trabajador."),
                               new XAttribute("tercer_parrafo",
                               "2- El Empleador se compromete a remunerar al Trabajador con la"
                               + " suma de " + enletras(sueldo.ToString())
                               + " pesos como sueldo BASE por Mes además se asigna al Trabajador una"
                               + " comisión de __________________________. Las remuneraciones se"
                               + " pagarán Los 5 primeros días del Mes por 1 Mes periodos vencidos,"
                               + " en dinero efectivo, moneda nacional y del monto de ellas el"
                               + " Empleador hará las deducciones que establecen las leyes vigentes."),
                               new XAttribute("cuarto_parrafo",
                                   "3- El presente contrato durará haste el " + fecha_ter + " "
                                   + " y podrá ponérsele término cuando concurran para ello causas"
                                   + " justificadas que, en conformidad a la ley, puedan producir su"
                                   + " caducidad, o sea permitido dar al Trabajador el aviso de desahucio"
                                   + " con 30 días de anticipación, a lo menos."),
                               new XAttribute("quinto_parrafo",
                                   "4- Se pagará la gratificación en base del 25% de las"
                                   + " remuneraciones, con tope de 4,75% de Ingresos Mínimos Mensuales,"
                                   + " los cuales se darán en calidad de anticipos."),
                               new XAttribute("sexto_parrafo",
                                   "5- Se entienden incorporadas al presente contrato todas las"
                                    + " disposiciones que se dicten con posterioridad a la fecha de"
                                    + " suscripción y que tengan relación con Él."),
                               new XAttribute("septimo_parrafo",
                                   "6- Se deja constancia que don(a) " + nombre + " "
                                   + " ingreso al servicio el " + fecha_ini + " "),
                               new XAttribute("octavo_parrafo",
                                   "7- El trabajador declara que su régimen previsional es el siguiente: \n"
                                    + " REGIMEN DE PENSIONES: \n"
                                    + "\n"
                                    + "\n"
                                    + " a) Régimen antiguo ______________ b) A.F.P.______" + afp + "_________. \n"
                                    + "\n"
                                    + "\n"
                                    + " REGIMEN DE SALUD: \n"
                                    + "\n"
                                    + "\n"
                                    + " a) FONASA o ISAPRE.___________" + salud + "____________\n"),
                               new XAttribute("noveno_parrafo",
                                   "\n"
                                   + "\n"
                                   + "\n"
                                   + "\n"
                                   + " _____________________ ______________________ \n"

                                   + " Firma del Empleador            Firma del Trabajador"))
              ));

                    miXML.Save(Ruta);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    Console.Write("Error: " + ex.Message);

                }
            }
        } // fin del metodo ArchivoExiste


        public List<String> leer(String tipo)
        {
            List<String> nodos = new List<String>();
            XmlDocument xDoc = new XmlDocument();

            //La ruta del documento XML permite rutas relativas 
            //respecto del ejecutable!

            xDoc.Load("contratos/contract.xml");

            XmlNodeList personas = xDoc.GetElementsByTagName("Tipos_de_contrato");

            XmlNodeList lista =
                ((XmlElement)personas[0]).GetElementsByTagName(tipo);
            
            foreach (XmlElement nodo in lista)
            {
                string nNombre = nodo.GetAttribute("Titulo");
                //.GetElementsByTagName("nombre");
                string p1 = nodo.GetAttribute("primer_parrafo");
                string p2 = nodo.GetAttribute("segundo_parrafo");
                string p3 = nodo.GetAttribute("tercer_parrafo");
                string p4 = nodo.GetAttribute("cuarto_parrafo");
                string p5 = nodo.GetAttribute("quinto_parrafo");
                string p6 = nodo.GetAttribute("sexto_parrafo");
                string p7 = nodo.GetAttribute("septimo_parrafo");
                string p8 = nodo.GetAttribute("octavo_parrafo");
                string p9 = nodo.GetAttribute("noveno_parrafo");


                nodos.Add(nNombre);
                nodos.Add(p1);
                nodos.Add(p2);
                nodos.Add(p3);
                nodos.Add(p4);
                nodos.Add(p5);
                nodos.Add(p6);
                nodos.Add(p7);
                nodos.Add(p8);
                nodos.Add(p9);
            }
            return nodos;
        } //fin del metodo

        public List<String> leerpaises()
        {
            List<String> nodos = new List<String>();
            XmlTextReader reader = new XmlTextReader("contratos/country.xml");
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            if (reader.Read())
                            {
                                nodos.Add(reader.Value);

                            }
                            break;
                    }

                }
            }
            return nodos;
        } //fin del metodo


    }



}
