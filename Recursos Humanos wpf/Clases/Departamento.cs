using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Departamento
    {
        public int id { get; set; }
        public string name { get; set; }
        public string rut_jefe { get; set; }
        public Departamento() { }

        public Departamento(int id)
        {
            this.id = id;
        }
        public Departamento(int id, string name) {
            this.id = id;
            this.name = name;
        }
        public Departamento(String name,String rut_jefe)
        {
            this.name = name;
            this.rut_jefe = rut_jefe;
        }
        public Departamento(int id, String name, String rut_jefe)
        {
            this.id = id;
            this.name = name;
            this.rut_jefe = rut_jefe;
        }
        public List<Departamento> findAll(){
            List<Departamento> list = null;
            try
            {
                String sql = "select * from departamento";
                Departamento dpto = null;
                list = new List<Departamento>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    dpto = new Departamento(int.Parse(dtRow["id_departamento"].ToString()), dtRow["nombre"].ToString());
                    list.Add(dpto);
                } return list;
            }
            catch (Exception e)
            {
                Console.Write("sdds" + e.Message);
                return list;
            }
        }
        public int save()
        {
            String query = "insert into departamento (nombre, id_jefe) values('" + this.name + "'," + int.Parse(this.rut_jefe) + ")";
            MySqlConnection con = null;
            try
            {
                con = new Conexion().GetConexion();
                con.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, con);


                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                con.Close();
                Console.WriteLine("EEROR " + ex.Message);
                return 0;
            }
        }
        public List<Departamento> findAll_administrativo()
        {
            List<Departamento> depto = null;
            String query = "select id_departamento, nombre, IFNULL(id_jefe,'Asignar jefe') as id_jefe from departamento";
            MySqlConnection con = null;
            try
            {

                con = new Conexion().GetConexion();
                con.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                depto = new List<Departamento>();

                while (res.Read())
                {

                    Departamento rows = new Departamento(res.GetInt32(0), res.GetString(1), res.GetString(2));
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
        public int Delete()
        {
            String query = "DELETE  FROM departamento where id_departamento = " + this.id;
            MySqlConnection con = null;
            try
            {
                con = new Conexion().GetConexion();
                con.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, con);


                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                con.Close();
                Console.WriteLine("EEROR " + ex.Message);
                return 0;
            }
        }

        public int update()
        {
            String query = "update departamento set nombre='" + this.name + "', id_jefe= " + this.rut_jefe+ " where id_departamento = " + this.id;
            MySqlConnection con = null;
            try
            {
                con = new Conexion().GetConexion();
                con.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, con);


                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                con.Close();
                Console.WriteLine("EEROR " + ex.Message);
                return 0;
            }
        }
    }
}
