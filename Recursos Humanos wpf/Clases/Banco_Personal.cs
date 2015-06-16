using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Banco_Personal
    {
        MySqlConnection conex = null;
        public int banco_id_banco { get; set; }
        public int personal_id_personal { get; set; }
        public string cta_bancaria { get; set; }

        public Banco_Personal() { }
        public Banco_Personal(int banco_id_banco,int personal_id_personal,string cta_bancaria) 
        {
            this.banco_id_banco = banco_id_banco;
            this.personal_id_personal = personal_id_personal;
            this.cta_bancaria = cta_bancaria;
        }

        public int save() {
            try
            {
                int ardilloqlo;
                conex = new Conexion().GetConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("insert into banco_personal (banco_id_banco,personal_id_personal,cta_bancaria) values ('{0}','{1}','{2}')",
                    this.banco_id_banco,this.personal_id_personal,this.cta_bancaria), conex);
                ardilloqlo=comando.ExecuteNonQuery();
                return ardilloqlo;
            }
            catch (Exception e) {
                Console.Write("error: "+e.Message);
                return 0;
            }
        }
    }
}
