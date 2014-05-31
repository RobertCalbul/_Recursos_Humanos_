﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace Recursos_Humanos_wpf.Clases
{
    class User_Group
    {
        MySqlConnection con = null;
        public int id { get; set; }
        public String name { get; set; }

        public User_Group() { }

        public User_Group(int id, String name) {
            this.id = id;
            this.name = name;
        }
        public User_Group(int id)
        {
            this.id = id;
        }
        public User_Group(String name)
        {
            this.name = name;
        }

        public List<User_Group> findAll()
        {
            List<User_Group> listUserGroup = null;
            String sql = "select * from user_group";
            try
            {
                con = new Conexion().getConexion();
                listUserGroup = new List<User_Group>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                while (res.Read()) listUserGroup.Add(new User_Group(res.GetInt32(0),res.GetString(1)));
                return listUserGroup;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR User_Group.findAll() " + ex.Message);
                return listUserGroup;
            }
        }

        public int save()
        {
            String sql = "INSERT INTO user_group (nombre) values('"+this.name+"')";
            try
            {
                con = new Conexion().getConexion();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                return  sqlCom.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR User_Group.save() " + ex.Message);
                return 0;
            }

        }

        public int getIdByName()
        {
            List<Privilegio> listPrivilegio = null;
            String sql = "SELECT id_user_group from user_group WHERE nombre ='" + this.name + "'";
            try
            {
                con = new Conexion().getConexion();
                listPrivilegio = new List<Privilegio>();
                con.Open();

                MySqlCommand sqlCom = new MySqlCommand(sql, con);
                MySqlDataReader res = sqlCom.ExecuteReader();

                int id = -1;
                if (res.Read()) id = res.GetInt32(0);
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR User_Group.getIdByName() " + ex.Message);
                return -1;
            }
        }
    }
}
