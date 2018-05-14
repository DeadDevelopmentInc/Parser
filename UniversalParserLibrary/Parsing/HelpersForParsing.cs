using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Parsing
{
    static class HelpersForParsing
    {
        internal static List<string> GetAbitiesFromSection(Section section)
        {
            int i = -1;
            List<string> abilities = new List<string>();
            foreach(Paragraph p in section.Paragraphs)
            {
                i++;
                if (p.Text.Contains("SKILLS SUMMARY")) break;
                if (i < 2) continue;
                abilities.Add(p.Text);
            }
            return abilities;
        }

        internal static int GetITExperienceFromSection(string line)
        {
            string age = "";
            Regex reg = new Regex(@"[\d]");
            MatchCollection matches = reg.Matches(line);
            if(matches.Count > 0) { foreach (Match m in matches) { age = age.Insert(age.Length, m.Value); } return int.Parse(age); }
            return 0;
        }
    }
}
