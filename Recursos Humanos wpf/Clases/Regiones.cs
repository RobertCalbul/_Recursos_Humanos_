using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Regiones
    {
        public int id_region { get; set;}
        public string nombre { get; set; }
        public Regiones() { }
        public Regiones(int id, string nombre) {
            this.id_region = id;
            this.nombre = nombre;
       
        }//fin constructor
            
        //buscar todas las regiones y pasarle por defecto la lista con las comunas asociadas a ellas por id_region
        public List<Regiones> findAll() {
            List<Regiones> listreg = null;
            try
            {
                string sql = "select * from regiones";
                Regiones reg = null;
                listreg = new List<Regiones>();
                foreach (DataRow dtrow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    //creo la region
                    reg = new Regiones(int.Parse(dtrow["id"].ToString()), dtrow["nombre"].ToString());
                    listreg.Add(reg);
                }//end foreach
                return listreg;
            }//end try
            catch (Exception ex) {
                Console.WriteLine("ERROR Regiones.findAll() "+ex.Message);
                return listreg;
            }
        }//end findAll
    }//fin class
}
