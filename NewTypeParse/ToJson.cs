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
        public static void CreateJsonModel(string[] obj, string type)
        {
            //Create list and fill in him
            List<Skill> list = new List<Skill>();
            foreach (string s in obj)
            {
                list.Add(new Skill()
                {
                    _id = Convert.ToString(Guid.NewGuid()),
                    name = s,
                    allNames = new string[] { s },
                    type = type,
                    isSkillNew = true
                });
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<Skill>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skills/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, list);
            }
        }

        /// <summary>
        /// Method for creating json model from new model
        /// </summary>
        /// <param name="source">List with all skills</param>
        public static void CreateJsonModelNew(List<string> source)
        {
            //Save type of skills 
            string type = source[0];
            List<SkillNew> list = new List<SkillNew>();
            //i = 1 because 0 it is place of type of skills
            //i+=2 because i it is skill, i+1 level of knowledge
            for(int i = 1; i < source.Count; i+=2)
            {
                list.Add(new SkillNew()
                {
                    _id = Convert.ToString(Guid.NewGuid()),
                    name = source[i],
                    allNames = new string[] { source[i] },
                    type = type,
                    isSkillNew = true,
                    level = source[i + 1]
                });
            }
            //Create serializer
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(List<SkillNew>));
            //Create and fill in file
            using (FileStream fs = new FileStream("skillsNew/" + type[0].ToString() + type[1].ToString() + ".json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(fs, list);
            }
        }
    }
}
