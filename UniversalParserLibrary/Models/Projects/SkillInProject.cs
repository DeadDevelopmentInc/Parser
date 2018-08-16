using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class SkillInProject
    {
        public string _id { get; set; } = Guid.NewGuid().ToString();
        public string exactName { get; set; }
    }
}
