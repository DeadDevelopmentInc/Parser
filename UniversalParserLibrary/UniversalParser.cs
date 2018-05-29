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
using UniversalParserLibrary.Models.Exceptions_and_Events;

namespace UniversalParserLibrary
{
    public static class UniversalParser
    {

        /// <summary>
        /// Method for parse files from the specified folder 
        /// </summary>
        /// <param name="destination_name">specified folder</param>
        public static void StartParsing(string destination_name)
        {
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

                    PrivateDictionary.SendProjects(Project.FindSimpleProjects(LogicForParsing.ProjectsList));
                    
                    PrivateDictionary.UpdateDictionary();
                }
                
            }
            else new Models.Exceptions_and_Events.Exception("finding folder", "ERROR", "folder not found");
        }

        /// <summary>
        /// Method for training base with data from the specified folder 
        /// </summary>
        /// <param name="destination_name">specified folder</param>
        public static void StartTraining(string destination_name, bool type_of_parse)
        {
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
                else
                {
                    new Models.Exceptions_and_Events.Exception("finding documents", "ERROR", "folder doesn't contain documents, start retraining");
                    WriteDataInDBWithSaving();
                }
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
            PrivateDictionary.UpdateDictionary(newList);
        }

        private static void WriteDataInDBWithSaving()
        {
            var tempList = LogicForTraining.GenerateTrains(LogicForTraining.TrainList);
            var forWrite = new List<Skill>();
            foreach(var item in tempList) { forWrite.Add(item.ForWrite()); }
            PrivateDictionary.UpdateDictionary(forWrite);
        }
    }
}
