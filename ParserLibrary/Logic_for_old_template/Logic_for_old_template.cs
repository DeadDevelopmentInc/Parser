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
                skillsList = Helpers.Helpers.GetTextFromTable(section.Tables[0]); //table with skills
                expList = Helpers.Helpers.GetTextFromTable(section.Tables[1]); // table with exp 
#if OLD_PARSE_DEBUG_SKILL
                foreach(string s in skillsList) { Console.WriteLine(s); }
#endif
#if OLD_PARSE_DEBUG_EXP
                foreach(string s in expList) { Console.WriteLine(s); }
#endif
                Console.WriteLine("Parse complete");
                //Split and save exp
                var expModelList = Helpers.ProcessExps.ProccExp(expList);
                expModelList.AddRange(ProcessSkills.ProccSkills(skillsList));
                //Processing and save expearence
#if OLD_PARSE_DEBUG_SKILLS_AND_EXP
                foreach (ModelSkill s in skillsModelList) { Console.WriteLine(s.name + " " + s.level + " " + s.type + " " + s.allNames.Count.ToString()); }
#endif


                DeleteSimilarSkills(ref expModelList);
                
                string connectionString = "mongodb://admin:78564523@ds046667.mlab.com:46667/workers_db";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase("workers_db");
                database.CreateCollection(name);
                var colSkills = database.GetCollection<ModelSkill>(name);
                var colLevels = database.GetCollection<SkillLevel>(name+"Lvl");
                var data = ToModelSkills(expModelList);
                colSkills.InsertMany(data.Item1.ToArray());
                colLevels.InsertMany(data.Item2.ToArray());
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        private static void DeleteSimilarSkills(ref List<BufferClass> expModelList)
        {
            for(int i = 0; i < expModelList.Count - 1; i++)
            {
                for(int j = i + 1; j < expModelList.Count; j++)
                {
                    if (PrivateDictionary.CheckTwoValues(expModelList[i].name, expModelList[j].name))
                    {
                        if(expModelList[i].name == expModelList[j].name) { expModelList.RemoveAt(j); continue; }
                        else if(expModelList[i].allNames.Contains(expModelList[j].name)) { expModelList.RemoveAt(j); continue; }
                        expModelList[i].allNames.Add(expModelList[j].name);
                        expModelList[i].AddLevel(expModelList[j].level);
                        expModelList[i].type = PrivateDictionary.GetTypeTechByKey(expModelList[i].name);
                        expModelList.RemoveAt(j);
                    }
                }
            }
        }

        internal static bool ExName(List<string> array, string name)
        {
            foreach(string s in array)
            {
                if (s.Equals(name)) return true;
            }
            return false;
        }

        private static bool ExCollection(List<BsonDocument> bsons, string name)
        {
            foreach(BsonDocument doc in bsons)
            {
                if(doc["name"] == name) { return true; }
            }
            return false;
        }
        
        private static Tuple<List<ModelSkill>, List<SkillLevel>> ToModelSkills(List<BufferClass> buffer)
        {
            List<ModelSkill> skills = new List<ModelSkill>();
            List<SkillLevel> levels = new List<SkillLevel>();
            foreach(BufferClass bc in buffer)
            {
                string id = Convert.ToString(Guid.NewGuid());
                skills.Add(new ModelSkill
                {
                    _id = id,
                    name = bc.name,
                    type = bc.type,
                    allNames = bc.allNames
                });
                levels.Add(new SkillLevel
                {
                    _id = id,
                    level = bc.level
                });
            }
            return new Tuple<List<ModelSkill>, List<SkillLevel>>(skills, levels);
        }
    }
}
