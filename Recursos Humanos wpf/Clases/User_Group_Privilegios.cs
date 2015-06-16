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
        public User_Group User_Group { get; set; }
        public Privilegio privilegio { get; set; }
        public int estado { get; set; }

        public User_Group_Privilegios() { }
        public User_Group_Privilegios(Privilegio privilegio)
        {
            this.privilegio = privilegio;
        }
        public User_Group_Privilegios(User_Group User_Group, Privilegio privilegio)
        {
            this.User_Group = User_Group;
            this.privilegio = privilegio;
        }
        public int save()
        {
            String sql = "INSERT INTO usergroup_privilegios (id_user_group,id_privilegios,estado) "+
                "values(" + this.User_Group.id + ","+this.privilegio.id+",1)";
            try
            {
                con = new Conexion().GetConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ERROR User_Group_Privilegios.save() " + ex.Message);
                return 0;
            }
        }

        public int deleteByIdPrivilegio()
        {
            String sql = "DELETE FROM usergroup_privilegios WHERE id_privilegios ="+this.privilegio.id;
            try
            {
                con = new Conexion().GetConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("ERROR User_Group_Privilegios.deleteByIdPrivilegio() " + ex.Message);
                return 0;
            }
        }

        public int ifExistPrivilegio()
        {
            String sql = "SELECT * FROM usergroup_privilegios WHERE id_privilegios = "+this.privilegio.id+" AND id_user_group = "+this.User_Group.id;
            try { 
                con = new Conexion().GetConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                if (res.Read()) return 1;
                else return 0;
            }catch(Exception ex){
                return 0;
            }
        }
    }
}
