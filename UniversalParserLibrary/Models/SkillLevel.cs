using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Store up level for each skill for each user
    /// </summary>
    internal class SkillLevel
    {
        public string _id { get; set; }
        public string exactName { get; set; }
        public string level { get; set; }
    }
}
