using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Login
    {
        MySqlConnection con = null;
        public int id { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public User_Group UserGroup { get; set; }

        public Login()
        { }
        public Login(int id) {
            this.id = id;
        }
        public Login(String nombre)
        {
            this.nombre = nombre;
        }
        public Login(String nombre,String password) {
            this.nombre = nombre;
            this.password = password;
        }
        public Login(string nombre, string password, User_Group UserGroup)
        {
            this.nombre = nombre;
            this.password = password;
            this.UserGroup = UserGroup;
        }
        public Login(int id, string nombre, string password, User_Group UserGroup)
        {
            this.id = id;
            this.nombre = nombre;
            this.password = password;
            this.UserGroup = UserGroup;
        }

        public Login findBy()
        {
            Login arreglo = null;
            try
            {   /*
                 *0. id_login
                 *1. nombre 
                 *2. password
                 *3. id_usergroup
                 * 
                 */
                arreglo = new Login();
                string sql = "SELECT * FROM login WHERE nombre='" + this.nombre + "' AND password='" + this.password+"'";
                //SELECT * FROM login where nombre='admin' and password='admin';                
                DataTable dataTable = new Clases.Consultas().QueryDB(sql);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dtRow in dataTable.Rows)
                    {
                        arreglo.id= int.Parse(dtRow["id_login"].ToString());
                        arreglo.nombre = dtRow["nombre"].ToString();
                        arreglo.password = dtRow["password"].ToString();
                        arreglo.UserGroup = new User_Group(int.Parse(dtRow["id_usergroup"].ToString()));
                    }
                }
                else
                {                    
                    return null;
                }
                return arreglo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Login.findBy() " + ex.Message);
                return arreglo;
            }
        } // fin del metodo findby

        public List<Login> findAll()
        {
            List<Login> listLogin = null;
            String sql = "SELECT * FROM login";
            try
            {
                con = new Conexion().getConexion();
                listLogin = new List<Login>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                while (res.Read())
                {
                    listLogin.Add(new Login(res.GetInt32(0),res.GetString(1),res.GetString(2),new User_Group(res.GetInt32(3))));
                }
                return listLogin;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Login.findAll() " + ex.Message);
                return listLogin;
            }
        }

        public Login getIdByName()
        {
            Login id = null;
            String sql = "SELECT id_login FROM login WHERE nombre ='"+this.nombre+"'";
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                if (res.Read())
                {
                    id = new Login(res.GetInt32(0));
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Login.getIdByName() " + ex.Message);
                return id;
            }

        }
        public int save()
        {
            String sql = "INSERT INTO login (nombre,password,id_usergroup) values('"+this.nombre+"','"+this.password+"',"+this.UserGroup.id+")";
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();  
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Login.save() " + ex.Message);
                return 0;
            }
        }

        public int deleteById() {
            String sql = "DELETE FROM login WHERE id_login="+this.id;
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Login.deleteById() " + ex.Message);
                return 0;
            }
        }
    }
}
