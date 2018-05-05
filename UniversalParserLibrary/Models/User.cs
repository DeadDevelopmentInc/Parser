using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Class for users skill
    /// </summary>
    internal class User
    {
        public string _id { get; set; }

        public List<SkillLevel> skills { get; set; }
    }
}
