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

        public void CrearArchivoXML(String Ruta, String rut, String fecha_ini, String nombre,
            String dire, String cargo, String departamento, String tipo_contrato, int sueldo,
            String fecha_ter, String afp, String salud)
        {

            {
                try
                {
                    MessageBox.Show("" + Ruta + " " + rut + " " + fecha_ini + " " + fecha_ini + " " + nombre + dire + " " + cargo + " " + departamento 
                        + " " + tipo_contrato + " " + sueldo + " " + fecha_ter + " " + afp + " " + salud);
                    DateTime thisDay = DateTime.Today;
                    System.Xml.Linq.XDocument miXML = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("datos de contrato"),
                    new XElement("Tipos_de_contrato",
                           new XElement("administrativo",
                               new XAttribute("Titulo", "Contrato de trabajo"),
                               new XAttribute("primer_parrafo", "En temuco a" + thisDay.ToString("D") + ",entre" +
                               "Tiendas de musica azalea ltda. con domicilio en Brasil #1429 pueblo nuevo, en la"
                               + " cuidad de Temuco y don(a): " + nombre + " de nacionalidad Chilena "
                               + " nacido el _____________________________________, domiciliado en"
                               + " ___________________________________ , de estado civil soltero"
                               + " y procedente de " + dire + " se ha convenido el siguiente"
                               + "CONTRATO DE TRABAJO, para cuyos efectos las partes convienen"
                               + "denominarse, respectivamente, EMPLEADOR Y TRABAJADOR."),
                               new XAttribute("segundo_parrado",
                               "1- El Trabajador se compromete a ejecutar el trabajo de"
                               + " " + cargo + " en el establecimiento u departamento"
                               + " " + departamento + " denominado(a) _____________________y"
                               + " ubicado(a)_________________________ en pudiendo ser trasladado a"
                               + " otro domicilio o labores similares, dentro de la ciudad por causa"
                               + "justificada, sin que ello importe menoscabo para el Trabajador."),
                               new XAttribute("tercer_parrafo",
                               "2- El Empleador se compromete a remunerar al Trabajador con la"
                               + " suma de __________________(________________________________"
                               + " pesos) como sueldo fijo por Mes además se asigna al Trabajador una"
                               + " comisión de __________________________. Las remuneraciones se"
                               + " pagarán Los 5 primeros días del Mes por 1 Mes periodos vencidos,"
                               + " en dinero efectivo, moneda nacional y del monto de ellas el"
                               + " Empleador hará las deducciones que establecen las leyes vigentes."),
                               new XAttribute("cuarto_parrafo",
                                   "3- El presente contrato durará ____________________________"
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
                                   "6- Se deja constancia que don(a) ________________________________"
                                   + " ingreso al servicio el _____________________"),
                               new XAttribute("octavo_parrafo",
                                   "7- El trabajador declara que su régimen previsional es el"

                                    + " siguiente:"

                                    + " REGIMEN DE PENSIONES:"

                                    + " a) Régimen antiguo ______________ b) A.F.P.__________________."

                                    + " REGIMEN DE SALUD:"

                                    + " a) FONASA ____________________ b) ISAPRE ____________________"),
                               new XAttribute("noveno_parrafo",
                                   "_____________________ ______________________"

                                   + " Firma del Empleador Firma del Trabajador"),
                               new XAttribute("fecha_inicio", fecha_ini),
                               new XAttribute("rut", rut),
                               new XElement("nombrecompleto_personal", nombre),
                               new XElement("direccion", dire),
                               new XElement("cargo", cargo),
                               new XElement("departamento", departamento),
                               new XElement("tipo_contrato", tipo_contrato),
                               new XElement("sueldo", sueldo.ToString()),
                               new XElement("fecha_termino", fecha_ter),
                               new XElement("afp", afp),
                               new XElement("salud", salud))
              ));

                    miXML.Save(@Ruta);

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
                MessageBox.Show(nodo.GetAttribute("primer_parrafo"));
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
    }



}
