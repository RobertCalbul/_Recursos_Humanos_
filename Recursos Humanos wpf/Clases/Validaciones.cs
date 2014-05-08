using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace Recursos_Humanos_wpf.Clases
{
    class Validaciones
    {
        public Validaciones() { }

        public string validaRut(string value, TextBox tRut)
        {
            string rut = value.Replace(" ", "").Replace(".", "").Replace("-", "").ToUpper();
            int[] multiplos = { 2, 3, 4, 5, 6, 7, 2, 3 };
            int suma = 0;
            string digito = "";
            int lengthRut = rut.Length - 1;
            int contador = 0;
            tRut.Foreground = rut.Length == 9 ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2d2d2d")) : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DD4337"));

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
        public  bool validaEmail(string email)
        {
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            return Regex.IsMatch(email, expresion) ? Regex.Replace(email, expresion, String.Empty).Length == 0 ? true : false : false;
        }
        public Boolean validaFecha(string fecha)
        {
            string expresion = "^\\d{4}-\\d{2}-\\d{2}";
            return Regex.IsMatch(fecha, expresion) ? Regex.Replace(fecha, expresion, String.Empty).Length == 0 ? true : false : false;
        }
        public void validaNumeros(TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 48 && ascci <= 57 ? false : true;
        }
        public String DateFormat(String value)
        {
            return value.Substring(6, 4) + "-" + value.Substring(3, 2) + "-" + value.Substring(0, 2);
        }
        public void validaString(TextCompositionEventArgs e)
        {
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
            e.Handled = ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 ? false : true;
        }
    }
    
}
