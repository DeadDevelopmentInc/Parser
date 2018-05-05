using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;
using UniversalParserLibrary;

namespace NewTypeParse
{
    class Program
    {
        static void Main(string[] args)
        {            
            bool fl = false;
            while (!fl)
            {
                Console.Write("Write your act:\n" +
                    "1.Parse in this folder with documents\n" +
                    "2.Train parser with documents in folder (with saving data in db)\n" +
                    "3.Train parser with documents in folder (without saving data in db)\n" +
                    "4.Exit\n" +
                    "Write: ");
                switch (Console.ReadLine())
                {
                    case "1": { Console.Write("Write folder: "); UniversalParser.StartParsing(Console.ReadLine()); } break;
                    case "2": { Console.Write("Write folder: "); UniversalParser.StartTraining(Console.ReadLine(), true); } break;
                    case "3": { Console.Write("Write folder: "); UniversalParser.StartTraining(Console.ReadLine(), false); } break;
                    case "4": { fl = true; } break;
                }
            }
        }
    }
}
