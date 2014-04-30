using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Afp
    {
        MySqlConnection conex = null;
        public String nombre_apf { get; set; }
        public double descuento { get; set; }

        public Afp(){}

        public Afp(String nombre_afp, double descuento){
            this.nombre_apf = nombre_apf;
            this.descuento = descuento;
        }

        public int save(){
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("insert into afp (nombre,descuento) values ('{0}','{1}')",
                    this.nombre_apf, this.descuento), conex);
                return comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {
               
                return 0;
            }
            finally {
                conex.Close();
            }
        }
    }
}
