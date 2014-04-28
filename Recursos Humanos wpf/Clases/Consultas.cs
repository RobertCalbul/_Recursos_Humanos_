using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.IO;
using System.Windows;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Consultas
    {        MySqlConnection conex;

        public DataTable QueryDB(String query)
        {
            DataTable dt = new DataTable();
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlDataAdapter da = new MySqlDataAdapter(query, conex);
                da.Fill(dt);
                conex.Close();
                return dt;
            }
            catch (Exception ex)
            {
                conex.Close();
                MessageBox.Show("Se produjo un error al hacer la consulta: " + ex.Message);
                return dt;
            }
        }

        public int Update(String query){

            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand sqlCom = new MySqlCommand(query, conex);
                return sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                conex.Close();
                MessageBox.Show("Se produjo un error al actualizar o insertar datos: " + ex.Message);
                return 0;
            }
            finally {
                conex.Close();
            }
        }

        

        public  int guardar_afp(string name,double desc)
        {
            int retorno = 0;
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("Insert into afp (nombre,descuento) values ('{0}','{1}')",
                    name, desc), conex);
                retorno = comando.ExecuteNonQuery();
                conex.Close();
                return retorno;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conex.Close();
                return retorno;
            }
           
        }

        public int guardar_salud(string name, double desc)
        {
            int retorno = 0;
            try
            {
                conex = new Conexion().getConexion();
                conex.Open();
                MySqlCommand comando = new MySqlCommand(string.Format("Insert into salud (nombre,descuento) values ('{0}','{1}')",
                    name, desc), conex);
                retorno = comando.ExecuteNonQuery();
                conex.Close();
                return retorno;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                conex.Close();
                return retorno;
            }

        }

    }
  }

