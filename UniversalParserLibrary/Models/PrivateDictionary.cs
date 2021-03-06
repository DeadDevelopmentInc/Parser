﻿using MongoDB.Bson;
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
        /// <summary>
        /// Admin string for dictionary
        /// </summary>
        internal static List<Skill> globalSkills { get; set; } = new List<Skill>();
        internal static List<Project> globalProjects { get; set; } = new List<Project>();
        static object locker = new object();

        static PrivateDictionary()
        {
            globalSkills = GetDataFromDB<Skill>("skills");
            foreach(var skill in globalSkills) { skill.isSkillNew = false; }
            globalProjects = GetDataFromDB<Project>("projects");
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
        internal static void UpdateDictionarySkills()
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
            IMongoDatabase database = client.GetDatabase("ems");
            var collection = database.GetCollection<Skill>("skills");
            collection.DeleteMany(Builders<Skill>.Filter.Empty);
            foreach (var sk in globalSkills)
            { 
                collection.ReplaceOne(Builders<Skill>.Filter.Eq(r => r._id, sk._id), sk, new UpdateOptions { IsUpsert = true });
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projects"></param>
        internal static void SendProjects(List<Project> projects)
        {
            Project.FindSimpleProjects(projects);
            MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
            IMongoDatabase database = client.GetDatabase("ems");
            var collection = database.GetCollection<Project>("projects");
            collection.DeleteMany(Builders<Project>.Filter.Empty);
            if(projects.Count != 0)
                foreach (var sk in globalProjects)
                {
                    collection.ReplaceOne(Builders<Project>.Filter.Eq(r => r._id, sk._id), sk, new UpdateOptions { IsUpsert = true });
                }
        }

        internal static List<T> GetDataFromDB<T>(string collectionName)
        {
            int suc = 0;
            try
            {
                MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
                IMongoDatabase database = client.GetDatabase("ems");
                var collection = database.GetCollection<T>(collectionName);
                suc = 1;
                return collection.Find(Builders<T>.Filter.Empty).ToList();
            }
            catch(Exception e) { new Models.Exceptions_and_Events.Exception("getting data from db", "ERROR", e.Message); return new List<T>(); }
            finally { new Exceptions_and_Events.Info("getting data from db", "INFO", "getted " + collectionName +" data", suc); }
        }

        /// <summary>
        /// Update dictionary with local skills
        /// </summary>
        /// <param name="skills">local skills, replay global skills</param>
        internal static void UpdateDictionarySkills(List<Skill> skills)
        {
            MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
            IMongoDatabase database = client.GetDatabase("ems");
            var collection = database.GetCollection<Skill>("skills");
            collection.DeleteMany(Builders<Skill>.Filter.Empty);
            collection.InsertMany(skills.ToArray());
            globalSkills = skills;
        }

        /// <summary>
        /// Find all concurrences in template
        /// </summary>
        /// <param name="skills"></param>
        internal static void FindAllСoncurrencesInTemplate(ref List<BufferSkill> skills)
        {
            for(int i = 0; i < skills.Count; i++) { skills[i] = FindSkill(skills[i]); }
            for(int i = 0; i < skills.Count - 1; i++)
            {
                for(int j = i + 1; j < skills.Count;)
                {
                    if (skills[i]._id == skills[j]._id)
                    {
                        if(skills[j].Date != null) { skills[i].SimilarSkills.Add(skills[j]); }
                        skills[i].AllSkills.Add(skills[j]);
                        skills.RemoveAt(j);
                    }
                    else { j++; }
                }
            }
        }

        private static BufferSkill FindSkill(BufferSkill skill)
        {
            lock(locker)
            {
                foreach (Skill globalSkill in globalSkills)
                {
                    if (SimpleSkill(globalSkill, skill.name)) { skill._id = globalSkill._id; return skill; }
                }
                lock (locker)
                {
                    globalSkills.Add(new Skill
                    {
                        name = skill.name,
                        _id = skill._id = Convert.ToString(Guid.NewGuid())
                    });
                }
            }            
            return skill;
        }

        internal static void FindSkill(SkillInProject skill)
        {
            lock(locker)
            {
                foreach (Skill globalSkill in globalSkills)
                {
                    if (SimpleSkill(globalSkill, skill.exactName)) { skill._id = globalSkill._id; }
                }
            }
            
        }
        
        private static bool SimpleSkill(Skill skill, string name)
        {
            if (skill.name == name) { return true; }
            foreach(string s in skill.allNames) { if (s == name) { return true; } }
            return false;
        }
    }

}
