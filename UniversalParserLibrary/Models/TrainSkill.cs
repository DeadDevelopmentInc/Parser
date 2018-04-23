using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class TrainSkill
    {
        public string NameOfSkill { get; set; }
        public string CodeOfSkill { get; set; } = null;
        public string TypeOfSkill { get; set; } = null;
        public List<TrainSkill> Skills { get; set; } = new List<TrainSkill>();
    }
}
