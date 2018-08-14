using MongoDB.Bson.Serialization.Attributes;
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
        [BsonElementAttribute("totalit")]
        public int itexperience { get; set; } = 0;
        public List<string> abilities { get; set; }
        [BsonElementAttribute("FirstName")]
        public string fname { get; set; }
        [BsonElementAttribute("MiddleName")]
        public string mname { get; set; }
        [BsonElementAttribute("SecondName")]
        public string lname { get; set; }
        public List<string> passport { get; set; }
        [BsonElementAttribute("totalit")]
        public Date startWork { get; set; }
        [BsonElementAttribute("totalit")]
        public string room { get; set; }
        [BsonElementAttribute("totalit")]
        public string adress { get; set; }
        public string company { get; set; } = "IBA Gomel";
        public string sphere { get; set; }
        public string division { get; set; }
        public string department { get; set; }
        public string sector { get; set; }
        public string position { get; set; }
        [BsonElementAttribute("totalit")]
        public string univer { get; set; }
        public string phones { get; set; }
        public List<Date> vacation { get; set; }
        public string email { get; set; }
        [BsonElementAttribute("totalit")]
        public string lotusLastExt { get; set; }
        public string certifications { get; set; }
        [BsonElementAttribute("totalit")]
        public string forLang { get; set; }
        [BsonElementAttribute("totalit")]
        public string error { get; set; }
        [BsonElementAttribute("totalit")]
        public DateTime lParsed { get; set; } = DateTime.Today;
        [BsonElementAttribute("totalit")]
        public DateTime lUploaded { get; set; }
        [BsonElementAttribute("totalit")]
        public string sParse { get; set; }
        [BsonElementAttribute("totalit")]
        public string degree { get; set; }
        [BsonElementAttribute("totalit")]
        public string educations { get; set; }
        [BsonElementAttribute("totalit")]
        public string exams { get; set; }
        public Date birthDay { get; set; }
        [BsonElementAttribute("totalit")]
        public string personId { get; set; }

        public List<SkillLevel> skills { get; set; }

        public List<UserProject> projects { get; set; }

        public User(string id, List<SkillLevel> levels, List<UserProject> projects, Section section)
        {
            var temp = PostgreDB.GettingPersonalInfoFromDB(id, this);
            abilities = HelpersForParsing.GetAbitiesFromSection(section);
            itexperience = HelpersForParsing.GetITExperienceFromSection(section.Paragraphs[1].Text);


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
    }
}
