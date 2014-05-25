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
        //MySqlConnection conex = null;
        public int id_Login { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public int id_UserGroup { get; set; }

        public Login()
        { }
        public Login(int id_Login, string nombre, string password,int id_UserGroup)
        {
            this.id_Login = id_Login;
            this.nombre = nombre;
            this.password = password;
            this.id_UserGroup = id_UserGroup;
        }

        public object[] findBy(string nombre, string password)
        {
            object[] arreglo = null;
            try
            {   /*
                 *0. id_login
                 *1. nombre 
                 *2. password
                 *3. id_usergroup
                 * 
                 */
                arreglo = new object[3];
                string sql = "SELECT * FROM login WHERE nombre='" + nombre + "' AND password='" + password+"'";
                //SELECT * FROM login where nombre='admin' and password='admin';
                
                DataTable dataTable = new Clases.Consultas().QueryDB(sql);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dtRow in dataTable.Rows)
                    {
                        arreglo[0] = dtRow["id_login"];
                        arreglo[1] = dtRow["nombre"];
                        arreglo[2] = dtRow["password"];
                        arreglo[3] = dtRow["id_usergroup"];

                    }
                }
                else
                {
                    new Dialog("Lo sentimos. El usuario o contraseña es incorrecta.").Show();
                    return null;
                }
                return arreglo;
            }
            catch (Exception e)
            {
                return arreglo;
            }

        } // fin del metodo findby




    }
}
