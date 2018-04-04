using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary;
using ParserLibrary.Helpers;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;

namespace NewTypeParse
{
    class Program
    {

        /// <summary>
        /// old logic for one file
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string container = null;
            Console.Write("Write name of document (pattern *.doc): ");
            bool fl = false;
            while (!fl)
            {
                try
                {
                    container = Console.ReadLine();
                    Regex regex = new Regex(@"(\w*).doc");
                    MatchCollection match = regex.Matches(container);
                    if (match.Count > 0)
                    {
                        fl = true;
                    }
                }
                catch
                {
                    Console.WriteLine("Check writed data");
                }
            }
            Parser.StartParse(container, container);
            Console.Write("Press any button");
            Console.ReadKey();
        }

        /// <summary>
        /// Method main for parsing folder
        /// </summary>
        /// <param name="args"></param>
        //static void Main(string[] args)
        //{
        //    string container = null;
        //    Console.Write("Write folder path: ");
        //    bool fl = false;
        //    while (!fl)
        //    {
        //        container = Console.ReadLine();
        //        if (Directory.Exists(container))
        //        {
        //            //Create list with threads
        //            List<Thread> threads = new List<Thread>();
        //            //Open directory
        //            DirectoryInfo dir = new DirectoryInfo(container);
        //            //Get fies from directory
        //            FileInfo[] files = dir.GetFiles("*.doc");
        //            //Number of threads
        //            int i = 0;
        //            //For each file, create new thread
        //            foreach(FileInfo file in files)
        //            {
        //                threads.Add(new Thread(() => Parser.StartParse(file.FullName, file.Name)));
        //                threads[i].Start();
        //                i++;
        //            }
        //            //Await where all threads finish
        //            bool awaitTh = false;
        //            while(!awaitTh)
        //            {
        //                foreach(Thread th in threads)
        //                {
        //                    if(th.IsAlive) { awaitTh = false; break; }
        //                    else { awaitTh = true; }
        //                }
        //            }                    
        //            Console.WriteLine("DONE");
        //            Console.Write("Close or press any button for reparse");
        //        }
        //        else
        //        {
        //            Console.WriteLine(container + " PATH NOT FOUND");
        //        }
        //    }

        //    Console.ReadKey();
        //}

    }
}
