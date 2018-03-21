using Spire.Doc;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NewTypeParse.ForNewTemplate
{
    static class ProccessNewTemplate
    {
        public static void Responce(Section section)
        {
            try
            {
                List<string> skillsList = new List<string>();
                List<String> expList = new List<String>();
                //Now need for all tables with different skills
                //Read table, handle, and save json model
                //Number of table in section 7
//                expList = Helpers.GetTextFromTable(section.Tables[7]);
//#if NEW_PARCE_DEBUG
//                foreach (string s in skillsList) { Console.WriteLine(s); }
//                foreach (string s in expList) { Console.WriteLine(s); }
//                Console.ReadKey();
//#endif
//                Helpers.ProccExp(ref expList);
//                for (int i = 0; i < 6; i++)
//                {
//                    skillsList.Clear();
//                    ITable table = section.Tables[i];
//                    skillsList = Helpers.GetTextFromTable(table);
//                    Console.WriteLine(@"Parse {0} table complete", i + 1);
//                    //After reading, create json model
//                    //ToJson.CreateJsonModelNew(skillsList);
//                }

//                Console.WriteLine("Create json model complete");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        /// <summary>
        /// Method for creating json model from new model
        /// </summary>
        /// <param name="source">List with all skills</param>
        private static void CreateJsonModelNew(List<string> source, List<Exp> expList)
        {
            string year = null;
            //Save type of skills 
            string type = source[0];
            List<SkillNew> listSkills = new List<SkillNew>();
            List<AdditionInfo> listAdd = new List<AdditionInfo>();
            //i = 1 because 0 it is place of type of skills
            //i+=2 because i it is skill, i+1 level of knowledge
            for (int i = 1; i < source.Count; i += 2)
            {
                string id = Convert.ToString(Guid.NewGuid());
                listSkills.Add(new SkillNew()
                {
                    _id = id,
                    name = source[i],
                    allNames = new string[] { source[i] },
                    type = type,
                    isSkillNew = true,
                    level = source[i + 1]
                });
                //if (FindExpOfSkills(listSkills[i].name, expList, ref year))
                //{
                //    listAdd.Add(new AdditionInfo()
                //    {
                //        Id = id,
                //        Years = year
                //    });
                //}
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<SkillNew>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skillsNew/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, listSkills);
            }
            //Writee exp
            //CreateJsonModelExp(listAdd, type);
        }
    }
}
