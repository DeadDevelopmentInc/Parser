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
            string type = "pFD";
            string idOfFile = "24";
            //string type = args[0]; //type of process
            //string idOfFile = args[1]; //path to file
            switch (type)
            {
                case "pFD": { UniversalParser.StartParsingAllDoc(); } break;
                case "pFP": { UniversalParser.StartParsingAllWithoutDoc(); } break;
                case "tN": { UniversalParser.StartTraining(true); } break; //with saving S
                case "tO": { UniversalParser.StartTraining(false); } break; //without saving base
                case "pSD": { UniversalParser.SingleParsingWithDoc(idOfFile); } break; //parsing single document
                case "pSP": { UniversalParser.SingleParsingWithoutDoc(idOfFile); } break;
                default: { } break;
            }
            Console.ReadLine();
        }
    }
}
