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
        public User_Group(int id)
        {
            this.id = id;
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

        public List<User_Group> getPrivilegio() { 
             List<User_Group> listUserGroup = null;
             String sql = "SELECT b.id_privilegios, b.nombre FROM usergroup_privilegios AS a "+
            "INNER JOIN privilegios AS b "+
            "ON (a.id_privilegios = b.id_privilegios) "+
            "INNER JOIN user_group AS c "+
            "ON (a.id_user_group = c.id_user_group) "+
            "WHERE c.id_user_group = "+this.id;
        try
        {
            con = new Conexion().getConexion();
            listUserGroup = new List<User_Group>();
            con.Open();

            MySqlCommand sqlCom = new MySqlCommand(sql, con);
            MySqlDataReader res = sqlCom.ExecuteReader();

            while (res.Read()) listUserGroup.Add(new User_Group(res.GetInt32(0), res.GetString(1)));
            return listUserGroup;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR User_Group.getPrivilegio() " + ex.Message);
            return listUserGroup;
        }
        }
    }
}
