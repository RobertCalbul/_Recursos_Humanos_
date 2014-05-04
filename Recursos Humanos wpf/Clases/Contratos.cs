using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;

namespace Recursos_Humanos_wpf.Clases
{
    class Contratos
    {
        public String rut { get; set; }
        public String nombre_completo { get; set; }
        public String direccion { get; set; }
        public String depto { get; set; }
        public String afp { get; set; }
        public String salud { get; set; }
        public String fInicio{ get; set; }
        public String Cargo { get; set; }
        public String tContrato { get; set; }
        public String fTermino { get; set; }
        public int SueldoBase { get; set; }
        public String estado {get;set;}
        public Contratos() { }
        public Contratos(String rut,String fInicio, String fTermino, String estado, int SueldoBase, String tContrato, String Cargo) {
            this.rut = rut;
            this.fInicio = fInicio;
            this.fTermino = fTermino;
            this.estado = estado;
            this.SueldoBase = SueldoBase;
            this.tContrato = tContrato;
            this.Cargo = Cargo;
        }
        public Contratos(String rut, String fInicio, String nombre, String direccion, String Cargo, String depto, String tContrato, int SueldoBase, String fTermino, String afp, String salud)
        {
            this.rut = rut;
            this.nombre_completo = nombre;
            this.direccion = direccion;
            this.depto = depto;
            this.afp = afp;
            this.salud = salud;
            this.fInicio = fInicio;
            this.fTermino = fTermino;
            this.Cargo = Cargo;
            this.tContrato = tContrato;
            this.SueldoBase = SueldoBase;
        }

        public string[] findBy(string value){
            string[] obj = null;
            try
            {
                obj = new string[6];
                string sql = "SELECT e.fecha_inicio,e.fecha_termino,e.estado,"
                    + " (SELECT c.tipo AS tipo_contrato"
                    + " FROM personal AS p"
                    + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
                    + " INNER JOIN tipo_contrato AS c ON(e.tipo_contrato_id_tipo_contrato=c.id_tipo_contrato)"
                    + " WHERE p.rut ='" + value + "') AS tipo_contrato, "
                    + " IFNULL((SELECT f.cargo AS nombre_cargo"
                    + " FROM personal AS p"
                    + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
                    + " INNER JOIN cargo AS f ON(f.id_cargo=e.cargo_id_cargo)"
                    + " WHERE p.rut LIKE '%" + value + "%'),'1')AS cargo "
                    + " FROM personal AS p"
                    + " INNER JOIN contrato AS e ON (e.id_contrato = p.contrato_id_contrato)"
                    + " INNER JOIN cargo AS f ON(f.id_cargo=e.cargo_id_cargo)"
                    + " WHERE p.nombre LIKE  '%" + value + "%' OR p.apellido LIKE '%" + value + "%' OR p.rut LIKE '%" + value + "%'";
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    obj[0] = dtRow["cargo"].ToString();
                    obj[1] = dtRow["fecha_inicio"].ToString().Substring(0, 10); ;
                    obj[2] = dtRow["fecha_termino"].ToString().Substring(0, 10);
                    obj[3] = dtRow["estado"].ToString();
                    obj[4] = dtRow["tipo_contrato"].ToString();
                    obj[5] = dtRow["cargo"].ToString();
                   // if (obj[5] == "1" || obj[5] == "") return new string[1];
                    //else  
                    MessageBox.Show(obj[0]);
                }
                return obj;
            }
            catch (Exception e) {
                Console.WriteLine("Contrato.findBy() " + e.Message.ToString());
                return obj;
            }
        }

        public int save() { 
            int flag = 0;
            try{
                Clases.Consultas consult = new Clases.Consultas();
                String sql = "INSERT INTO contrato (fecha_inicio,fecha_termino,estado,sueldo_minimo,tipo_contrato_id_tipo_contrato, cargo_id_cargo)" +
                    " values('"+ this.fInicio + "','" + this.fTermino + "','" + this.estado + "',"+this.SueldoBase+"," + this.tContrato + "," + this.Cargo+ ")";
                MessageBox.Show(sql);
                if (consult.Update(sql) > 0)
                {
                    sql = "SELECT DISTINCT LAST_INSERT_ID() as id_contrato FROM contrato";
                    String id_contrato = "";
                    foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)id_contrato = dtRow["id_contrato"].ToString();

                    sql = "SELECT id_personal from personal where rut='"+this.rut+"'";
                    String id_personal = "";
                    foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows) id_personal = dtRow["id_personal"].ToString();

                    sql = "INSERT INTO personal_contrato(id_personal,id_contrato) values(" + id_personal + "," + id_contrato + ")";
                    return consult.Update(sql); 
                }
                else flag = 0;
                return flag;
            }catch(Exception ex){
                Console.WriteLine("Contratos,save() " + ex.Message.ToString());
                return 0;
            }
        }

        public int DeleteByRut(Personal per) {

            try {
                string sql = "";
                //string sql = "Select id from personal where rut = '" + per.rut + "'";
                //DataTable dataTable = new Clases.Consultas().QueryDB(sql);
                //String id_contrato = "";
                //foreach (DataRow dtRow in dataTable.Rows) id_contrato = dtRow["contrato_id_contrato"].ToString();

                sql = "DELETE c.* FROM contrato c"
                +" INNER JOIN personal_contrato pc ON pc.id_contrato = c.id_contrato"
                +" INNER JOIN personal p ON p.id_personal = pc.id_personal"
                +" WHERE (p.rut='"+per.rut+"')";
                return new Clases.Consultas().Update(sql);
            }catch(Exception ex)
            {
                Console.Write("error: " + ex.Message);
                return 0;
            }
        }
    }
}
