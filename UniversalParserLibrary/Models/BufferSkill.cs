using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Buffer class for parsing document
    /// </summary>
    internal class BufferSkill
    {
        public string _id { get; set; }
        public string name { get; set; }
        public string level { get; set; } = "Working knowledge";

        public string Date { get; set; }
        public List<BufferSkill> SimilarSkills { get; set; } = new List<BufferSkill>();

        /// <summary>
        /// Sum levels for similar skills
        /// </summary>
        /// <param name="newLevel">added level</param>
        internal void AddLevel(string newLevel)
        {
            if ((level == "Expert" | level == "Experienced") &
                (newLevel != "Expert" | newLevel != "Experienced")) { level = "Expert"; return; }
            if ((level == "Working knowledge" | level == "Extensive knowledge") &
                (newLevel != "Working knowledge" | newLevel != "Extensive knowledge")) { level = "Experienced"; return; }
            level = "Expert";
        }
    }
}
