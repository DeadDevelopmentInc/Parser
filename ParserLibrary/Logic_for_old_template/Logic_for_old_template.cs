﻿using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_old_template
{
    internal static class Logic_for_old_template
    {
        internal static void Responce(Section section)
        {
            try
            {
                //Create list of skills
                List<string> skillsList = new List<string>();
                List<string> expList = new List<string>();
                //Get text from tables
                skillsList = Helpers.GetTextFromTable(section.Tables[0]); //table with skills
                expList = Helpers.GetTextFromTable(section.Tables[1]); // table with exp 
#if OLD_PARSE_DEBUG_SKILL
                foreach(string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
                Console.WriteLine("Parse complete");
                //Split and save exp
                var skillsModelList = ProcessSkills.ProccSkills(skillsList);
                skillsModelList.AddRange(ProcessExps.ProccExp(expList));
                //Processing and save expearence
#if OLD_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }
    }
}
