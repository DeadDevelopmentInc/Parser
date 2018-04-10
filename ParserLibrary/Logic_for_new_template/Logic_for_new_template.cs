//#define NEW_PARSE_DEBUG_EXP
//#define NEW_PARSE_DEBUG_SKILLS_AND_EXP
using MongoDB.Bson;
using MongoDB.Driver;
using ParserLibrary.Helpers;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_new_template
{
    internal static class Logic_for_new_template
    {
        internal static void Response(Section section, string name)
        {
            try
            {
                //Create list of skills
                List<string> expList = new List<string>();
                var skillsModelList = new List<BufferClass>();
                //Get text from tables
                for (int i = 0; i < 6; i++)
                {
                    List<string> str = AddMethods.GetTextFromTable(section.Tables[i]);
                    skillsModelList.AddRange(ProcessSkills.ProccSkills(str));
                    Console.WriteLine(@"Parse {0} table complete", i + 1);
                    //After reading, create json model
                }
                expList = AddMethods.GetTextFromTable(section.Tables[7]); // table with exp 
#if NEW_PARSE_DEBUG_SKILL
                foreach(string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
#if NEW_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
                Console.WriteLine("Parse complete");
                
                
                var expModelList = ProcessExps.ProccExp(expList);
                expModelList.AddRange(skillsModelList);
#if NEW_PARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList)
                {
                    Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString());
                }
#endif
                AddMethods.DeleteSimilarSkills(ref expModelList);
                AddMethods.CheckLeadSkill(ref expModelList);
                string connectionString = "mongodb://admin:78564523@ds046667.mlab.com:46667/workers_db";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase("workers_db");
                database.CreateCollection(name);
                var colSkills = database.GetCollection<ModelSkill>(name);
                var colLevels = database.GetCollection<SkillLevel>(name + "Lvl");
                var data = AddMethods.ToModelSkills(expModelList);
                colSkills.InsertMany(data.Item1.ToArray());
                colLevels.InsertMany(data.Item2.ToArray());
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        
    }
}