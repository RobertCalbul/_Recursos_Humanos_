using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recursos_Humanos_wpf.Clases
{
    class Privilegio
    {
        MySqlConnection con = null;
        public int id { get; set; }
        public String name { get; set; }

        public Privilegio() { }
        public Privilegio(String name)
        {
            this.name = name;
        }
        public Privilegio(int id)
        {
            this.id = id;
        }
        public Privilegio(int id, String name) {
            this.id = id;
            this.name = name;
        }
        public List<Privilegio> findAll()
        {
            List<Privilegio> listUserGroup = null;
            String sql = "select * from privilegios";
            try
            {
                con = new Conexion().getConexion();
                listUserGroup = new List<Privilegio>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                while (res.Read()) listUserGroup.Add(new Privilegio(res.GetInt32(0), res.GetString(1)));
                return listUserGroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Privilegio.findAll() " + ex.Message);
                return listUserGroup;
            }
        }

        public List<Privilegio> findByidGroup()
        {
            List<Privilegio> listPrivilegio = null;
            String sql = "SELECT b.id_privilegios, b.nombre FROM usergroup_privilegios AS a " +
           "INNER JOIN privilegios AS b " +
           "ON (a.id_privilegios = b.id_privilegios) " +
           "INNER JOIN user_group AS c " +
           "ON (a.id_user_group = c.id_user_group) " +
           "WHERE c.id_user_group = " + this.id;
            try
            {
                con = new Conexion().getConexion();
                listPrivilegio = new List<Privilegio>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                while (res.Read()) listPrivilegio.Add(new Privilegio(res.GetInt32(0), res.GetString(1)));
                return listPrivilegio;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Privilegios.findByidGroup() " + ex.Message);
                return listPrivilegio;
            }
        }
        public int getIdByName()
        {
            List<Privilegio> listPrivilegio = null;
            String sql = "SELECT id_privilegios from privilegios WHERE nombre ='" + this.name+"'";
            try
            {
                con = new Conexion().getConexion();
                listPrivilegio = new List<Privilegio>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                int id = -1;
                if (res.Read()) id = res.GetInt32(0);
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Privilegios.getIdByName() " + ex.Message);
                return -1;
            }
        }
        public int save()
        {
            String sql = "INSERT INTO privilegios (nombre) values('" + this.name + "')";
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Privilegios.save() " + ex.Message);
                return 0;
            }

        }
        public int deleteById()
        {
            String sql = "DELETE FROM  privilegios WHERE id_privilegios=" + this.id;
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Privilegio.deleteById() " + ex.Message);
                return 0;
            }

        }
        public int update()
        {
            String sql = "UPDATE privilegios set nombre='"+this.name+"' WHERE id_privilegios="+this.id;
            try
            {
                con = new Conexion().getConexion();
                con.Open();
                MySqlCommand comando = new MySqlCommand(sql, con);
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Write("ERROR Privilegio.update()"+ex.Message);
                return 0;
            }
        }
    }
}
