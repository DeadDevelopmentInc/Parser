using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public List<SkillLevel> skills { get; set; }

        public List<UserProject> projects { get; set; }

        public User GetUser()
        {
            foreach(UserProject pr in projects)
            {
                string[] buff = Regex.Split(pr.Environment, ", ");
                foreach(string s in buff)
                {
                    var skill = new SkillInProject { exactName = s };
                    PrivateDictionary.FindSkill(skill);
                    pr.skills.Add(skill);
                }
            }
            return this;
        }
    }
}
