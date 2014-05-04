using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Salud
    {
        public int id {get; set;}
        public String name_salud { get; set; }
        public double desc { get; set; }
        MySqlConnection conex = null;
        public Salud() { }
        public Salud(int id, string name_salud, double desc)
        {
            this.id = id;
            this.name_salud = name_salud;
            this.desc = desc;
        }
        public Salud(string name_salud, double desc){
            this.name_salud = name_salud;
            this.desc = desc;
        }

        public List<Salud> findAll()
        {
            List<Salud> list = null;
            try
            {
                String sql = "select * from salud";
                Salud salud = null;
                list = new List<Salud>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    salud = new Salud(int.Parse(dtRow["id_salud"].ToString()), dtRow["nombre"].ToString(), double.Parse(dtRow["descuento"].ToString()));
                    list.Add(salud);
                } return list;
            }
            catch (Exception e)
            {
                Console.Write("error: " + e.Message);
                return list;
            }
        }
        public int save() {
            try {

                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("insert into salud (nombre,descuento) values ('{0}','{1}')",
                    this.name_salud, this.desc), conex);
                return comando.ExecuteNonQuery();
            }catch(Exception e){
                Console.Write(e.Message);
                return 0;
            }
        }
    }
}
