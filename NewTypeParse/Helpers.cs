using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewTypeParse
{
    class Helpers
    {
        /// <summary>
        /// Split string from old templte
        /// </summary>
        /// <param name="allSkills">All skills</param>
        public static void SplitSkills(List<string> allSkills)
        {
            for(int i = 0; i < allSkills.Count & !allSkills[i].Contains("Fore"); i+=2)
            {
                string[] skills = Regex.Split(allSkills[i+1], ", ");
                ToJson.CreateJsonModel(skills, allSkills[i]);
            }
        }
    }
}
