using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Recursos_Humanos_wpf.Clases
{
    class tipo_jornada
    {
        public int id_tipo_jornada { get; set; }
        public String nombre { get; set; }
        public int nr_hr_semanales { get; set; }

        public tipo_jornada() { }
        public tipo_jornada(int id_tipo_jornada, String nombre)
        {
            this.id_tipo_jornada = id_tipo_jornada;
            this.nombre = nombre;
        }


        public List<tipo_jornada> findforCargo(String cargo)
        {
            List<tipo_jornada> list = null;
            try
            {
                String sql = "select c.id_tipo_jornada,c.nombre from tipo_jornada as c"
                              +" INNER JOIN jornada_cargo as jc ON(jc.tipo_jornada_id_tipo_jornada = c.id_tipo_jornada)"
                              +" INNER JOIN cargo as ca ON(ca.id_cargo = jc.cargo_id_cargo)"
                              +" WHERE ca.cargo='" + cargo+ "'";
                tipo_jornada jornada = null;
                list = new List<tipo_jornada>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    jornada = new tipo_jornada(int.Parse(dtRow["id_tipo_jornada"].ToString()), dtRow["nombre"].ToString());
                    list.Add(jornada);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR TIPO_JORNADA.findAll() " + ex.Message);
                return list;
            }
        }
    }
}
