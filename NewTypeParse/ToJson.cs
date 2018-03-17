using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NewTypeParse
{
    class ToJson
    {
        /// <summary>
        /// Method for creating json model from old model
        /// </summary>
        /// <param name="obj"> Array with splited skills</param>
        /// <param name="type">type of skills</param>
        public static void CreateJsonModel(string[] obj, string type, List<Exp> expList)
        {
            string year = null;
            //Create list and fill in him
            List<Skill> listSkills = new List<Skill>();
            List<AdditionInfo> listAdd = new List<AdditionInfo>();
            for(int i = 0; i < obj.Length; i++)
            {
                var id = Convert.ToString(Guid.NewGuid());
                listSkills.Add(new Skill()
                {
                    _id = id,
                    name = obj[i],
                    allNames = new string[] { obj[i] },
                    type = type,
                    isSkillNew = true
                });
                if(FindExpOfSkills(listSkills[i].name, expList, ref year))
                {
                    listAdd.Add(new AdditionInfo()
                    {
                        Id = id,
                        Years = year
                    });
                }
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatterSkills = new DataContractJsonSerializer(typeof(List<Skill>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skills/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatterSkills.WriteObject(fs, listSkills);
            }
            //Write exp model
            CreateJsonModelExp(listAdd, type);
        }

        /// <summary>
        /// Method for creating json model from new model
        /// </summary>
        /// <param name="source">List with all skills</param>
        public static void CreateJsonModelNew(List<string> source, List<Exp> expList)
        {
            string year = null;
            //Save type of skills 
            string type = source[0];
            List<SkillNew> listSkills = new List<SkillNew>();
            List<AdditionInfo> listAdd = new List<AdditionInfo>();
            //i = 1 because 0 it is place of type of skills
            //i+=2 because i it is skill, i+1 level of knowledge
            for (int i = 1; i < source.Count; i+=2)
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
                if (FindExpOfSkills(listSkills[i].name, expList, ref year))
                {
                    listAdd.Add(new AdditionInfo()
                    {
                        Id = id,
                        Years = year
                    });
                }
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<SkillNew>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skillsNew/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, listSkills);
            }
            //Writee exp
            CreateJsonModelExp(listAdd, type);
        }
        

        public static void CreateJsonModelExp(List<AdditionInfo> list, string type)
        {
            DataContractJsonSerializer jsonFormatterExp = new DataContractJsonSerializer(typeof(List<AdditionInfo>));
            
            using (FileStream fs = new FileStream("exp/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatterExp.WriteObject(fs, list);
            }
        }


        public static bool FindExpOfSkills(string nameSkills, List<Exp> list, ref string year)
        {
            foreach(Exp ex in list)
            {
                if (nameSkills.Contains(ex.Name))
                {
                    year = ex.Year;
                    list.Remove(ex);
                    return true;
                } 
            }
            return false;
        }
    }
}
