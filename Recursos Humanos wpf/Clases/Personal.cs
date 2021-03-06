﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace Recursos_Humanos_wpf.Clases
{
    class Personal
    {
        MySqlConnection conex = null;
        public string rut { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int edad { get; set; }
        public byte[] foto_portada { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string email { get; set; }
        public string cta_bancaria { get; set; }
        public string nacionalidad { get; set; }
        public string fecha_nacimiento { get; set; }
        public int comuna { get; set; }
        public int region_residencia { get; set; }
        public int AFP_id_afp { get; set; }
        public int salud_id_salud { get; set; }
        public int id_banco { get; set; }

        public Personal()
        { }
        public Personal(string rut)
        {
            this.rut = rut;
        }



        //ESTE ES EL ACTUAL
        public Personal(string rut, string nombre, string apellido, int edad, byte[] foto_portada, string telefono, string direccion, string email,
            string cta_bancaria, string nacionalidad, string fecha_nacimiento, int comuna, int region_residencia, int id_afp, int id_salud)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.apellido = apellido;
            this.edad = edad;
            this.foto_portada = foto_portada;
            this.telefono = telefono;
            this.direccion = direccion;
            this.email = email;
            this.cta_bancaria = cta_bancaria;
            this.nacionalidad = nacionalidad;
            this.fecha_nacimiento = fecha_nacimiento;
            this.comuna = comuna;
            this.region_residencia = region_residencia;
            this.AFP_id_afp = id_afp;
            this.salud_id_salud = id_salud;
        }
        public Personal(string rut, string nombre, string apellido, int edad, string telefono, string direccion, string email,
    string cta_bancaria, string nacionalidad, string fecha_nacimiento, int comuna, int region_residencia, int id_afp, int id_salud, int id_banco)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.apellido = apellido;
            this.edad = edad;
            this.telefono = telefono;
            this.direccion = direccion;
            this.email = email;
            this.cta_bancaria = cta_bancaria;
            this.nacionalidad = nacionalidad;
            this.fecha_nacimiento = fecha_nacimiento;
            this.comuna = comuna;
            this.region_residencia = region_residencia;
            this.AFP_id_afp = id_afp;
            this.salud_id_salud = id_salud;
            this.id_banco = id_banco;
        }
        public List<string> findAll(int value)
        {
            conex = new Clases.Conexion().GetConexion();
            conex.Open();

            MySqlCommand sqlCom = new MySqlCommand("SELECT * FROM recursos_humanos.personal", conex);
            MySqlDataReader res = sqlCom.ExecuteReader();

            List<string> resultados = new List<string>();

            while (res.Read())
            {
                string Myreader = "";
                Myreader += value == 0 ? res.GetString(1) : value == 1 ? res.GetString(2) : value == 2 ? res.GetString(3) : "";
                Myreader += value == 3 ? res.GetString(6) : value == 4 ? res.GetString(7) : value == 5 ? res.GetString(8) : "";
                resultados.Add(Myreader);
            }
            conex.Close();
            return resultados;
        }

        public int get_idPersonal()
        {
            try
            {
                conex = new Clases.Conexion().GetConexion();
                conex.Open();

                MySqlCommand sqlCom = new MySqlCommand(string.Format("SELECT id_personal FROM personal WHERE rut = '{0}'", this.rut), conex);
                MySqlDataReader res = sqlCom.ExecuteReader();

                int resultados = 0;

                while (res.Read()) resultados = res.GetInt32(0);
                conex.Close();
                return resultados;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Personal.get_idPersonal() " + ex.Message);
                return 0;
            }
        }
        public object[] findBy(string value, string paramSearch)
        {
            object[] arreglo = null;
            try
            {   /*
                 *0. foto portada 
                 *1. nombre 
                 *2. apellido
                 *3. rut
                 *4. fecha
                 *5. fecha_nacimiento
                 *6. direccion
                 *7. comuna
                 *8. salud
                 *9. departamento
                 *10. afp
                 *11. edad
                 *12. region_residencia
                 *13. telefono
                 *14. email
                 *15. nacionalidad
                 *16. cta_bancaria
                 *17. banco
                 * 
                 */
                arreglo = new object[17];
                string sql = "SELECT p.foto_portada,p.nombre,p.apellido,p.rut,p.fecha_nacimiento,p.direccion,p.comuna,"
                                + " s.nombre AS salud,d.nombre AS depto,a.nombre AS afp,p.edad,p.region_residencia,"
                                + " p.telefono,p.email,p.nacionalidad,bp.cta_bancaria,b.nombre as banco"
                                + " FROM personal AS p"
                                + " INNER JOIN afp AS a ON(p.AFP_id_afp = a.id_afp)"
                                + " INNER JOIN salud AS s ON(p.salud_id_salud = s.id_salud)"
                                + " INNER JOIN personal_departamento AS pd ON(p.id_personal = pd.id_personal)"
                                + " INNER JOIN departamento AS d ON(pd.id_departamento = d.id_departamento)"
                                + " INNER JOIN banco_personal AS bp ON(bp.personal_id_personal = p.id_personal)"
                                + " INNER JOIN banco AS b ON(b.id_banco = bp.banco_id_banco)"
                                + " WHERE p." + paramSearch + " = '" + value + "'";
                DataTable dataTable = new Clases.Consultas().QueryDB(sql);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dtRow in dataTable.Rows)
                    {
                        //cargar foto de perfil empleado
                        arreglo[0] = dtRow["foto_portada"];
                        arreglo[1] = dtRow["nombre"];
                        arreglo[2] = dtRow["apellido"];
                        arreglo[3] = dtRow["rut"];
                        arreglo[4] = dtRow["fecha_nacimiento"];
                        arreglo[5] = dtRow["direccion"];
                        arreglo[6] = dtRow["comuna"];
                        arreglo[7] = dtRow["salud"];
                        arreglo[8] = dtRow["depto"];
                        arreglo[9] = dtRow["afp"];
                        arreglo[10] = dtRow["edad"];
                        arreglo[11] = dtRow["region_residencia"];
                        arreglo[12] = dtRow["telefono"];
                        arreglo[13] = dtRow["email"];
                        arreglo[14] = dtRow["nacionalidad"];
                        arreglo[15] = dtRow["cta_bancaria"];
                        arreglo[16] = dtRow["banco"];
                    }
                }
                // else new Dialog("No se encontraron coincidencias",).ShowDialog();
                return arreglo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Personal.findBy() " + ex.Message);
                return arreglo;
            }

        } // fin del metodo findby

        public int Save()
        {
            int retorno = 0;
            try
            {
                conex = new Conexion().GetConexion();
                MySqlCommand command = new MySqlCommand();
                command.Connection = conex;
                command.CommandText = "Insert into personal (rut,nombre,apellido,edad,foto_portada,telefono,direccion,email,nacionalidad,fecha_nacimiento,AFP_id_afp,salud_id_salud,comuna,region_residencia)"
                + " VALUES (?rut, ?nombre, ?apellido,?edad,?foto_portada,?telefono,?direccion,?email,?nacionalidad,?fecha_nacimiento,?AFP_id_afp,?salud_id_salud,?comuna,?region_residencia);";
                MySqlParameter fileNameParameter = new MySqlParameter("?rut", MySqlDbType.VarChar, 20);
                MySqlParameter fileNameParameter2 = new MySqlParameter("?nombre", MySqlDbType.VarChar, 20);
                MySqlParameter fileNameParameter3 = new MySqlParameter("?apellido", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter4 = new MySqlParameter("?edad", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter5 = new MySqlParameter("?foto_portada", MySqlDbType.LongBlob, this.foto_portada.Length);
                MySqlParameter fileNameParameter6 = new MySqlParameter("?telefono", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter7 = new MySqlParameter("?direccion", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter8 = new MySqlParameter("?email", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter10 = new MySqlParameter("?nacionalidad", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter11 = new MySqlParameter("?fecha_nacimiento", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter12 = new MySqlParameter("?comuna", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter13 = new MySqlParameter("?region_residencia", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter14 = new MySqlParameter("?AFP_id_afp", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter15 = new MySqlParameter("?salud_id_salud", MySqlDbType.Int32, 11);


                fileNameParameter.Value = this.rut;
                fileNameParameter2.Value = this.nombre;
                fileNameParameter3.Value = this.apellido;
                fileNameParameter4.Value = this.edad;
                fileNameParameter5.Value = this.foto_portada;
                fileNameParameter6.Value = this.telefono;
                fileNameParameter7.Value = this.direccion;
                fileNameParameter8.Value = this.email;
                fileNameParameter10.Value = this.nacionalidad;
                fileNameParameter11.Value = this.fecha_nacimiento;
                fileNameParameter12.Value = this.comuna;
                fileNameParameter13.Value = this.region_residencia;
                fileNameParameter14.Value = this.AFP_id_afp;
                fileNameParameter15.Value = this.salud_id_salud;


                command.Parameters.Add(fileNameParameter);
                command.Parameters.Add(fileNameParameter2);
                command.Parameters.Add(fileNameParameter3);
                command.Parameters.Add(fileNameParameter4);
                command.Parameters.Add(fileNameParameter5);
                command.Parameters.Add(fileNameParameter6);
                command.Parameters.Add(fileNameParameter7);
                command.Parameters.Add(fileNameParameter8);
                command.Parameters.Add(fileNameParameter10);
                command.Parameters.Add(fileNameParameter11);
                command.Parameters.Add(fileNameParameter12);
                command.Parameters.Add(fileNameParameter13);
                command.Parameters.Add(fileNameParameter14);
                command.Parameters.Add(fileNameParameter15);

                conex.Open();
                retorno = command.ExecuteNonQuery();
                conex.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
                conex.Close();
                return retorno;
            }
            return retorno;

        }

        public int Update()
        {
            /**Metodo que se encarga de actualizar la informacion basica de un empleado registrado
             * 
             * **/
            try
            {
                string sql = "UPDATE personal as p"
                    + " INNER JOIN banco_personal AS bp ON(bp.personal_id_personal = p.id_personal)"
                    + " set p.nombre ='" + this.nombre
                    + "',p.apellido='" + this.apellido
                    + "',p.telefono='" + this.telefono
                    + "',p.email='" + this.email
                    + "',p.edad='" + this.edad
                    + "',p.direccion='" + this.direccion
                    + "',bp.cta_bancaria='" + this.cta_bancaria
                    + "',bp.banco_id_banco='" + this.id_banco
                    + "',p.AFP_id_afp='" + this.AFP_id_afp
                    + "',p.salud_id_salud='" + this.salud_id_salud
                    + "',p.nacionalidad ='" + this.nacionalidad
                    + "',p.fecha_nacimiento='" + this.fecha_nacimiento
                    + "',p.comuna='" + this.comuna
                    + "',p.region_residencia='" + this.region_residencia
                    + "' WHERE p.rut='" + this.rut + "'";
                return new Clases.Consultas().Update(sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: al actualizar datos: " + ex.Message);
                return 0;
            }

        }

        public int DeleteByRrut()
        {
            try
            {
                conex = new Conexion().GetConexion();
                conex.Open();
                MySqlCommand comando3 = new MySqlCommand(string.Format("delete from personal where rut = '{0}'", this.rut), conex);
                return comando3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                conex.Close();
                return 0;
            }
            finally
            {
                conex.Close();
            }

        }
    }
}