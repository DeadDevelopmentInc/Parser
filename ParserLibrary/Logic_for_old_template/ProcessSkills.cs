using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_old_template
{
    internal static class ProcessSkills
    {
        /// <summary>
        /// Split string from old templte
        /// </summary>
        /// <param name="allSkills">string list with all skills</param>
        /// <returns>List with model of skills, ready for writing in db</returns>
        internal static List<ModelSkill> ProccSkills(List<string> allSkills)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            for (int i = 1; i < allSkills.Count - 1 & !allSkills[i - 1].Contains("Fore"); i += 2)
            {
                //If skills line contain struct (,)
                if (allSkills[i].Contains("(")) { list.AddRange(SplitLineSkills(allSkills[i], allSkills[i - 1])); }
                else { list.AddRange(SplitLineSkills(Regex.Split(allSkills[i], ", "), allSkills[i - 1])); }
            }
            return list;
        }

        /// <summary>
        /// Splitting line with diffclt strct
        /// </summary>
        /// <param name="line">Line with skills</param>
        /// <param name="type">Type of skills</param>
        /// <returns>List of model of skills</returns>
        private static List<ModelSkill> SplitLineSkills(string line, string type)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            line = Regex.Replace(line, ",(?=[^()]*\\))", "|");
            string[] newSkills = Regex.Split(line, ", ");
            for (int j = 0; j < newSkills.Length; j++) { newSkills[j] = newSkills[j].Replace('|', ','); }
            foreach (string s in newSkills)
            {
                list.Add(new ModelSkill() {
                    name = s,
                    type = type
                });
            }
            return list;
        }

        /// <summary>
        /// Enters data into the model
        /// </summary>
        /// <param name="line">Array with skills</param>
        /// <param name="type">Type of skills</param>
        /// <returns>List with model</returns>
        private static List<ModelSkill> SplitLineSkills(string[] line, string type)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            foreach (string s in line)
            {
                list.Add(new ModelSkill() {
                    name = s,
                    type = type
                });
            }
            return list;
        }

    }
}
