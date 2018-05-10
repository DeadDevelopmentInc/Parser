using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;

namespace UniversalParserLibrary.Helpers
{
    internal static class Readers
    {
        /// <summary>
        /// Read information from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static List<string> GetTextFromTable(ITable table)
        {
            List<string> list = new List<string>();
            foreach (TableRow row in table.Rows)
            {
                foreach (TableCell cell in row.Cells)                                                            //For each cell read value
                {
                    foreach (Paragraph paragraph in cell.Paragraphs) { list.Add(paragraph.Text.Trim(':')); }     //Delete stuff from line
                }
            }
            return list;
        }

        /// <summary>
        /// Method for getting skills from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static List<BufferSkill> GetSkillsFromOldTable(ITable table)
        {
            List<string> arraySkills = GetTextFromTable(table);
            List<BufferSkill> list = new List<BufferSkill>();
            for (int i = 1; i < arraySkills.Count && !arraySkills[i - 1].Contains("Fore"); i += 2)
            {
                arraySkills[i] = Regex.Replace(arraySkills[i], ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(arraySkills[i], ", ");
                foreach (string m in buff)
                {
                    string buffer = m.Replace("|", ",");
                    list.Add(new BufferSkill { name = buffer });
                }
            }
            return list;
        }

        internal static List<BufferSkill> GetExpsFromOldTable(ITable table, List<BufferProject> bufferProjects)
        {
            List<BufferSkill> list = new List<BufferSkill>();
            var exps = GetExpsFromTable(table, ref bufferProjects, true);
            foreach (var exp in exps)
            {
                string temp = exp.Item1;
                temp = Regex.Replace(temp, ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(temp, ", ");
                foreach (string s in buff)
                {
                    list.Add(new BufferSkill
                    {
                        name = s,
                        Date = exp.Item2
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Method for getting skills from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static List<BufferSkill> GetSkillsFromNewTable(ITable table)
        {
            List<string> arraySkills = GetTextFromTable(table);
            List<BufferSkill> list = new List<BufferSkill>();
            for (int i = 1; i < arraySkills.Count; i += 2)
            {
                arraySkills[i] = Regex.Replace(arraySkills[i], ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(arraySkills[i], ", ");
                foreach (string s in buff)
                {
                    list.Add(new BufferSkill()
                    {
                        name = s,
                        level = arraySkills[i + 1]
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Method for getting exps from table
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static List<Tuple<string, string>> GetExpsFromTable(ITable table, ref List<BufferProject> bufferProjects, bool parse)
        {
            string date = null;
            List<Tuple<string, string>> list = new List<Tuple<string, string>>();
            List<string> temp = new List<string>();
            temp = GetTextFromTable(table);
            if(parse)
                CreateBufferProjects(bufferProjects, temp);
            Regex regex = new Regex(@"^\w*\s\d{4}\s\W\s\w*");
            for (int i = 5; i < temp.Count; i++)
            {
                MatchCollection match = regex.Matches(temp[i]);
                if(match.Count > 0) { date = temp[i]; } 
                if (temp[i] == "Environment") { list.Add(new Tuple<string, string>(temp[i + 1], date)); date = null; }
            }
            return list;
        }

        internal static void CreateBufferProjects(List<BufferProject> projects, List<string> texts)
        {
            Regex regex = new Regex(@"^\w*\s\d{4}\s\W\s\w*");
            List<string> temp = new List<string>();
            for(int  i = 0; i < texts.Count; i++)
            {
                MatchCollection match = regex.Matches(texts[i]);
                if (match.Count > 0) { temp.Clear();  temp.Add(texts[i-1] != "" ? texts[i - 1] : texts[i - 2]); temp.Add(texts[i]); }
                else
                {
                    if (texts[i] != "") { temp.Add(texts[i]); }
                    if (temp.Count == 10)
                    {
                        string[] date = temp[1].Contains("-") ? Regex.Split(temp[1], " - ") : Regex.Split(temp[1], " – ");
                        projects.Add(new BufferProject(temp[2], temp[2], temp[3], temp[9], temp[0], temp[5], date[0], date[1]));
                        temp.Clear();
                    }
                } 
            }
            return;
        }

        /// <summary>
        /// Method for finding only exps names in table
        /// </summary>
        /// <param name="table">Table for search</param>
        /// <returns>List with exps names</returns>
        internal static List<TrainSkill> GetNamesOfExpsFromTable(ITable table)
        {
            List<TrainSkill> skills = new List<TrainSkill>();
            List<string> list = new List<string>();
            List<string> temp = new List<string>();
            temp = GetTextFromTable(table);
            for (int i = 1; i < temp.Count; i++) { if (temp[i - 1].Contains("Environment")) { list.Add(temp[i]); } }

            for (int i = 0; i < list.Count; i++)
            {
                list[i] = Regex.Replace(list[i], ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(list[i], ", ");
                foreach (string m in buff)
                {
                    string buffer = m.Replace("|", ",");
                    skills.Add(new TrainSkill { NameOfSkill = buffer });
                }
            }
            return skills;
        }

        /// <summary>
        /// Method for finding only skills names in table
        /// </summary>
        /// <param name="table">able for search</param>
        /// <returns>List with skills names</returns>
        internal static List<TrainSkill> GetNamesOfSkillsFromOldTable(ITable table)
        {
            List<TrainSkill> skills = new List<TrainSkill>();
            List<string> temp = new List<string>();
            temp = GetTextFromTable(table);
            for (int i = 1; i < temp.Count - 2; i += 2)
            {
                //Replace ( , , ) => ( | | )
                temp[i] = Regex.Replace(temp[i], ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(temp[i], ", ");
                foreach (string m in buff)
                {
                    string buffer = m.Replace("|", ",");
                    skills.Add(new TrainSkill { NameOfSkill = buffer, TypeOfSkill = temp[i - 1] });
                }
            }
            return skills;
        }

        /// <summary>
        /// Method for parse skill's table from new template 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        internal static List<TrainSkill> GetNamesOfSkillsFromNewTable(ITable table)
        {
            List<TrainSkill> skills = new List<TrainSkill>();
            List<string> temp = new List<string>();
            temp = GetTextFromTable(table);
            for (int i = 1; i < temp.Count; i += 2)
            {
                //Replace ( , , ) => ( | | )
                temp[i] = Regex.Replace(temp[i], ",(?=[^()]*\\))", "|");
                string[] buff = Regex.Split(temp[i], ", ");
                foreach (string m in buff)
                {
                    string buffer = m.Replace("|", ",");
                    skills.Add(new TrainSkill { NameOfSkill = buffer, TypeOfSkill = temp[0] });
                }
            }
            return skills;
        }        
    }
}
