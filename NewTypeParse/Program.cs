using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using ParserLibrary;
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
        //static void Main(string[] args)
        //{
        //    string container = null;
        //    Console.Write("Write name of document (pattern *.doc): ");
        //    bool fl = false;
        //    while(!fl)
        //    {
        //        try
        //        {
        //            container = Console.ReadLine();
        //            Regex regex = new Regex(@"(\w*).doc");
        //            MatchCollection match = regex.Matches(container);
        //            if (match.Count > 0)
        //            {
        //                fl = true;
        //            }
        //        }
        //        catch
        //        {
        //            Console.WriteLine("Check writed data");
        //        }
        //    }
        //    Parser.StartParse(container);
        //    Console.Write("Press any button");
        //    Console.ReadKey();
        //}

        /// <summary>
        /// Method main for parsing folder
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            string container = null;
            Console.Write("Write folder path: ");
            bool fl = false;
            while (!fl)
            {
                container = Console.ReadLine();
                if (Directory.Exists(container))
                {
                    DirectoryInfo dir = new DirectoryInfo(container);
                    FileInfo[] files = dir.GetFiles("*.doc");
                    foreach (FileInfo file in files)
                    {
                        Thread thread = new Thread(() => Parser.StartParse(file.FullName));
                        thread.Start();
                        Console.WriteLine("Parse " + file.Name + " complete");
                    }
                }
                else
                {
                    Console.WriteLine(container + " PATH NOT FOUND");
                }
            }

            Console.Write("Close or press any button for reparse");
            Console.ReadKey();
        }

    }
}
