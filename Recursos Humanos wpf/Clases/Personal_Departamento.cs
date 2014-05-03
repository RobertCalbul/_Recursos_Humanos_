using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Personal_Departamento
    {
        MySqlConnection conex = null;
        public int id_departamento_personal { get; set; }
        public int id_personal { get; set; }
        public int id_departamento { get; set; }

        public Personal_Departamento() { }
        public Personal_Departamento(int id_departamento_personal,int id_personal, int id_departamento) 
        {
            this.id_departamento_personal = id_departamento_personal;
            this.id_personal = id_personal;
            this.id_departamento = id_departamento;
        }
        public Personal_Departamento(int id_personal, int id_departamento)
        {
            this.id_personal = id_personal;
            this.id_departamento = id_departamento;
        }
        public int save() {
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("insert into personal_departamento (id_personal,id_departamento) values ('{0}','{1}')",
                    this.id_personal, this.id_departamento), conex);
                return comando.ExecuteNonQuery();
            }
            catch (Exception e) {
                return 0;
            }
        }
    }
}
