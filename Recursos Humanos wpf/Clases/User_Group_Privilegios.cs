using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class User_Group_Privilegios
    {
        MySqlConnection con = null;
        public User_Group id_User_Group { get; set; }
        public Privilegio id_Privilegio { get; set; }
        public int estado { get; set; }

        public User_Group_Privilegios() { }
        public User_Group_Privilegios(User_Group id_User_Group, Privilegio id_Privilegio) {
            this.id_User_Group = id_User_Group;
            this.id_Privilegio = id_Privilegio;
        }
        public int save()
        {
            String sql = "INSERT INTO usergroup_privilegios (id_user_group,id_privilegios,estado) "+
                "values(" + this.id_User_Group.id + ","+this.id_Privilegio.id+",1)";
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR User_Group_Privilegios.save() " + ex.Message);
                return 0;
            }
        }
    }
}
