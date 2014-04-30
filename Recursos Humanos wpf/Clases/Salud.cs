using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Salud
    {
        public String name_salud { get; set; }
        public double desc { get; set; }
        MySqlConnection conex = null;
        public Salud() { }

        public Salud(String name_salud, double desc){
            this.name_salud = name_salud;
            this.desc = desc;
        }

        public int save() {
            try {

                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("INSERT INTO salud (nombre,descuento) VALUES ('{0}','{1}')",
                    this.name_salud, this.desc), conex);
                return comando.ExecuteNonQuery();
            }catch(Exception e){
                return 0;
            }
        }
    }
}
