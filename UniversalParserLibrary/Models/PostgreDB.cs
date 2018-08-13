using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                using (var cmd = new NpgsqlCommand("SELECT cvname,cvfile FROM cv", Connection))
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
                using (var cmd = new NpgsqlCommand("SELECT cvname,cvfile FROM cv where \"person_id\"='" + id + "'", Connection))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                    {

                        var array = (byte[])reader[1];
                        File.WriteAllBytes("ems-resume/" + reader[0] + ".doc", array);
                    }
            }
            catch (Exception e) { new Exceptions_and_Events.Exception("Connectiong to DB", "ERROR", e.Message); }
        }

        internal static void UploadFileInDB()
        {

        }
    }
}
