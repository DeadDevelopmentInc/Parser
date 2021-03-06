﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;
using UniversalParserLibrary.Helpers;
using Spire.Doc;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Bson;
using UniversalParserLibrary.Parsing;
using Spire.Doc.Documents;

namespace UniversalParserLibrary.Parsing
{
    internal static class LogicForParsing
    {
        internal static List<Project> ProjectsList { get; set; } = new List<Project>();
        private static object locker = new object();

        public static void NewParse(string destination, string name)
        {
            Document doc = new Document();
            doc.LoadFromFile(destination);
            //Find section with table
            Section section = doc.Sections[0];
            var te = section.Paragraphs;
            List<string> vs = new List<string>();
            foreach(Paragraph t in te) { vs.Add(t.Text); }
            List<BufferSkill> skills = new List<BufferSkill>();
            List<UserProject> projects = new List<UserProject>();
            try
            {      
                new Models.Exceptions_and_Events.Info("Start Parsing", "INFO", "current user", name.Replace(".doc", ""), 1);
                //Get type of template
                int type = section.Tables.Count;
                dynamic temp = null;
                switch (type)
                {
                    case 8: { temp = ParseSectionFromNewTemplate(section); } break;
                    case 2: { temp = ParseSectionFromOldTemplate(section); } break;
                }
                unchecked { skills = ParseTemplate(temp.Item1, temp.Item2); projects = temp.Item2; }
                
            }
            catch (Exception e) { new Models.Exceptions_and_Events.Exception("Resume Parsing", "ERROR", e.Message, name.Replace(".doc", "")); }
            finally { new Models.Exceptions_and_Events.Info("Finish Parsing", "INFO", "current user", name.Replace(".doc", ""), 1); SendDataToDB(name, skills, projects, section); }


        }

        private static void ProcessBufferSkills(List<BufferSkill> skills)
        {
            List<SkillLevel> mainList = new List<SkillLevel>();
            foreach(BufferSkill skill in skills)
            {
                mainList.Add(new SkillLevel {
                    _id = skill._id,
                    level = skill.level
                });
            }
        }

        private static List<BufferSkill> ParseTemplate(List<BufferSkill> skills, List<UserProject> projects)
        {
            DeleteSimpleSkills(ref skills);
            return skills;
        }


        private static Tuple<List<BufferSkill>, List<UserProject>> ParseSectionFromNewTemplate(Section section)
        {
            var bufferProjects = new List<BufferProject>();            
            var skills = Readers.GetExpsFromOldTable(section.Tables[7], bufferProjects);
            for (int i = 0; i < 6; i++) { skills.AddRange(Readers.GetSkillsFromNewTable(section.Tables[i])); }
            return CreateTuple(skills, bufferProjects);
        }

        private static Tuple<List<BufferSkill>, List<UserProject>> ParseSectionFromOldTemplate(Section section)
        {
            var bufferProjects = new List<BufferProject>();            
            var skills = Readers.GetExpsFromOldTable(section.Tables[1], bufferProjects);            
            skills.AddRange(Readers.GetSkillsFromOldTable(section.Tables[0]));
            return CreateTuple(skills, bufferProjects);
        }
        
        private static Tuple<List<BufferSkill>, List<UserProject>> CreateTuple(List<BufferSkill> bufferSkills, List<BufferProject> bufferProjects)
        {
            var userProjects = new List<UserProject>();
            var projects = new List<Project>();
            foreach (BufferProject pr in bufferProjects)
            {
                userProjects.Add(new UserProject
                {
                    _id = pr._id,
                    name = pr.name,
                    role = pr.role,
                    responsibility = pr.responsibility,
                    sourceCompany = pr.sourceCompany,
                    startProjectDate = pr.startDate,
                    endProjectDate = pr.endDate,
                    result = pr.result,
                    Environment = pr.Environment,
                });
                projects.Add(new Project
                {
                    _id = pr._id,
                    name = pr.name,
                    activity = pr.activity,
                    customer = pr.customer,
                    result = pr.result,
                    startDate = pr.startDate,
                    endDate = pr.endDate
                });
            }
            AddToList(projects);
            return new Tuple<List<BufferSkill>, List<UserProject>>(bufferSkills, userProjects);
        }

        private static void DeleteSimpleSkills(ref List<BufferSkill> skills)
        {
            PrivateDictionary.FindAllСoncurrencesInTemplate(ref skills);
            for(int i = 0; i < skills.Count; i++)
            {
                if (skills[i].Date != null) { skills[i].SimilarSkills.Add(skills[i]); }
                skills[i].level = FindLevel(skills[i].SimilarSkills);
            }
        }

        private static string FindLevel(List<BufferSkill> skills)
        {
            List<Tuple<Date, Date >> list = new List<Tuple<Date , Date >>();
            Regex date = new Regex(@"^\w+\s\d{4}\s\S\s\w+\s\d{4}$");
            foreach (BufferSkill s in skills)
            {
                MatchCollection match = date.Matches(s.Date);
                if (match.Count > 0)
                {
                    string[] dates = s.Date.Split(' ');
                    list.Add(new Tuple<Date , Date >(
                        new Date () { Month = dates[0], Year = dates[1] },
                        new Date () { Month = dates[3], Year = dates[4] }));
                }
                else
                {
                    string[] dates = s.Date.Split(' ');
                    list.Add(new Tuple<Date , Date >(
                        new Date () { Month = dates[0], Year = dates[1] },
                        new Date () { MonthInt = DateTime.Now.Month, YearInt = DateTime.Now.Year }));
                }
            }
            return CalculateDate(list);
        }

        private static string CalculateDate(List<Tuple<Date , Date >> list)
        {
            double date = 0;
            //Event a
            for (int i = 0; i < list.Count - 1; i++)
            {
                //Event b
                for (int j = i + 1; j < list.Count; j++)
                {

                    //If events a and b didn't cross
                    if (list[j].Item2.NotIntersect(list[i].Item1) |
                        list[i].Item2.NotIntersect(list[j].Item1))
                    { continue; }
                    //Well then need find best way
                    else
                    {
                        //When event b happened before a and their cross
                        if (list[j].Item1.NotIntersect(list[i].Item1))
                        { list[i].Item1.Update(list[j].Item1); }
                        //If event b happened after a and their cross
                        if (list[i].Item2.NotIntersect(list[j].Item2))
                        { list[i].Item2.Update(list[j].Item2); }

                        list.RemoveAt(j);
                        j--;
                    }

                }
            }
            foreach (var n in list)
            {
                date += n.Item1.GetLenght(n.Item2);
            }
            return GetLevel(Convert.ToInt32(date));
        }

        /// <summary>
        /// Get level from number of year
        /// </summary>
        /// <param name="year">Summary years</param>
        /// <returns>Level of years</returns>
        private static string GetLevel(int year)
        {
            if (year <= 1) return "Working knowledge";
            else if (year <= 2) return "Extensive knowledge";
            else if (year <= 4) return "Experienced";
            else return "Expert";
        }

        private static List<SkillLevel> ProcessDataForDB(List<BufferSkill> skills)
        {
            List<SkillLevel> levels = new List<SkillLevel>();
            List<BufferSkill> addsSkills = new List<BufferSkill>();
            foreach(BufferSkill skill in skills)
            {
                if(skill.AllSkills.Count != 0)
                {
                    for(int i = 0; i < skill.AllSkills.Count - 1; i++)
                    {
                        for (int j = i + 1; j < skill.AllSkills.Count; j++)
                        {
                            if (skill.AllSkills[i].name == skill.AllSkills[j].name) { skill.AllSkills.Remove(skill.AllSkills[j]); j--; }
                        }
                    }
                    foreach(var skil in skill.AllSkills)
                    {
                        if(skil.name != skill.name)
                        {
                            skil.level = skill.level;
                            addsSkills.Add(skil);
                        }
                    }
                }
            }
            skills.AddRange(addsSkills);
            foreach(BufferSkill skill in skills)
            {
                if (skill.name != "")
                {
                    levels.Add(new SkillLevel
                    {
                        _id = skill._id,
                        level = skill.level,
                        exactName = skill.name
                    });
                }
            }
            return levels;
        }
        
        private static void SendDataToDB(string name, List<BufferSkill> skills, List<UserProject> projects, Section section)
        {
            try
            {
                name = name.Remove(name.IndexOf(".doc"), 4);
                List<SkillLevel> levels = ProcessDataForDB(skills);
                MongoClient client = new MongoClient(Properties.Settings.Default.connectionStringMongo);
                IMongoDatabase database = client.GetDatabase("ems");
                var colUsers = database.GetCollection<BsonDocument>("users");
                UpdateOptions updateOptions = new UpdateOptions { IsUpsert = true };
                User user = new User(name, levels, projects, section);
                FilterDefinition<BsonDocument> builders = Builders<BsonDocument>.Filter.Eq("personId", name);
                colUsers.UpdateOne(builders, user.GetUserBson(), updateOptions);
                new Models.Exceptions_and_Events.Info("sending data","INFO","document " + name +" succesesfull send in db", 1);
            }
            /*Exception e - from System Exception
              new Exception({params}) - from Exeptions_and_Events*/
            catch(Exception e) { new Models.Exceptions_and_Events.Exception("writing in db", "ERROR", e.Message); }

        }

        private static void AddToList(List<Project> projects)
        {
            lock (locker) { ProjectsList.AddRange(projects); }
        }
    }
}
