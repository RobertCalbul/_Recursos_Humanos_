using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
namespace Recursos_Humanos_wpf.Clases
{
    class Conexion
    {
        public MySqlConnection getConexion()
        {
            return new MySqlConnection("data source=localhost; user id=root; password=12345678; database=recursos_humanos");
        }
    }
}
