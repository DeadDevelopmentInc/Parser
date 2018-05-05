//#define OLD_PARSE_DEBUG_EXP
//#define OLD_PARSE_DEBUG_SKILLS_AND_EXP
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ParserLibrary.Helpers;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_old_template
{
    internal static class Logic_for_old_template
    {
        internal static void Response(Section section, string name)
        {
            try
            {
                //Create list of skills
                List<string> skillsList = new List<string>();
                List<string> expList = new List<string>();
                //Get text from tables
                skillsList = HelpersMethods.GetTextFromTable(section.Tables[0]); //table with skills
                expList = HelpersMethods.GetTextFromTable(section.Tables[1]); // table with exp 
#if OLD_PARSE_DEBUG_SKILL
                foreach(string s in skillsList) { Console.WriteLine(s); }
#endif
#if OLD_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
                Console.WriteLine("Parse complete " + name);
                //Split and save exp
                var expModelList = Helpers.ProcessExps.ProccExp(expList);
                expModelList.AddRange(ProcessSkills.ProccSkills(skillsList));
                //Processing and save expearence
#if OLD_PARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList) { Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString()); }
#endif


                //HelpersMethods.DeleteSimilarSkills(ref expModelList);
                //HelpersMethods.CheckLeadSkill(ref expModelList);
                string connectionString = "mongodb://admin:78564523@ds046667.mlab.com:46667/workers_db";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase("workers_db");
                database.CreateCollection(name);
                var colSkills = database.GetCollection<ModelSkill>(name);
                var colLevels = database.GetCollection<SkillLevel>(name+"Lvl");
                var data = HelpersMethods.ToModelSkills(expModelList);
                colSkills.InsertMany(data.Item1.ToArray());
                colLevels.InsertMany(data.Item2.ToArray());
            }
            catch(Exception ex)
            {
                Console.WriteLine("EXCEPTION HERE " + name + "\n" + ex.Message);
            }
        }
    }
}
