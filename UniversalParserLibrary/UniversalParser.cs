using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversalParserLibrary.Training;
using UniversalParserLibrary.Parsing;
using UniversalParserLibrary.Models;
using Npgsql;
using MongoDB.Driver;
using MongoDB.Bson;

namespace UniversalParserLibrary
{
    public static class UniversalParser
    {
        /// <summary>
        /// Method for parse files from the specified folder 
        /// </summary>
        /// <param name="destination_name">specified folder</param>
        public static void StartParsingAllDoc()
        {
            string destination_name = "ems-resume";
            //PostgreDB.ReadFilesInDB();
            if (Directory.Exists(destination_name))
            {
                List<Thread> threads = new List<Thread>();                                                     //Create list with threads
                DirectoryInfo dir = new DirectoryInfo(destination_name);                                       //Open directory
                FileInfo[] files = dir.GetFiles("*.doc");                                                      //Get files from directory
        
                if (files.Length != 0)
                {
                    foreach (FileInfo file in files)                                                               //For each file, create new thread       
                    {
                        threads.Add(new Thread(() => LogicForParsing.NewParse(file.FullName, file.Name)));
                        threads.Last().Start();
                    }
                    AwaitThreads(ref threads);
                    new Models.Exceptions_and_Events.Info("updating projects and skills", "INFO", "sending projects", 0);
                    PrivateDictionary.SendProjects(Project.FindSimpleProjects(LogicForParsing.ProjectsList));
                    new Models.Exceptions_and_Events.Info("updating projects and skills", "INFO", "sending skills", 0);
                    PrivateDictionary.UpdateDictionarySkills();
                }
                
            }
            else new Models.Exceptions_and_Events.Exception("finding folder", "ERROR", "folder not found");
        }

        /// <summary>
        /// Method for parse all users from postre without files
        /// </summary>
        public static void StartParsingAllWithoutDoc()
        {
            var users = PostgreDB.GetUsers();
            try
            {
                MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
                IMongoDatabase database = client.GetDatabase("ems");
                var colUsers = database.GetCollection<BsonDocument>("users");
                UpdateOptions updateOptions = new UpdateOptions { IsUpsert = true };
                foreach (var user in users)
                {
                    FilterDefinition<BsonDocument> builders = Builders<BsonDocument>.Filter.Eq("lotuspersonUn", user.personId);
                    colUsers.UpdateOne(builders, user.GetUserBson(), updateOptions);
                    new Models.Exceptions_and_Events.Info("sending data", "INFO", "document " + user.personId + " succesesfull send in db", 1);
                }               
            }
            /*Exception e - from System Exception
              new Exception({params}) - from Exeptions_and_Events*/
            catch (Exception e) { new Models.Exceptions_and_Events.Exception("writing in db", "ERROR", e.Message); }
        }

        /// <summary>
        /// Add method for starting parse single document
        /// </summary>
        public static void SingleParsingWithDoc(string id)
        {
            PostgreDB.ReadFilesInDB(id);
            FileInfo file = new FileInfo("ems-resume/" + id + ".doc");
            LogicForParsing.NewParse(file.FullName, file.Name);
        }

        /// <summary>
        /// Add method for starting parse single user
        /// </summary>
        public static void SingleParsingWithoutDoc(string id)
        {
            User user = new User();
            PostgreDB.GettingPersonalInfoFromDB(id, user);
            try
            {
                MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
                IMongoDatabase database = client.GetDatabase("ems");
                var colUsers = database.GetCollection<BsonDocument>("users");
                UpdateOptions updateOptions = new UpdateOptions { IsUpsert = true };
                FilterDefinition<BsonDocument> builders = Builders<BsonDocument>.Filter.Eq("lotuspersonUn", user.personId);
                colUsers.UpdateOne(builders, user.GetUserBson(), updateOptions);
                new Models.Exceptions_and_Events.Info("sending data", "INFO", "document " + user.personId + " succesesfull send in db", 1);
            }
            /*Exception e - from System Exception
              new Exception({params}) - from Exeptions_and_Events*/
            catch (Exception e) { new Models.Exceptions_and_Events.Exception("writing in db", "ERROR", e.Message); }

        }

        /// <summary>
        /// Method for training base with data from the specified folder 
        /// </summary>
        /// <param name="destination_name">specified folder</param>
        public static void StartTraining(bool type_of_parse)
        {
            string destination_name = "ems-resume";
            PostgreDB.ReadFilesInDB();
            LogicForTraining.TrainList = new List<TrainSkill>();
            if (Directory.Exists(destination_name))
            {
                List<Thread> threads = new List<Thread>();                                                     //Create list with threads
                DirectoryInfo dir = new DirectoryInfo(destination_name);                                       //Open directory
                FileInfo[] files = dir.GetFiles("*.doc");                                                      //Get files from directory
                if (files.Length != 0)
                {
                    foreach (FileInfo file in files)                                                               //For each file, create new thread       
                    {
                        threads.Add(new Thread(() => LogicForTraining.NewTrain(file.FullName)));
                        threads.Last().Start();
                    }
                    AwaitThreads(ref threads);
                    if (type_of_parse) { WriteDataInDBWithSaving(); }
                    else { WriteDataInDB(); }
                }
                else { new Models.Exceptions_and_Events.Exception("finding documents", "ERROR", "folder doesn't contain documents"); }
            }
            else new Models.Exceptions_and_Events.Exception("finding folder", "ERROR", "folder not found");
        }
        
        /// <summary>
        /// Method for waiting while all threads finish
        /// </summary>
        /// <param name="threads"></param>
        private static void AwaitThreads(ref List<Thread> threads)
        {
            bool awaitTh = false;
            while (!awaitTh)
            {
                foreach (Thread th in threads)
                {
                    if (th.IsAlive) { awaitTh = false; break; }
                    else { awaitTh = true; }
                }
            }
            threads.Clear();
        }

        private static void WriteDataInDB()
        {
            var list = LogicForTraining.TrainList;
            LogicForTraining.GenerateData(list);
            List<Skill> newList = new List<Skill>();
            foreach(var skill in list) { newList.Add(skill.ForWrite()); }
            PrivateDictionary.UpdateDictionarySkills(newList);
        }

        private static void WriteDataInDBWithSaving()
        {
            var tempList = LogicForTraining.GenerateTrains(LogicForTraining.TrainList);
            var forWrite = new List<Skill>();
            foreach(var item in tempList) { forWrite.Add(item.ForWrite()); }
            PrivateDictionary.UpdateDictionarySkills(forWrite);
        }
    }
}
