using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models.Users;

namespace UniversalParserLibrary.Models
{
    static class PostgreDB
    {
        private static NpgsqlConnection Connection;

        static PostgreDB()
        {
            
            try
            {
                Connection = new NpgsqlConnection(Properties.Settings.Default.connectionStringPostgre);
                Connection.Open();
            }
            catch(Exception e){ new Exceptions_and_Events.Exception("Connectiong to DB", "ERROR", e.Message); }
        }

        internal static void ReadFilesInDB()
        {
            new Models.Exceptions_and_Events.Info("Reading data in DB", "INFO", "Start reading data", 0);
            if (!Directory.Exists("ems-resume"))
            {
                Directory.CreateDirectory("ems-resume");
            }
            else
            {
                Directory.Delete("ems-resume", true);
                Directory.CreateDirectory("ems-resume");
            }
            try
            {

                using (var cmd = new NpgsqlCommand("SELECT person_id,cvfile FROM cv", Connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        var array = (byte[])reader[1];
                        File.WriteAllBytes("ems-resume/" + reader[0] + ".doc", array);
                    }
                }
            }
            catch (Exception e) { new Exceptions_and_Events.Exception("Reading data in DB", "ERROR", e.Message); }


        }

        internal static void ReadFilesInDB(string id)
        {
            
            if (!Directory.Exists("ems-resume"))
            {
                Directory.CreateDirectory("ems-resume");
            }
            else
            {
                Directory.Delete("ems-resume", true);
                Directory.CreateDirectory("ems-resume");
            }
            try
            {
                using (var cmd = new NpgsqlCommand("SELECT person_id,cvfile FROM cv where \"person_id\"='" + id + "'", Connection))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {

                        var array = (byte[])reader[1];
                        File.WriteAllBytes("ems-resume/" + reader[0] + ".doc", array);
                    }
            }
            catch (Exception e) { new Exceptions_and_Events.Exception("Connectiong to DB", "ERROR", e.Message); }
        }

        internal static User GettingPersonalInfoFromDB(string personal_id, User user)
        {
            using (var cmd = new NpgsqlCommand("SELECT firstname,middlename,lastname,  engfirstname, englastname, startworkingdate, location, companyaddress, spherename, divisionname, departmentname, sectorname, jobtitle, university, vacation_start_date, vacation_end_date, sciencedegree, birthday, email, pejd FROM emp_info where \"personun\"='" + personal_id + "'", Connection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    user.fname = (string)reader[0];
                    user.mname = (string)reader[1];
                    user.lname = (string)reader[2];
                    user.passport = new List<string> { (string)reader[3], (string)reader[4] };
                    user.startWork = (DateTime)reader[5];
                    user.room = (string)reader[6];
                    user.adress = (string)reader[7];
                    user.sphere = (string)reader[8];
                    user.division = (string)reader[9];
                    user.department = (string)reader[10];
                    user.sector = (string)reader[11];
                    user.position = (string)reader[12];
                    user.univer = (string)reader[13];
                    user.vacation = new List<DateTime> { (DateTime)reader[14], (DateTime)reader[15] };
                    user.degree = (string)reader[16];
                    user.birthDay= (DateTime)reader[17];
                    user.email = (string)reader[18];
                    user.sParse = "Postgre";
                    user.personId = personal_id;
                    user.phones = (string)reader[19];
                }
            using (var cmd = new NpgsqlCommand("SELECT cvdate FROM cv where \"person_id\"='" + personal_id + "'", Connection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                {
                    user.lUploaded = (DateTime)reader[0];
                }
            return user;
        }
    }
}



// vniba, in phones
//phones mob + in
