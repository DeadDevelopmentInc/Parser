using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;
using UniversalParserLibrary.Helpers;
using Spire.Doc;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace UniversalParserLibrary.Parsing
{
    internal static class LogicForParsing
    {
        public static void NewParse(string destination, string name)
        {
            try
            {
                List<BufferSkill> skills = new List<BufferSkill>();
                Document doc = new Document();
                doc.LoadFromFile(destination);
                //Find section with table
                Section section = doc.Sections[0];
                Console.WriteLine("Complete read " + destination + " file");
                //Get type of template
                int type = doc.Sections[0].Tables.Count;
                switch (type)
                {
                    case 8: { skills = ParseNewTemplate(ParseSectionFromNewTemplate(section)); } break;
                    case 2: { skills = ParseOldTemplate(ParseSectionFromOldTemplate(section)); } break;
                }
                SendDataToDB(name, skills);

            }
            catch(Exception e) { Console.WriteLine(e.Message); }
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

        private static List<BufferSkill> ParseNewTemplate(List<BufferSkill> skills)
        {
            DeleteSimpleSkills(ref skills);
            return skills;
        }

        private static List<BufferSkill> ParseOldTemplate(List<BufferSkill> skills)
        {
            DeleteSimpleSkills(ref skills);
            return skills;
        }

        private static List<BufferSkill> ParseSectionFromNewTemplate(Section section)
        {
            var skills = Readers.GetExpsFromOldTable(section.Tables[7]);
            for (int i = 0; i < 7; i++) { skills.AddRange(Readers.GetSkillsFromNewTable(section.Tables[i])); }

            return skills;
        }

        private static List<BufferSkill> ParseSectionFromOldTemplate(Section section)
        {
            var skills = Readers.GetExpsFromOldTable(section.Tables[1]);
            skills.AddRange(Readers.GetSkillsFromOldTable(section.Tables[0]));
            return skills;
        }
        
        private static void DeleteSimpleSkills(ref List<BufferSkill> skills)
        {
            PrivateDictionary.FindAllСoncurrencesInOldTemplate(ref skills);
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
            foreach(BufferSkill skill in skills)
            {
                levels.Add(new SkillLevel {
                    _id = skill._id,
                    level = skill.level
                });
            }
            return levels;
        }

        private static void SendDataToDB(string name, List<BufferSkill> skills)
        {
            List<SkillLevel> levels = ProcessDataForDB(skills);
            string connectionString = "mongodb://admin:78564523@ds014578.mlab.com:14578/workers_db";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("workers_db");
            var colSkills = database.GetCollection<User>("users");
            colSkills.InsertOne(new User { _id = name, skills = levels});
        }
    }
}
