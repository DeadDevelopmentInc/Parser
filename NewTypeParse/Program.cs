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
            int type = 0;
            Console.Write("Write name of document (pattern *.doc) and type of document (new template = 1 or old template 2): ");
            bool fl = false;
            while(!fl)
            {
                try
                {
                    container = Console.ReadLine();
                    Regex regex = new Regex(@"(\w*).doc");
                    string[] build = container.Split(' ');
                    MatchCollection match = regex.Matches(build[0]);
                    if (match.Count > 0 & int.TryParse(build[1], out type) & type >= 0 && 2 >= type)
                    {
                        fl = true;
                        container = build[0];
                    }
                }
                catch
                {
                    Console.WriteLine("Check writed data");
                }
            }
            Logic.ParseDoc(container, type);
            Console.Write("Press any button");
            Console.ReadKey();
        }
    }
}
