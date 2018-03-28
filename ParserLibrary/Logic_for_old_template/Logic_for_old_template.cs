//#define OLD_PARSE_DEBUG_EXP
//#define OLD_RARSE_DEBUG_SKILLS_AND_EXP
using MongoDB.Driver;
using Spire.Doc;
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
#if OLD_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
                Console.WriteLine("Parse complete");
                //Split and save exp
                var skillsModelList = ProcessSkills.ProccSkills(skillsList);
                var expModelList = ProcessExps.ProccExp(expList);
#if OLD_RARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList)
                {
                    Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString());
                }
#endif
                //Processing and save expearence
#if OLD_RARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList)
                {
                    Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString());
                }
#endif


                //string connectionString = "mongodb://admin:78564523@ds046667.mlab.com:46667";
                //MongoClient client = new MongoClient(connectionString);
                //IMongoDatabase database = client.GetDatabase("workers_db");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        private static List<ModelSkill> Split(List<ModelSkill> skillsModelList,
            List<ModelSkill> expModelList)
        {
            foreach(ModelSkill exp in expModelList)
            {
                for(int i = 0; i < skillsModelList.Count; i++)
                {
                    if(exp.name.Contains(skillsModelList[i].name))
                    {
                        if (exp.name != skillsModelList[i].name) exp.allNames.Add(skillsModelList[i].name);
                        exp.type = skillsModelList[i].type;
                        skillsModelList.RemoveAt(i);
                    }
                }
            }
            return expModelList;
        }

        
    }
}
