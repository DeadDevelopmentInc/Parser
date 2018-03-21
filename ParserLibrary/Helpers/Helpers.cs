using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary
{
    internal static class Helpers
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
                foreach (TableCell cell in row.Cells)
                {
                    //For each cell read value
                    foreach (Paragraph paragraph in cell.Paragraphs)
                    {
                        //Delete stuff from line
                        string s = paragraph.Text.Trim(':');
                        list.Add(s);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Split string from old templte
        /// </summary>
        /// <param name="allSkills">All skills</param>
        public static List<ModelSkill> ProccSkills(List<string> allSkills)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            for (int i = 1; i < allSkills.Count - 1 & !allSkills[i].Contains("Fore"); i+=2)
            {
                if (allSkills[i].Contains("("))
                    list.AddRange(SplitLineSkills(allSkills[i], allSkills[i - 1]));
                else
                    list.AddRange(SplitLineSkills(Regex.Split(allSkills[i], ", "), allSkills[i - 1]));
            }
            return list;
        }

        /// <summary>
        /// Method for proccessing readed information
        /// Correctly work only with old template
        /// </summary>
        /// <param name="list"></param>
        public static List<string> ProccExp(List<string> list)
        {
            List<string> listExp = new List<string>();
            List<BufferClass> bufferList = new List<BufferClass>();
            Regex regex = new Regex(@"^\w+\s\d{4}\s\W\s\w+");
            for (int i = 5; i < list.Count; i++)
            {
                MatchCollection match = regex.Matches(list[i - 5]);
                if (match.Count > 0 & list[i] == "Environment")
                {
                    var year = list[i - 5];
                    listExp.Add(year);
                    listExp.Add(list[i + 1]);
                    bufferList.AddRange(SplitLineExps(list[i + 1], list[i - 5]));
                }
            }
            CreateLevel(ref bufferList);
            return listExp;
        }

        internal static List<ModelSkill> SplitLineSkills(string line, string type)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            line = Regex.Replace(line, ",(?=[^()]*\\))", "|");
            string[] newSkills = Regex.Split(line, ", ");
            for (int j = 0; j < newSkills.Length; j++) newSkills[j] = newSkills[j].Replace('|', ',');
            foreach (string s in newSkills)
            {
                list.Add(new ModelSkill {
                    name = s,
                    type = type
                });
            }
            return list;
        }

        internal static List<ModelSkill> SplitLineSkills(string[] line, string type)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            foreach (string s in line)
            {
                list.Add(new ModelSkill
                {
                    name = s,
                    type = type
                });
            }
            return list;
        }

        internal static List<BufferClass> SplitLineExps(string line, string date)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            List<BufferClass> bufferList = new List<BufferClass>();
            line = Regex.Replace(line, ",(?=[^()]*\\))", "|");
            string[] newSkills = Regex.Split(line, ", ");
            for (int j = 0; j < newSkills.Length; j++) newSkills[j] = newSkills[j].Replace('|', ',');
            foreach(string s in newSkills)
            {
                bufferList.Add(new BufferClass() {
                    name = s,
                    level = date
                });
            }
            return bufferList;
        }

        private static string GetLevel(int year)
        {
            if (year <= 1)
                return "Working knowledge";
            else if (year <= 2)
                return "Extensive knowledge";
            else if (year <= 4)
                return "Experienced";
            else
                return "Expert";
        }

        internal static void CreateLevel(ref List<BufferClass> buffer)
        {
            for(int i = 0; i < buffer.Count - 1; i++)
            {
                List<BufferClass> temp = new List<BufferClass>();
                List<int> tempPosition = new List<int>();
                temp.Add(buffer[i]);
                tempPosition.Add(i);
                for (int j = i + 1; j < buffer.Count; j++)
                {
                    if(buffer[i].name.Contains(buffer[j].name))
                    {
                        temp.Add(buffer[j]);
                        tempPosition.Add(j);
                    }
                }
                if(temp.Count > 1)
                {

                }
            }
        }

        private static void ProcessDate(List<BufferClass> buffers)
        {
            List<string> list = new List<string>();
            foreach(BufferClass s in buffers)
            {
                list.Add(s.level);
            }
            
        }
    }
}



