//#define OLD_PARSE_DEBUG_EXP
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewTypeParse.ForOldTemplate
{
    public static class PrccssOldTmplt
    {
        public static void Responce(Section section)
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
                ProccExp(ref expList);
                List<Exp> exps = Parse(expList.ToArray());
                dynamic proccList = SaveSkills(skillsList, expList);
                //Processing and save expearence
                Console.WriteLine("Create json model complete");
#if OLD_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        /// <summary>
        /// Method for proccessing readed information
        /// Correctly work only with old template
        /// </summary>
        /// <param name="list"></param>
        private static void ProccExp(ref List<string> list)
        {
            List<string> listExp = new List<string>();
            Regex regex = new Regex(@"^\w+\s\d{4}\s\W\s\w");
            for (int i = 0; i < list.Count; i++)
            {
                MatchCollection match = regex.Matches(list[i]);
                if (match.Count > 0 & list[i + 5] == "Environment")
                {
                    var year = list[i].Split(' ');
                    listExp.Add(year[1]);
                    listExp.Add(list[i + 6]);
                    i += 8;
                }
            }
            list = listExp;
        }

        /// <summary>
        /// Split string from old templte
        /// </summary>
        /// <param name="allSkills">All skills</param>
        private static List<Tuple<List<Skill>, string>> SaveSkills(
            List<string> allSkills, 
            List<string> expList
            )
        {
            List<Tuple<List<Skill>, string>> list = new List<Tuple<List<Skill>, string>>();
            for (int i = 0; i < allSkills.Count & !allSkills[i].Contains("Fore"); i += 2)
            {
                string[] skills = Regex.Split(allSkills[i + 1], ", ");
                List<Skill> skillList = new List<Skill>();
                foreach(string s in skills)
                {                    
                    skillList.Add(new Skill()
                    {
                        _id = Convert.ToString(Guid.NewGuid()),
                        name = s,
                        type = allSkills[i],
                        isSkillNew = true
                    });
                }
                list.Add(new Tuple<List<Skill>, string>(skillList, allSkills[i]));
                //ToJson.CreateJsonModel(skills, allSkills[i], expList);
            }
            return list;
        }

        private static List<Exp> Parse(string[] array)
        {
            List<Exp> exps = new List<Exp>(); ;
            for(int i = 1; i < array.Length; i += 2)
            {
                array[i] = Regex.Replace(array[i], ",(?=[^()]*\\))", "|");
                string[] expes = Regex.Split(array[i], ", ");
                for(int j = 0; j < expes.Length; j++) expes[j] = expes[j].Replace('|', ',');
                foreach(string s in expes)
                {
                    exps.Add(new Exp()
                    {
                        Name = s,
                        Year = (DateTime.Now.Year - Convert.ToInt32(array[i - 1])).ToString()
                    });
                }
            }
            for(int  i = 0; i < exps.Count - 1; i++)
            {
                for(int  j = i + 1; j < exps.Count; j++)
                {
                    if(exps[i].Name == exps[j].Name)
                    {
                        if (Convert.ToInt32(exps[i].Year) > Convert.ToInt32(exps[j].Year))
                            exps.RemoveAt(j);
                        else
                        {
                            exps.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
            return exps;
        }
    }
}
