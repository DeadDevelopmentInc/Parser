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
            string type = args[0];
            string folder = args[1];
            switch (type)
            {
                case "1": { UniversalParser.StartParsing(folder); } break;
                case "2": { UniversalParser.StartTraining(folder, true); } break;
                case "3": { UniversalParser.StartTraining(folder, false); } break;
                default: {  } break;
            }
            Console.ReadLine();
        }
    }
}
