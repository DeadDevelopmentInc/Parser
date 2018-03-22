using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_old_template
{
    internal static class ProcessExps
    {
        /// <summary>
        /// Method for proccessing readed information
        /// Correctly work only with old template
        /// </summary>
        /// <param name="list">List with skills</param>
        internal static List<ModelSkill> ProccExp(List<string> list)
        {
            List<ModelSkill> listExp = new List<ModelSkill>();
            List<BufferClass> bufferList = new List<BufferClass>();
            //Regex for date
            Regex regex = new Regex(@"^\w+\s\d{4}\s\W\s\w+");
            for (int i = 5; i < list.Count; i++)
            {
                MatchCollection match = regex.Matches(list[i - 5]);
                if (match.Count > 0 & list[i] == "Environment") { bufferList.AddRange(SplitLineExps(list[i + 1], list[i - 5])); }
            }
            CreateLevel(ref bufferList);
            return listExp;
        }

        /// <summary>
        /// Spliting exp line with date
        /// </summary>
        /// <param name="line">Line with skills</param>
        /// <param name="date">Line with date</param>
        /// <returns>Return list with buffer models</returns>
        private static List<BufferClass> SplitLineExps(string line, string date)
        {
            List<ModelSkill> list = new List<ModelSkill>();
            List<BufferClass> bufferList = new List<BufferClass>();
            line = Regex.Replace(line, ",(?=[^()]*\\))", "|");
            string[] newSkills = Regex.Split(line, ", ");
            for (int j = 0; j < newSkills.Length; j++) { newSkills[j] = newSkills[j].Replace('|', ','); }
            foreach (string s in newSkills)
            {
                bufferList.Add(new BufferClass() { 
                    name = s,
                    level = date
                });
            }
            return bufferList;
        }

        /// <summary>
        /// Delete similar skill and calculate exp
        /// </summary>
        /// <param name="buffer">ref buffer with models</param>
        private static void CreateLevel(ref List<BufferClass> buffer)
        {
            for (int i = 0; i < buffer.Count - 1; i++)
            {
                List<BufferClass> temp = new List<BufferClass>();
                List<int> tempPosition = new List<int>();
                temp.Add(buffer[i]);
                tempPosition.Add(i);
                for (int j = i + 1; j < buffer.Count; j++)
                {
                    if (buffer[i].name.Contains(buffer[j].name))
                    {
                        if (!buffer[i].name.Equals(buffer[j].name)) { buffer[i].SimilarSkills.Add(buffer[j].name); }
                        temp.Add(buffer[j]); tempPosition.Add(j);
                    }                    
                }
                if (temp.Count > 1)
                {
                    buffer[i].level = ProcessDate(temp);
                    int j = 0;
                    foreach (int s in tempPosition) { buffer.RemoveAt(s - j); j++; }
                }
            }
        }

        /// <summary>
        /// Proccess exp
        /// </summary>
        /// <param name="buffers">List with similar skills</param>
        /// <returns></returns>
        private static string ProcessDate(List<BufferClass> buffers)
        {
            List<Tuple<SkillDate, SkillDate>> list = new List<Tuple<SkillDate, SkillDate>>();
            Regex date = new Regex(@"^\w+\s\d{4}\s\S\s\w+\s\d{4}$");
            foreach (BufferClass s in buffers)
            {
                MatchCollection match = date.Matches(s.level);
                if (match.Count > 0)
                {
                    string[] dates = s.level.Split(' ');
                    list.Add(new Tuple<SkillDate, SkillDate>(
                        new SkillDate() { Month = dates[0], Year = dates[1] },
                        new SkillDate() { Month = dates[3], Year = dates[4] }));
                }
                else
                {
                    string[] dates = s.level.Split(' ');
                    list.Add(new Tuple<SkillDate, SkillDate>(
                        new SkillDate() { Month = dates[0], Year = dates[1] },
                        new SkillDate() { MonthInt = DateTime.Now.Month, YearInt = DateTime.Now.Year }));
                }
            }
            return CalculateDate(list);
        }

        private static string CalculateDate(List<Tuple<SkillDate, SkillDate>> list)
        {
            double date = 0;
            for(int i = 0; i < list.Count - 1; i++)
            {
                for(int j = i + 1; j < list.Count; j++)
                {
                    if (list[i].Item1.YearInt >= list[j].Item1.YearInt &
                    list[i].Item1.YearInt < list[j].Item2.YearInt)
                    {
                        if(list[i].Item1.MonthInt > list[j].Item1.MonthInt)
                        {
                            list[i] = list[j];
                        }
                        else
                        {
                            list[i].Item1.MonthInt = list[j].Item1.MonthInt;
                        }
                        list.RemoveAt(j);
                        j--;
                    }
                    if (list[i].Item2.YearInt >= list[j].Item1.YearInt &
                        list[i].Item2.YearInt <= list[j].Item2.YearInt)
                    {
                        if(list[i].Item2.YearInt == list[j].Item2.YearInt)
                        {
                            if(list[i].Item2.MonthInt < list[j].Item2.MonthInt) list[i].Item2.MonthInt = list[j].Item2.MonthInt;
                        }
                        else
                        {
                            list[i] = list[j];
                        }
                        list.RemoveAt(j);
                        j--;
                    }
                    if(list[i].Item1.YearInt > list[j].Item1.YearInt &
                        list[i].Item2.YearInt < list[j].Item2.YearInt)
                    {
                        list[i] = list[j];
                        list.RemoveAt(j);
                        j--;
                    }
                    if(list[i].Item1.YearInt < list[j].Item1.YearInt &
                        list[i].Item2.YearInt > list[j].Item2.YearInt)
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            foreach(var n in list)
            {
                date += n.Item1.GetLenght(n.Item2);
            }
            return GetLevel(Convert.ToInt32(date));
        }

        /// <summary>
        /// Get level from number of year
        /// </summary>
        /// <param name="year">Summary years</param>
        /// <returns>Level of years</returns>
        private static string GetLevel(int year)
        {
            if (year <= 1)      return "Working knowledge";
            else if (year <= 2) return "Extensive knowledge";
            else if (year <= 4) return "Experienced";
            else                return "Expert";
        }
    }
}
