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

namespace UniversalParserLibrary
{
    public static class UniversalParser
    {
        internal static List<Skill> Skills { get; set; } = new List<Skill>(); 


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
                foreach (FileInfo file in files)                                                               //For each file, create new thread       
                {
                    threads.Add(new Thread(() => LogicForParsing.NewParse(file.FullName, file.Name)));
                    threads.Last().Start();
                }
                AwaitThreads(ref threads);
                PrivateDictionary.UpdateDictionary();
                Console.WriteLine("DONE");
            }
            else
            {
                Console.WriteLine(destination_name + " PATH NOT FOUND\n");
            }
        }

        /// <summary>
        /// Method for training base with data from the specified folder 
        /// </summary>
        /// <param name="destination_name">specified folder</param>
        public static void StartTraining(string destination_name, bool type_of_parse)
        {
            if (Directory.Exists(destination_name))
            {
                List<Thread> threads = new List<Thread>();                                                     //Create list with threads
                DirectoryInfo dir = new DirectoryInfo(destination_name);                                       //Open directory
                FileInfo[] files = dir.GetFiles("*.doc");                                                      //Get files from directory
                foreach (FileInfo file in files)                                                               //For each file, create new thread       
                {
                    threads.Add(new Thread(() => LogicForTraining.NewTrain(file.FullName)));
                    threads.Last().Start();
                }
                AwaitThreads(ref threads);
                Console.WriteLine("DONE");
            }
            else
            {
                Console.WriteLine(destination_name + " PATH NOT FOUND\n");
            }
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


        }
    }
}
