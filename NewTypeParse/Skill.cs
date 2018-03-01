using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// Two model for skills from old and new template
/// </summary>
namespace NewTypeParse
{
    [DataContract]
    public class Skill
    {
        [DataMember]
        public string _id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string[] allNames { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public bool isSkillNew { get; set; }

        public Skill()
        { }

        public Skill(string id, string name, string[] allNames, string type, bool isNew)
        {
            _id = id;
            this.name = name;
            this.type = type;
            isSkillNew = isNew;
            this.allNames = allNames;
        }
    }


    [DataContract]
    public class SkillNew
    {
        [DataMember]
        public string _id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string level { get; set; }
        [DataMember]
        public string[] allNames { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public bool isSkillNew { get; set; }

        public SkillNew()
        { }

        public SkillNew(string id, string name, string[] allNames, string level, string type, bool isNew)
        {
            _id = id;
            this.name = name;
            this.type = type;
            isSkillNew = isNew;
            this.allNames = allNames;
            this.level = level;
        }
    }
}
