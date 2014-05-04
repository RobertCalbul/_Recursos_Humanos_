using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Afp
    {
        MySqlConnection conex = null;
        public int id { get; set; }
        public string nombre_afp { get; set; }
        public double descuento { get; set; }

        public Afp(){}

        public Afp(int id, string nombre_afp, double descuento)
        {
            this.id = id;
            this.nombre_afp = nombre_afp;
            this.descuento = descuento;
        }
        public Afp(String nombre_afp, double descuento)
        {
            this.nombre_afp = nombre_afp;
            this.descuento = descuento;
        }
        
        public List<Afp> findAll()
        {
            List<Afp> list = null;
            try {
                String sql = "select * from afp";
                Afp afp = null;
                list = new List<Afp>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    afp = new Afp(int.Parse(dtRow["id_afp"].ToString()), dtRow["nombre"].ToString(), double.Parse(dtRow["descuento"].ToString()));
                    list.Add(afp);
                }return list;
            }catch(Exception e)
             {
                 Console.Write("error: " + e.Message);
                return list;
             }
        }

        public int save(){
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("insert into afp (nombre,descuento) values ('{0}','{1}')",
                    this.nombre_afp, this.descuento), conex);
                return comando.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
                return 0;
            }
            finally {
                conex.Close();
            }
        }
    }
}
