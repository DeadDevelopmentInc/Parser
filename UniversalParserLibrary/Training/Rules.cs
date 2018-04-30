﻿using MongoDB.Driver;
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
        static Rule rule { get; set; } = new Rule();
        static string connectionString = @"mongodb://admin:78564523@ds014578.mlab.com:14578/workers_db";

        static Rules()
        {
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("workers_db");
            var collection = database.GetCollection<Rule>("Rules_for_parser");
            rule = collection.Find(Builders<Rule>.Filter.Empty).Single();
        }

        internal static TrainSkill CreateRules(TrainSkill skill)
        {
            skill.CodeOfSkill = skill.NameOfSkill;
            foreach(var ru in rule.rules)
            {
                skill.CodeOfSkill = skill.CodeOfSkill.ToLower();
                if(skill.CodeOfSkill.Contains("("))
                {
                    skill.CodeOfSkill = skill.CodeOfSkill.Remove(skill.CodeOfSkill.IndexOf('('), skill.CodeOfSkill.IndexOf(')') - skill.CodeOfSkill.IndexOf('(') + 1);
                }
                if (skill.CodeOfSkill.Contains(ru.Item1)) { skill.CodeOfSkill = skill.CodeOfSkill.Replace(ru.Item1, ru.Item2); }
            }
            return skill;
        }
    }
}