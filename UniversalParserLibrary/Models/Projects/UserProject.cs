using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    class UserProject
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string sourceCompany { get; set; }
        public string responsibility { get; set; }
        [BsonElementAttribute("startProjectDate")]
        public DateTime startProjectDate { get; set; }
        [BsonElementAttribute("endProjectDate")]
        public DateTime endProjectDate { get; set; }
        public string result { get; set; }
        public List<SkillInProject> skills { get; set; } = new List<SkillInProject>();
        [BsonIgnore]
        public string Environment { get; set; }

        public int GetDuration()
        {
            int duration = 0;
            duration = Math.Abs((startProjectDate.Month - endProjectDate.Month) + 12 * (startProjectDate.Year - endProjectDate.Year));
            return duration;
        }
    }
}
