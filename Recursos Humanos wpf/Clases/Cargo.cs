using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Cargo
    {
        public int id { get; set; }
        public string cargo { get; set; }

        public Cargo() { }
        public Cargo(int id, string cargo) {
            this.id = id;
            this.cargo = cargo;
        }

        public List<Cargo> findAll() {
            List<Cargo> list = null;
            try { 
                String sql = "select * from cargo";
                Cargo cargo = null;
                list = new List<Cargo>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    cargo = new Cargo(int.Parse(dtRow["id_cargo"].ToString()), dtRow["cargo"].ToString());
                    list.Add(cargo);
                }
                return list;
            }catch(Exception e){
                return list;
            }
        }
    }
}
