using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Training;
using UniversalParserLibrary.Models.Algorithms;

namespace UniversalParserLibrary.Models
{
    internal class Project
    {
        public string _id { get; set; }
        public string name { get; set; }
        [BsonIgnore]
        public string code { get; set; }
        public string customer { get; set; }
        public string activity { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string result { get; set; }

        internal static List<Project> FindSimpleProjects(List<Project> projects)
        {
            for(int i = 0; i < projects.Count; i++) { projects[i] = Rules.CreateRules(projects[i]); }
            DiceCoefficientExtensions.Start(projects);
            return projects;
        }

        public void MoveDate(Project adds)
        {
            if(startDate > adds.startDate) { startDate = adds.startDate; }
            if(endDate < adds.endDate) { endDate = adds.endDate; }
        }
    }
}
