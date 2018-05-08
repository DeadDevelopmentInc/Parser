using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Class for proccecing dictionary with skills
    /// </summary>
    internal static class PrivateDictionary
    {
        const string connectionAdmin = @"mongodb://admin:78564523@ds014578.mlab.com:14578/workers_db";
        internal static List<Skill> globalSkills { get; set; } = new List<Skill>();
        static object locker = new object();

        static PrivateDictionary()
        {
            globalSkills = GetDataFromDB();
        }

        internal static void PrintDictionary()
        {
            foreach(Skill skill in globalSkills)
            {
                skill.Print();
            }
        }
        
        /// <summary>
        /// Update dictionary with global skills
        /// </summary>
        internal static void UpdateDictionary()
        {
            MongoClient client = new MongoClient(connectionAdmin);
            IMongoDatabase database = client.GetDatabase("workers_db");
            var collection = database.GetCollection<Skill>("skills");
            collection.DeleteMany(Builders<Skill>.Filter.Empty);
            collection.InsertMany(globalSkills.ToArray());

        }

        internal static List<Skill> GetDataFromDB()
        {
            MongoClient client = new MongoClient(connectionAdmin);
            IMongoDatabase database = client.GetDatabase("workers_db");
            var collection = database.GetCollection<Skill>("skills");
            return collection.Find(Builders<Skill>.Filter.Empty).ToList();
        }

        /// <summary>
        /// Update dictionary with local skills
        /// </summary>
        /// <param name="skills">local skills, replay global skills</param>
        internal static void UpdateDictionary(List<Skill> skills)
        {
            MongoClient client = new MongoClient(connectionAdmin);
            IMongoDatabase database = client.GetDatabase("workers_db");
            var collection = database.GetCollection<Skill>("skills");
            collection.DeleteMany(Builders<Skill>.Filter.Empty);
            collection.InsertMany(skills.ToArray());
            globalSkills = skills;
        }

        /// <summary>
        /// Find all concurrences in new template
        /// </summary>
        /// <param name="skills"></param>
        internal static void FindAllСoncurrencesInNewTemplate(ref List<BufferSkill> skills)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                skills[i] = FindSkill(skills[i]);
            }
            for (int i = 0; i < skills.Count - 1; i++)
            {
                for (int j = i + 1; j < skills.Count; j++)
                {
                    if (skills[i]._id == skills[j]._id)
                    {
                        if (skills[j].Date != null) { skills[i].AddLevel(skills[j].level); }
                        skills[i].AllSkills.Add(skills[j]);
                        skills.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// Find all concurrences in old template
        /// </summary>
        /// <param name="skills"></param>
        internal static void FindAllСoncurrencesInOldTemplate(ref List<BufferSkill> skills)
        {
            for(int i = 0; i < skills.Count; i++)
            {
                skills[i] = FindSkill(skills[i]);
            }
            for(int i = 0; i < skills.Count - 1; i++)
            {
                for(int j = i + 1; j < skills.Count; j++)
                {
                    if (skills[i]._id == skills[j]._id)
                    {
                        if(skills[j].Date != null) { skills[i].SimilarSkills.Add(skills[j]); }
                        skills[i].AllSkills.Add(skills[j]);
                        skills.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        private static BufferSkill FindSkill(BufferSkill skill)
        {
            foreach(Skill globalSkill in globalSkills)
            {
                if(SimpleSkill(globalSkill, skill.name)) { skill._id = globalSkill._id;  return skill; }
            }
            lock(locker)
            {
                globalSkills.Add(new Skill
                {
                    name = skill.name,
                    _id = skill._id = Convert.ToString(Guid.NewGuid())
                });
            }
            return skill;
        }

        private static bool SimpleSkill(Skill skill, string name)
        {
            if (skill.name == name) { return true; }
            foreach(string s in skill.allNames) { if (s == name) { return true; } }
            return false;
        }
    }

}
