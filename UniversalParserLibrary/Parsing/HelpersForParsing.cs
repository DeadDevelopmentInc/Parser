using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversalParserLibrary.Helpers;

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

        internal static List<Tuple<string, string>> GetForLangFromSection(Section section)
        {
            List<Tuple<string, string>> forLangs = new List<Tuple<string, string>>();
            int type = section.Tables.Count;
            switch (type)
            {
                case 8: {
                        List<string> arraySkills = Readers.GetTextFromTable(section.Tables[6]);
                        for (int i = 1; i < arraySkills.Count; i += 2)
                        {
                            arraySkills[i] = Regex.Replace(arraySkills[i], ",(?=[^()]*\\))", "|");
                            string[] buff = Regex.Split(arraySkills[i], ", ");
                            foreach (string s in buff)
                            {
                                forLangs.Add(new Tuple<string, string>(s, arraySkills[i + 1]));
                            }
                        }
                    } break;
                case 2: {
                        List<string> arraySkills = Readers.GetTextFromTable(section.Tables[0]);
                        for (int i = 1; i < arraySkills.Count; i++)
                        {
                            if (arraySkills[i - 1].Contains("Fore"))
                            {
                                for (int j = i; j < arraySkills.Count; j += 2)
                                {
                                    forLangs.Add(new Tuple<string, string>(arraySkills[j], arraySkills[j + 1]));
                                }
                            }
                        }
                    } break;
            }
            return forLangs;
        }

        internal static int GetITExperienceFromSection(string line)
        {
            string age = "";
            Regex reg = new Regex(@"[\d]");
            MatchCollection matches = reg.Matches(line);
            if(matches.Count > 0) { foreach (Match m in matches) { age = age.Insert(age.Length, m.Value); } return int.Parse(age); }
            return 0;
        }

        internal static  List<string> GetEducationsFromSection(Section section)
        {
            bool read = false;
            List<string> educations = new List<string>();
            foreach(Paragraph p in section.Paragraphs)
            {
                if (read) educations.Add(p.Text);
                if (p.Text.Contains("EDUCATION")) read = true;
            }
            return educations;
        }

        internal static List<string> GetCertificationsFromSection(Section section)
        {
            bool read = false;
            List<string> certifications = new List<string>();
            foreach (Paragraph p in section.Paragraphs)
            {
                if (p.Text.Contains("PROFESSIONAL EXPERIENCE")) break;
                if (read) certifications.Add(p.Text);
                if (p.Text.Contains("EDUCATION")) read = true;
            }
            return certifications;
        }
    }
}
