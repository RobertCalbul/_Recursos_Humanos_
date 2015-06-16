using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursos_Humanos_wpf.Clases
{
    class Registro_Horario
    {
        public int personal_id_personal { get; set; }
        public String fecha { get; set; }
        public String hora_llegada { get; set; }
        public String hora_salida { get; set; }

        public Registro_Horario(int personal_id_personal) {
            this.personal_id_personal = personal_id_personal;
        }
        public Registro_Horario(int personal_id_personal, String fecha, String hora_llegada, String hora_salida) {
            this.personal_id_personal = personal_id_personal;
            this.fecha = fecha;
            this.hora_llegada = hora_llegada;
            this.hora_salida = hora_salida;
        }

        public List<Registro_Horario> findAll()
        {
            List<Registro_Horario> depto = null;
            String query = "SELECT a.personal_id_personal,a.fecha, a.hora_llegada, a.hora_salida FROM registro_horario AS a "+
                            "INNER JOIN personal AS b "+
                            "ON(a.personal_id_personal = b.id_personal) "+
                            "WHERE b.id_personal = "+this.personal_id_personal;
            MySqlConnection con = null;
            try
            {

                con = new Conexion().GetConexion();
                con.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                depto = new List<Registro_Horario>();

                while (res.Read())
                {

                    Registro_Horario rows = new Registro_Horario(res.GetInt32(0), res.GetString(1), res.GetString(2),res.GetString(3));
                    depto.Add(rows);
                }
                con.Close();
                return depto;
            }
            catch (Exception ex)
            {
                con.Close();
                Console.WriteLine("EEROR " + ex.Message);
                return depto;
            }
        }
    }
}
