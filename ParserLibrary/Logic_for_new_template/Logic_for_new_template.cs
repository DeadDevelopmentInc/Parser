//#define NEW_PARSE_DEBUG_EXP
//#define NEW_PARSE_DEBUG_SKILLS_AND_EXP
using MongoDB.Driver;
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
                var skillsModelList = new List<ModelSkill>();
                //Get text from tables
                for (int i = 0; i < 6; i++)
                {
                    List<string> str = Helpers.Helpers.GetTextFromTable(section.Tables[i]);
                    //skillsModelList.AddRange(ProcessSkills.ProccSkills(str));
                    Console.WriteLine(@"Parse {0} table complete", i + 1);
                    //After reading, create json model
                }
                expList = Helpers.Helpers.GetTextFromTable(section.Tables[1]); // table with exp 
#if NEW_PARSE_DEBUG_SKILL
                foreach(string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
#if NEW_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
                Console.WriteLine("Parse complete");
                
                
                var expModelList = Helpers.ProcessExps.ProccExp(expList);
#if NEW_PARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList)
                {
                    Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString());
                }
#endif
                //Add(skillsModelList, ref expModelList);
                string connectionString = "mongodb://admin:78564523@ds046667.mlab.com:46667/workers_db";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase("workers_db");
                database.CreateCollection(name);
                var col = database.GetCollection<ModelSkill>(name);
                //col.InsertMany(expModelList.ToArray());
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }
    }
}
