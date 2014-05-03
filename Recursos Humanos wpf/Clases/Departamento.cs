using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Departamento
    {
        public int id { get; set; }
        public string name { get; set; }
        public string rutBoss { get; set; }
        public Departamento() { }

        public Departamento(int id, string name) {
            this.id = id;
            this.name = name;
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
                return list;
            }
        }
    }
}
