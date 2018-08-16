using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversalParserLibrary.Parsing;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Class for users skill
    /// </summary>
    internal class User
    {
        public string _id { get; set; }
        public int itexperience { get; set; } = 0;
        public List<string> abilities { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public List<string> passport { get; set; }
        public DateTime startWork { get; set; }
        public string room { get; set; }
        public string adress { get; set; }
        public string company { get; set; } = "IBA Gomel";
        public string sphere { get; set; }
        public string division { get; set; }
        public string department { get; set; }
        public string sector { get; set; }
        public string position { get; set; }
        public string univer { get; set; }
        public string phones { get; set; }
        public List<DateTime> vacation { get; set; }
        public string email { get; set; }
        public string lotusLastExt { get; set; }
        public List<string> certifications { get; set; }
        public List<Tuple<string, string>> forLang { get; set; }
        public string error { get; set; }
        public DateTime lParsed { get; set; } = DateTime.Today;
        public DateTime lUploaded { get; set; }
        public string sParse { get; set; }
        public string degree { get; set; }
        public List<string> educations { get; set; } = new List<string>();
        public List<string> exams { get; set; } = new List<string>();
        public DateTime birthDay { get; set; }
        public string personId { get; set; }
        public List<SkillLevel> skills { get; set; }
        public List<UserProject> projects { get; set; }

        public User(string id, List<SkillLevel> levels, List<UserProject> projects, Section section)
        {
            _id = Guid.NewGuid().ToString();
            personId = id;
            this.projects = projects;
            this.skills = levels;
            var temp = PostgreDB.GettingPersonalInfoFromDB(id, this);
            educations = HelpersForParsing.GetEducationsFromSection(section);
            certifications = HelpersForParsing.GetCertificationsFromSection(section);
            abilities = HelpersForParsing.GetAbitiesFromSection(section);
            itexperience = HelpersForParsing.GetITExperienceFromSection(section.Paragraphs[1].Text);
            forLang = HelpersForParsing.GetForLangFromSection(section);

        }

        public User GetUser()
        {
            foreach (UserProject pr in projects)
            {
                string temp = Regex.Replace(pr.Environment, ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(temp, ", ");
                foreach (string s in buff)
                {
                    string buffer = s.Replace("|", ",");
                    var skill = new SkillInProject { exactName = buffer };
                    PrivateDictionary.FindSkill(skill);
                    pr.skills.Add(skill);
                }
            }
            return this;
        }

        public UpdateDefinition<BsonDocument> GetUserBson()
        {
            foreach (UserProject pr in projects)
            {
                string temp = Regex.Replace(pr.Environment, ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(temp, ", ");
                foreach (string s in buff)
                {
                    string buffer = s.Replace("|", ",");
                    var skill = new SkillInProject { exactName = buffer };
                    PrivateDictionary.FindSkill(skill);
                    pr.skills.Add(skill);
                }
            }
            var update = Builders<BsonDocument>.Update.Set("firstname", fname);
            var up = new BsonDocument
            {
                { "firstname", fname },
                { "middlename", mname },
                { "lastname", lname },
                { "passport",  new BsonDocument {
                    { "givenNames", passport[0]},
                    {  "surname", passport[1]} } },
                { "startWorkingDay", startWork},
                { "officeRoomNumber", room},
                { "officeAddress", adress},
                { "sphere", new BsonDocument {
                        { "name", sphere } }
                },
                { "division", new BsonDocument {
                        { "name", division } }
                },
                { "department", new BsonDocument {
                        { "name", department} }
                },
                { "sector", new BsonDocument {
                        { "name", sector} }
                },
                { "company", new BsonDocument {
                        { "name", company } }
                },
                { "position", position},
                { "email", email },
                { "skills", GetSkills() },
                { "universities", GetUniversities() },
                { "phones", GetPhones() },
                { "vacation", new BsonDocument{ { "start_date", vacation[0]},{ "end_date", vacation[1] } } },
                { "resumeParserError", error == null ? new BsonArray(0) : new BsonArray(error)},
                { "resumeLastUpdated", lUploaded},
                { "resumeLastParsed", lParsed},
                { "resumeParsedSource", sParse},
                { "academicDegrees", GetDegrees()},
                { "foreignLanguages", GetForLangs() },
                { "exams", new BsonArray(exams)},
                { "educations", new BsonArray(educations)},
                { "birthDay", birthDay},
                { "projects", GetProj() },
                { "lotuspersonUn", personId},

            };
            foreach(var item in up)
            {
                update = update.Set(item.Name, item.Value);
            }
            return update;
        }

        BsonArray GetUniversities()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            bsonElements.Add(new BsonDocument { { "name", univer } });
            return new BsonArray(bsonElements);
        }

        BsonArray GetPhones()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            string[] buffer = Regex.Split(phones, ", "); //or use ;
            foreach(var b in buffer)
            {
                bsonElements.Add(new BsonDocument { { "Number", b }, { "Type", b.Length > 4 ? "mobile" : "internal_phone" } });
            }
            return new BsonArray(bsonElements);
        }

        BsonArray GetDegrees()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            if (degree != "")
            {
                string[] buffer = Regex.Split(degree, ", "); //or use ;
                foreach (var b in buffer)
                {
                    bsonElements.Add(new BsonDocument { { "name", b } });
                }
            }
            return new BsonArray(bsonElements);
        }

        BsonArray GetForLangs()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            foreach (var b in forLang)
            {
                bsonElements.Add(new BsonDocument { { "name", b.Item1 }, { "level", b.Item2} });
            }
            return new BsonArray(bsonElements);
        }

        BsonArray GetSkills()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            foreach (var b in skills)
            {
                bsonElements.Add(new BsonDocument
                {
                    { "skill", b._id },
                    { "experience", b.level },
                    { "change", "ParsedResume" },
                    //{ "lastUsedDate", b.lastUs == null ? BsonNull.Value : b.lastUs},
                    { "exactName", b.exactName}
                });
            }
            return new BsonArray(bsonElements);
        }

        BsonArray GetCert()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            foreach (var b in certifications)
            {
                bsonElements.Add(new BsonDocument { { "name", b }, { "change", "ParsedResume"} });
            }
            return new BsonArray(bsonElements);
        }

        BsonArray GetProj()
        {
            List<BsonDocument> bsonElements = new List<BsonDocument>();
            foreach(var pr in projects)
            {
                List<BsonDocument> tskills = new List<BsonDocument>();
                foreach (var sk in pr.skills)
                {
                    SkillLevel s = new SkillLevel();
                    foreach(var t in this.skills) { if (sk._id == t._id) s = t; }
                    if(s._id == null) { s._id = sk._id; s.exactName = sk.exactName; s.level = "Working knoledge"; PrivateDictionary.globalSkills.Add(new Skill(s._id, s.exactName, new List<string>(), "", true)); }
                    tskills.Add(new BsonDocument
                    {
                        { "skill", sk._id},
                        { "change", "ParsedResume"},
                        { "experience",  s.level},
                        { "lastUsedDate", s.lastUs},
                        { "exactname", s.exactName }
                    });
                }
                bsonElements.Add(new BsonDocument
                {
                    { "id", pr._id},
                    { "name", pr.name},
                    { "role", pr.role},
                    { "responsibility", pr.responsibility },
                    { "startProjectDate", pr.startProjectDate},
                    { "endProjectDate ", pr.endProjectDate },
                    { "change", "ParsedResume"},
                    { "result", pr.result},
                    { "skills", new BsonArray(tskills)},
                    { "duration", new BsonDocument{ { "measure", "m."}, { "value", pr.GetDuration()} } }
                });
            }
            return new BsonArray(bsonElements);
        }
    }
}
