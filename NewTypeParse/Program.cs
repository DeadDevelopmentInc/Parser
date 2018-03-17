using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using Spire.Doc.Interface;

namespace NewTypeParse
{
    class Program
    {
        static void Main(string[] args)
        {
            string container = null;
            Console.Write("Write name of document (pattern *.doc): ");
            bool fl = false;
            while(!fl)
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
            Logic.ParseDoc(container);
            Console.Write("Press any button");
            Console.ReadKey();
        }
    }
}
