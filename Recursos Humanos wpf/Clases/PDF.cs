using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
//Para el manejo de Archivos

//Clases necesarias de iTextSharp


namespace Recursos_Humanos_wpf.Clases
{
    internal class Pdf
    {
        public bool CrearCarpetaXml(string ruta)
        {
            {
                var respuesta = false;
                try
                {
                    if (Directory.Exists(ruta))
                    {
                        respuesta = true;
                    }
                    else
                    {
                        Directory.CreateDirectory(ruta);
                        respuesta = true;
                    }
                    return respuesta;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en CrearCarpetaXml, ClaseXml:" + ex.Message);
                    Console.Write("Error en CrearCarpetaXml, ClaseXml:" + ex.Message);
                    return respuesta;
                    //No fue posible crear el directorio...
                }
            }
        } // fin del metodo CrearCarpetaXml

        public string Enletras(string num)
        {
            string res, dec = "";

            long entero;

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

            decimales = Convert.ToInt32(Math.Round((nro - entero)*100, 2));

            if (decimales > 0)
            {
                dec = " CON " + decimales + "/100";
            }

            res = ToText(Convert.ToDouble(entero)) + dec;

            return res;
        }

        private string ToText(double value)
        {
            var num2Text = "";

            value = Math.Truncate(value);

            if (value == 0) num2Text = "CERO";

            else if (value == 1) num2Text = "UNO";

            else if (value == 2) num2Text = "DOS";

            else if (value == 3) num2Text = "TRES";

            else if (value == 4) num2Text = "CUATRO";

            else if (value == 5) num2Text = "CINCO";

            else if (value == 6) num2Text = "SEIS";

            else if (value == 7) num2Text = "SIETE";

            else if (value == 8) num2Text = "OCHO";

            else if (value == 9) num2Text = "NUEVE";

            else if (value == 10) num2Text = "DIEZ";

            else if (value == 11) num2Text = "ONCE";

            else if (value == 12) num2Text = "DOCE";

            else if (value == 13) num2Text = "TRECE";

            else if (value == 14) num2Text = "CATORCE";

            else if (value == 15) num2Text = "QUINCE";

            else if (value < 20) num2Text = "DIECI" + ToText(value - 10);

            else if (value == 20) num2Text = "VEINTE";

            else if (value < 30) num2Text = "VEINTI" + ToText(value - 20);

            else if (value == 30) num2Text = "TREINTA";

            else if (value == 40) num2Text = "CUARENTA";

            else if (value == 50) num2Text = "CINCUENTA";

            else if (value == 60) num2Text = "SESENTA";

            else if (value == 70) num2Text = "SETENTA";

            else if (value == 80) num2Text = "OCHENTA";

            else if (value == 90) num2Text = "NOVENTA";

            else if (value < 100) num2Text = ToText(Math.Truncate(value/10)*10) + " Y " + ToText(value%10);

            else if (value == 100) num2Text = "CIEN";

            else if (value < 200) num2Text = "CIENTO " + ToText(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800))
                num2Text = ToText(Math.Truncate(value/100)) + "CIENTOS";

            else if (value == 500) num2Text = "QUINIENTOS";

            else if (value == 700) num2Text = "SETECIENTOS";

            else if (value == 900) num2Text = "NOVECIENTOS";

            else if (value < 1000) num2Text = ToText(Math.Truncate(value/100)*100) + " " + ToText(value%100);

            else if (value == 1000) num2Text = "MIL";

            else if (value < 2000) num2Text = "MIL " + ToText(value%1000);

            else if (value < 1000000)
            {
                num2Text = ToText(Math.Truncate(value/1000)) + " MIL";

                if ((value%1000) > 0) num2Text = num2Text + " " + ToText(value%1000);
            }

            else if (value == 1000000) num2Text = "UN MILLON";

            else if (value < 2000000) num2Text = "UN MILLON " + ToText(value%1000000);

            else if (value < 1000000000000)
            {
                num2Text = ToText(Math.Truncate(value/1000000)) + " MILLONES ";

                if ((value - Math.Truncate(value/1000000)*1000000) > 0)
                    num2Text = num2Text + " " + ToText(value - Math.Truncate(value/1000000)*1000000);
            }

            else if (value == 1000000000000) num2Text = "UN BILLON";

            else if (value < 2000000000000)
                num2Text = "UN BILLON " + ToText(value - Math.Truncate(value/1000000000000)*1000000000000);

            else
            {
                num2Text = ToText(Math.Truncate(value/1000000000000)) + " BILLONES";

                if ((value - Math.Truncate(value/1000000000000)*1000000000000) > 0)
                    num2Text = num2Text + " " + ToText(value - Math.Truncate(value/1000000000000)*1000000000000);
            }

            return num2Text;
        }

        public void CrearArchivoXml(string ruta, string rut, string fechaIni, string nombre,
            string dire, string cargo, string departamento, string tipoContrato, int sueldo,
            string fechaTer, string afp, string salud)
        {

            {
                try
                {
                    var thisDay = DateTime.Today;
                    var miXml = new XDocument(
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
                                    + " " + departamento + " bajo el tipo de contrato " + tipoContrato + " "
                                    + " en conocimiento que puede ser trasladado a"
                                    + " otro domicilio o labores similares, dentro de la ciudad por causa"
                                    + " justificada, sin que ello importe menoscabo para el Trabajador."),
                                new XAttribute("tercer_parrafo",
                                    "2- El Empleador se compromete a remunerar al Trabajador con la"
                                    + " suma de " + Enletras(sueldo.ToString())
                                    + " pesos como sueldo BASE por Mes además se asigna al Trabajador una"
                                    + " comisión de __________________________. Las remuneraciones se"
                                    + " pagarán Los 5 primeros días del Mes por 1 Mes periodos vencidos,"
                                    + " en dinero efectivo, moneda nacional y del monto de ellas el"
                                    + " Empleador hará las deducciones que establecen las leyes vigentes."),
                                new XAttribute("cuarto_parrafo",
                                    "3- El presente contrato durará haste el " + fechaTer + " "
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
                                    + " ingreso al servicio el " + fechaIni + " "),
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

                    miXml.Save(ruta);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    Console.Write("Error: " + ex.Message);
                }
            }
        } // fin del metodo ArchivoExiste

        public List<string> Leer(string tipo)
        {
            var nodos = new List<string>();
            var xDoc = new XmlDocument();

            //La ruta del documento XML permite rutas relativas 
            //respecto del ejecutable!

            xDoc.Load("contratos/contract.xml");

            var personas = xDoc.GetElementsByTagName("Tipos_de_contrato");

            var lista =
                ((XmlElement) personas[0]).GetElementsByTagName(tipo);

            foreach (XmlElement nodo in lista)
            {
                var nNombre = nodo.GetAttribute("Titulo");
                //.GetElementsByTagName("nombre");
                var p1 = nodo.GetAttribute("primer_parrafo");
                var p2 = nodo.GetAttribute("segundo_parrafo");
                var p3 = nodo.GetAttribute("tercer_parrafo");
                var p4 = nodo.GetAttribute("cuarto_parrafo");
                var p5 = nodo.GetAttribute("quinto_parrafo");
                var p6 = nodo.GetAttribute("sexto_parrafo");
                var p7 = nodo.GetAttribute("septimo_parrafo");
                var p8 = nodo.GetAttribute("octavo_parrafo");
                var p9 = nodo.GetAttribute("noveno_parrafo");


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

        public List<string> Leerpaises()
        {
            var nodos = new List<string>();
            var reader = new XmlTextReader("contratos/country.xml");
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