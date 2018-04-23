using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Helpers
{
    internal static class HelpMeth
    {
        internal static void PrintTextForTest(List<string> text)
        {
            foreach(string s in text) { Console.WriteLine(s); }
        }

        internal static void PrintExpsForTest(List<Tuple<string, string>> text)
        {
            foreach (Tuple<string, string> s in text) { Console.WriteLine(s.Item1 + " IN " + s.Item2); }
        }
    }
}
