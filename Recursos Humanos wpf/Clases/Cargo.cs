﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    internal class Cargo
    {
        public int id { get; set; }
        public string cargo { get; set; }

        public Cargo()
        {
        }

        public Cargo(int id, string cargo)
        {
            this.id = id;
            this.cargo = cargo;
        }

        public List<Cargo> findAll(int tipo_con)
        {
            List<Cargo> list = null;
            try
            {
                String sql = "select * from cargo where id_tipo=" + tipo_con.ToString() + " order by cargo";
                Cargo cargo = null;
                list = new List<Cargo>();
                foreach (DataRow dtRow in new Clases.Consultas().QueryDB(sql).Rows)
                {
                    cargo = new Cargo(int.Parse(dtRow["id_cargo"].ToString()), dtRow["cargo"].ToString());
                    list.Add(cargo);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Cargo.findAll() " + ex.Message);
                return list;
            }
        }
    }
}