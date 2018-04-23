using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class Skill
    {
        public string _id { get; set; }
        public string name { get; set; }
        public List<string> allNames { get; set; } = new List<string>();
        public string type { get; set; } = "Other";
        public bool isSkillNew { get; set; } = true;

        public Skill()
        { }

        public Skill(string id, string name, List<string> allNames, string type, bool isNew)
        {
            _id = id;
            this.name = name;
            this.type = type;
            isSkillNew = isNew;
            this.allNames = allNames;
        }

        public void Print()
        {
            Console.WriteLine(_id.ToString() + ": " + name.ToString() + "\n\t");
        }
    }
}
