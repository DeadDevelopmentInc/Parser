using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class User
    {
        public string _id { get; set; }

        public List<SkillLevel> Levels { get; set; }
    }
}
