using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class User_Group
    {
        MySqlConnection con = null;
        public int id { get; set; }
        public String name { get; set; }

        public User_Group() { }
        public User_Group(int id, String name) {
            this.id = id;
            this.name = name;
        }

        public List<User_Group> findAll()
        {
            List<User_Group> listUserGroup = null;
            String sql = "select * from user_group";
            try
            {
                con = new Conexion().getConexion();
                listUserGroup = new List<User_Group>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                while (res.Read()) listUserGroup.Add(new User_Group(res.GetInt32(0),res.GetString(1)));
                return listUserGroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR User_Group.findAll() " + ex.Message);
                return listUserGroup;
            }

        }
    }
}
