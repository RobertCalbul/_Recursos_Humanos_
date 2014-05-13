using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class Banco
    {
        public int id { get; set;}
        public string nombre { get; set;}
        public Banco() { }
        public Banco(int id, string nombre) {
            this.id = id;
            this.nombre = nombre;
       
        }//fin constructor
            
        //buscar todas las regiones y pasarle por defecto la lista con las comunas asociadas a ellas por id_region
        public List <Banco> findAll() {
            List<Banco> listreg = null;
            try
            {
                string sql = "select * from banco";
                Banco reg = null;
                listreg = new List<Banco>();
                foreach (DataRow dtrow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    //creo la region
                    reg = new Banco(int.Parse(dtrow["id_banco"].ToString()), dtrow["nombre"].ToString());
                    listreg.Add(reg);
                }//end foreach
                return listreg;
            }//end try
            catch (Exception ex) { return listreg;}


        }//end findAll
    }//fin class
}
