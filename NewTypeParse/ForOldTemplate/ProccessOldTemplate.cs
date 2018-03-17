using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewTypeParse.ForOldTemplate
{
    static class ProccessOldTemplate
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
#if OLD_PARCE_DEBUG
                foreach(string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
                Console.WriteLine("Parse complete");
                //Split and save exp
                ProccExp(ref expList);
                dynamic proccList = SaveSkills(skillsList, expList);
                //Processing and save expearence
                Console.WriteLine("Create json model complete");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }


        /// <summary>
        /// Method for creating json model from old model
        /// </summary>
        /// <param name="obj"> Array with splited skills</param>
        /// <param name="type">type of skills</param>
        public static void CreateJsonModel(string[] obj, string type, List<Exp> expList)
        {
            string year = null;
            //Create list and fill in him
            List<Skill> listSkills = new List<Skill>();
            List<AdditionInfo> listAdd = new List<AdditionInfo>();
            for (int i = 0; i < obj.Length; i++)
            {
                var id = Convert.ToString(Guid.NewGuid());
                listSkills.Add(new Skill()
                {
                    _id = id,
                    name = obj[i],
                    allNames = new string[] { obj[i] },
                    type = type,
                    isSkillNew = true
                });
                if (FindExpOfSkills(listSkills[i].name, expList, ref year))
                {
                    listAdd.Add(new AdditionInfo()
                    {
                        Id = id,
                        Years = year
                    });
                }
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatterSkills = new DataContractJsonSerializer(typeof(List<Skill>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skills/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatterSkills.WriteObject(fs, listSkills);
            }
            //Write exp model
            CreateJsonModelExp(listAdd, type);
        }

        /// <summary>
        /// Split string from old templte
        /// </summary>
        /// <param name="allSkills">All skills</param>
        public static List<Tuple<List<string>, string>> SaveSkills(List<string> allSkills, List<string> expList)
        {
            List<Tuple<List<string>, string>> list = new List<Tuple<List<string>, string>>();
            for (int i = 0; i < allSkills.Count & !allSkills[i].Contains("Fore"); i += 2)
            {
                string[] skills = Regex.Split(allSkills[i + 1], ", ");
                list.Add(new Tuple<List<string>, string>(skills.ToList(), allSkills[i]));
                //ToJson.CreateJsonModel(skills, allSkills[i], expList);
            }
            return list;
        }

        /// <summary>
        /// Method for proccessing readed information
        /// Correctly work only with old template
        /// </summary>
        /// <param name="list"></param>
        public static void ProccExp(ref List<string> list)
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
        }


        public static void CreateJsonModelExp(List<AdditionInfo> list, string type)
        {
            DataContractJsonSerializer jsonFormatterExp = new DataContractJsonSerializer(typeof(List<AdditionInfo>));

            using (FileStream fs = new FileStream("exp/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatterExp.WriteObject(fs, list);
            }
        }


        public static bool FindExpOfSkills(string nameSkills, List<Exp> list, ref string year)
        {
            foreach (Exp ex in list)
            {
                if (nameSkills.Contains(ex.Name))
                {
                    year = ex.Year;
                    list.Remove(ex);
                    return true;
                }
            }
            return false;
        }
    }
}
