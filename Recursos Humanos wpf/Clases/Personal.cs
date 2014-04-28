using System;
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
        public int AFP_id_afp { get; set; }
        public int salud_id_salud { get; set; }
        public int departamento_id_departamento { get; set; }
        public int contrato_id_contrato { get; set; }

        public Personal()
        { }
        public Personal(string rut) {
            this.rut = rut;
        }
        public Personal(string rut, string nombre, string apellido, int edad, string telefono,
            string direccion, string email, string cta_bancaria, int AFP_id_afp, int salud_id_salud, int departamento_id_departamento)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.apellido = apellido;
            this.edad = edad;
            this.telefono = telefono;
            this.direccion = direccion;
            this.email = email;
            this.cta_bancaria = cta_bancaria;
            this.AFP_id_afp = AFP_id_afp;
            this.salud_id_salud = salud_id_salud;
            this.departamento_id_departamento = departamento_id_departamento;
            //this.contrato_id_contrato = contrato_id_contrato;
        }
        public Personal(string rut, string nombre, string apellido, int edad, byte[] foto_portada, string telefono,
            string direccion, string email, string cta_bancaria, int AFP_id_afp, int salud_id_salud, int departamento_id_departamento)
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
            this.AFP_id_afp = AFP_id_afp;
            this.salud_id_salud = salud_id_salud;
            this.departamento_id_departamento = departamento_id_departamento;
            //this.contrato_id_contrato = contrato_id_contrato;
        }
        public List<string> findAll(int value)
        {
            conex = new Clases.Conexion().getConexion();
            conex.Open();

            MySqlCommand sqlCom = new MySqlCommand("SELECT * FROM recursos_humanos.personal", conex);
            MySqlDataReader res = sqlCom.ExecuteReader();

            //Creo una lista para guardar los resultados
            List<string> resultados = new List<string>();

            //Leo los resultados
            while (res.Read())
            {
                string Myreader = "";
                Myreader += value==0?res.GetString(0):value==1?res.GetString(1):value==2?res.GetString(2):"";
                Myreader += value == 3 ? res.GetString(5) : value == 4 ? res.GetString(6) : value == 5 ? res.GetString(7) : "";
                resultados.Add(Myreader);
            }
            conex.Close();
            return resultados;
        }

        public object[] findBy(string value, string paramSearch)
        {
            object[] arreglo = null;
            try
            {
                arreglo = new object[12];
                // RUT -- NOMBRE -- APELLIDO -- EDAD -- FOTO -- TELEFONO -- DIRECCION
                // EMAIL -- CTA.BANCARIA -- AFP_NOMBRE -- SALUD_NOMBRE -- NOMBRE_DPTO --
                String sql = "SELECT b.rut, b.nombre, b.apellido,b.edad, b.foto_portada,"
                           + " b.telefono,b.direccion, b.email,b.cta_bancaria,a.id_afp,a.nombre as afp,"
                           + " c.id_salud,c.nombre AS salud,d.id_departamento,d.nombre AS dpto,b.contrato_id_contrato"
                           + " FROM personal AS b"
                           + " INNER JOIN afp AS a ON(b.AFP_id_afp=a.id_afp)"
                           + " INNER JOIN salud AS c ON (c.`id_salud`= b.`salud_id_salud`)"
                           + " INNER JOIN departamento AS d ON(d.`id_departamento`=b.`departamento_id_departamento`)"
                           + " where b." + paramSearch + "='" + value + "'";

                DataTable dataTable = new Clases.Consultas().QueryDB(sql);
                if (dataTable.Rows.Count != 0)
                {
                    foreach (DataRow dtRow in dataTable.Rows)
                    {
                        //cargar foto de perfil empleado
                        arreglo[0] = dtRow["foto_portada"];
                        arreglo[1] = dtRow["rut"];
                        arreglo[2] = dtRow["nombre"];
                        arreglo[3] = dtRow["apellido"];
                        arreglo[4] = dtRow["edad"];
                        arreglo[5] = dtRow["telefono"];
                        arreglo[6] = dtRow["direccion"];
                        arreglo[7] = dtRow["email"];
                        arreglo[8] = dtRow["cta_bancaria"];
                        arreglo[9] = string.Format("{0}:{1}", dtRow["id_afp"], dtRow["afp"]);
                        arreglo[10] = string.Format("{0}:{1}", dtRow["id_salud"], dtRow["salud"]);
                        arreglo[11] = string.Format("{0}:{1}", dtRow["id_departamento"], dtRow["dpto"]);
                           
                    } 
                }else
                {
                    MessageBox.Show("No se han encontrado resultados.");
                }
                return arreglo;
            }catch(Exception e){
                MessageBox.Show("ERRO CARGAR FINDPERSONAL "+e.Message);
                return arreglo;
            }

        }

        public int Save()
        {
            int retorno = 0;
            try
            {
                conex = new Conexion().getConexion();
                MySqlCommand command = new MySqlCommand();
                command.Connection = conex;
                command.CommandText = "Insert into personal (rut,nombre,apellido,edad,foto_portada,telefono,direccion,email,cta_bancaria,AFP_id_afp,salud_id_salud,departamento_id_departamento)"
                + " VALUES (?rut, ?nombre, ?apellido,?edad,?foto_portada,?telefono,?direccion,?email,?cta_bancaria,?AFP_id_afp,?salud_id_salud,?departamento_id_departamento);";
                MySqlParameter fileNameParameter = new MySqlParameter("?rut", MySqlDbType.VarChar, 20);
                MySqlParameter fileNameParameter2 = new MySqlParameter("?nombre", MySqlDbType.VarChar, 20);
                MySqlParameter fileNameParameter3 = new MySqlParameter("?apellido", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter4 = new MySqlParameter("?edad", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter5 = new MySqlParameter("?foto_portada", MySqlDbType.LongBlob, this.foto_portada.Length);
                MySqlParameter fileNameParameter6 = new MySqlParameter("?telefono", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter7 = new MySqlParameter("?direccion", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter8 = new MySqlParameter("?email", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter9 = new MySqlParameter("?cta_bancaria", MySqlDbType.VarChar, 45);
                MySqlParameter fileNameParameter10 = new MySqlParameter("?AFP_id_afp", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter11 = new MySqlParameter("?salud_id_salud", MySqlDbType.Int32, 11);
                MySqlParameter fileNameParameter12 = new MySqlParameter("?departamento_id_departamento", MySqlDbType.Int32, 11);

                fileNameParameter.Value = this.rut;
                fileNameParameter2.Value = this.nombre;
                fileNameParameter3.Value = this.apellido;
                fileNameParameter4.Value = this.edad;
                fileNameParameter5.Value = this.foto_portada;
                fileNameParameter6.Value = this.telefono;
                fileNameParameter7.Value = this.direccion;
                fileNameParameter8.Value = this.email;
                fileNameParameter9.Value = this.cta_bancaria;
                fileNameParameter10.Value = this.AFP_id_afp;
                fileNameParameter11.Value = this.salud_id_salud;
                fileNameParameter12.Value = this.departamento_id_departamento;


                command.Parameters.Add(fileNameParameter);
                command.Parameters.Add(fileNameParameter2);
                command.Parameters.Add(fileNameParameter3);
                command.Parameters.Add(fileNameParameter4);
                command.Parameters.Add(fileNameParameter5);
                command.Parameters.Add(fileNameParameter6);
                command.Parameters.Add(fileNameParameter7);
                command.Parameters.Add(fileNameParameter8);
                command.Parameters.Add(fileNameParameter9);
                command.Parameters.Add(fileNameParameter10);
                command.Parameters.Add(fileNameParameter11);
                command.Parameters.Add(fileNameParameter12);

                conex.Open();
                retorno = command.ExecuteNonQuery();
                conex.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conex.Close();
                return retorno;
            }
            return retorno;

        } 

        public int Update() {
            /**Metodo que se encarga de actualizar la informacion basica de un empleado registrado
             * 
             * **/
            try
            {
                string sql = "UPDATE personal set nombre ='" + this.nombre
                    + "',apellido='" + this.apellido
                    + "',telefono='" + this.telefono
                    + "',email='" + this.email
                    + "',edad='" + this.edad
                    + "',direccion='" + this.direccion
                    + "',cta_bancaria='" + this.cta_bancaria
                    + "',AFP_id_afp='" + this.AFP_id_afp
                    + "',salud_id_salud='" +this.salud_id_salud
                    + "',departamento_id_departamento='" + this.departamento_id_departamento
                    + "' WHERE rut='" + this.rut + "'";

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
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("delete from personal where rut =('{0}')", this.rut), conex);
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conex.Close();
                return 0;
            }
            finally {
                conex.Close();
            }

        }
    }
}