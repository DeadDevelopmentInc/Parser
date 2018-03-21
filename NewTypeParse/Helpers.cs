using Spire.Doc;
using Spire.Doc.Documents;
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
        public static void SaveSkills(List<string> allSkills, List<Exp> expList)
        {
            for(int i = 0; i < allSkills.Count & !allSkills[i].Contains("Fore"); i+=2)
            {
                string[] skills = Regex.Split(allSkills[i+1], ", ");
                ToJson.CreateJsonModel(skills, allSkills[i], expList);
            }
        }

        /// <summary>
        /// Method for proccessing readed information
        /// Correctly work only with old template
        /// </summary>
        /// <param name="list"></param>
        public static void ProccExp(ref List<string> list)
        {
            List<string> listExp = new List<string>();
            Regex regex = new Regex(@"^\w+\s\d{4}\s\W\s\w");            
            for(int i = 0; i < list.Count; i++)
            {
                MatchCollection match = regex.Matches(list[i]);
                if(match.Count > 0 & list[i + 5] == "Environment")
                {
                    var year = list[i].Split(' ');
                    listExp.Add(year[1]);
                    listExp.Add(list[i+6]);
                }
            }
        }


        //Now don't need
        //But in the feature need to create exp model, before creatin json model
        //public static List<Exp> CreateExpModel(string year, string skills)
        //{
        //    List<Exp> exp = new List<Exp>(); 
        //    string[] allSkills = Regex.Split(skills, ", ");
        //    foreach(string s in allSkills)
        //    {
        //        exp.Add(new Exp()
        //        {
        //            Name = s,
        //            Year = (DateTime.Now.Year - Convert.ToInt32(year)).ToString()
        //        });
        //    }
        //    return exp;
        //}

        //public static void DeleteEqExp(ref List<Exp> list)
        //{
        //    for(int i = 0; i < list.Count - 1; i++)
        //    {
        //        for(int j = i + 1; j < list.Count; j++)
        //        {
        //            if(list[i].Name == list[j].Name)
        //            {
        //                if(Convert.ToInt16(list[i].Year) > Convert.ToInt16(list[j].Year))
        //                {
        //                    list.Remove(list[j]);
        //                }
        //                else
        //                {
        //                    list.Remove(list[i]);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
