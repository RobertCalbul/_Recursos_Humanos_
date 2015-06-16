using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    internal class Comunas
    {
        public int id_comuna { get; set; }
        public string nombre_comuna { get; set; }

        public Comunas()
        {
        }

        public Comunas(int id, string nombre)
        {
            this.id_comuna = id;
            this.nombre_comuna = nombre;
        } //fin constructor

        //buscar comunas mediante la id de la region
        public List<Comunas> FindByidReg(int id_region)
        {
            List<Comunas> listcom = null;
            try
            {
                string sql = "select * from comunas where region_id=" + id_region.ToString() + " order by nombre";
                Comunas comuns = null;
                listcom = new List<Comunas>();
                foreach (DataRow dtrow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    comuns = new Comunas(int.Parse(dtrow["id"].ToString()), dtrow["nombre"].ToString());
                    listcom.Add(comuns);
                } // end foreach
                return listcom;
            } //ent try
            catch (Exception ex)
            {
                return listcom;
            }
        } //end metodo find comunas
    } //fin class comunas
} //fin la wea que sea