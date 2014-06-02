using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class TipoContrato
    {
        public int id { get; set; }
        public string tipo { get; set; }
        public TipoContrato() { }

        public TipoContrato(int id, string tipo) {
            this.id = id;
            this.tipo = tipo;
        }

        public List<TipoContrato> findAll() {
            List<TipoContrato> list = null;
            try
            {
                String sql = "SELECT * FROM tipo_contrato";
                TipoContrato Tipocargo = null;
                list = new List<TipoContrato>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    Tipocargo = new TipoContrato(int.Parse(dtRow["id_tipo_contrato"].ToString()), dtRow["tipo"].ToString());
                    list.Add(Tipocargo);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR TipoContrato.findAll() "+ex.Message);
                return list;
            }
        }
    }
}
