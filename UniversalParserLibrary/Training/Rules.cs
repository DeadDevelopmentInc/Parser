using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;

namespace UniversalParserLibrary.Training
{
    internal static class Rules
    {
        static bool newCreate = true;
        static Rule rule { get; set; } = new Rule();

        static Rules()
        {
            if(newCreate)
            {
                MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo.ToString());
                IMongoDatabase database = client.GetDatabase("ems");
                var collection = database.GetCollection<Rule>("rules_for_parser");
                rule = collection.Find(Builders<Rule>.Filter.Empty).Single();
            }
            //try
            //{
            //    MongoClient client = new MongoClient(connectionString);
            //    IMongoDatabase database = client.GetDatabase("ems");
            //    var collection = database.GetCollection<Rule>("Rules_for_parser");
            //    rule = collection.Find(Builders<Rule>.Filter.Empty).Single();
            //}
            //catch(Exception e) { new Models.Exceptions_and_Events.Exception("", "", e.Message); }
        }

        internal static TrainSkill CreateRules(TrainSkill skill)
        {
            skill.CodeOfSkill = skill.NameOfSkill;
            skill.CodeOfSkill = skill.CodeOfSkill.ToLower();
            if (skill.CodeOfSkill.Contains("("))
            {
                skill.CodeOfSkill = skill.CodeOfSkill.Remove(skill.CodeOfSkill.IndexOf('('), skill.CodeOfSkill.IndexOf(')') - skill.CodeOfSkill.IndexOf('(') + 1);
            }
            foreach (var ru in rule.rules)
            {
                if (skill.CodeOfSkill.Contains(ru.Item1)) { skill.CodeOfSkill = skill.CodeOfSkill.Replace(ru.Item1, ru.Item2); }
            }
            return skill;
        }

        internal static Project CreateRules(Project project)
        {
            project.code = project.name;
            project.code = project.code.ToLower();
            if (project.code.Contains("("))
            {
                project.code = project.code.Remove(project.code.IndexOf('('), project.code.IndexOf(')') - project.code.IndexOf('(') + 1);
            }
            foreach (var ru in rule.rules)
            {
                if (project.code.Contains(ru.Item1)) { project.code = project.code.Replace(ru.Item1, ru.Item2); }
            }
            return project;
        }
    }
}
