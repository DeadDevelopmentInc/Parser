using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    /// <summary>
    /// Class for training
    /// </summary>
    internal class TrainSkill
    {
        public string _id { get; set; } = Convert.ToString(Guid.NewGuid());
        public string NameOfSkill { get; set; }
        public string CodeOfSkill { get; set; } = null;
        public string TypeOfSkill { get; set; } = null;
        public List<string> SimilarSkills { get; set; } = new List<string>();
        public List<TrainSkill> Skills { get; set; } = new List<TrainSkill>();
        public bool IsSkillNew { get; set; } = true;

        /// <summary>
        /// After all operations, before writing in database, need find similar skill (using Names such a key)
        /// </summary>
        public void PostProccessing()
        {
            SimilarSkills = new List<string>();
            //Find all similar skills
            for (int i = 0; i < Skills.Count - 1; i++)
            {
                if(Skills[i].NameOfSkill == NameOfSkill) { Skills.RemoveAt(i); }
                for (int j = i + 1; j < Skills.Count; j++)
                {
                    if(Skills[j].NameOfSkill == Skills[i].NameOfSkill || Skills[j].NameOfSkill == NameOfSkill)
                    {
                        Skills.RemoveAt(j); j--;
                    }
                }
            }
            foreach(TrainSkill skill in Skills) { SimilarSkills.Add(skill.NameOfSkill); }
        }

        /// <summary>
        /// Return ready-to-write object (Skill) 
        /// </summary>
        /// <returns>Skill with this params</returns>
        public Skill ForWrite()
        {
            PostProccessing();
            return new Skill { name = NameOfSkill, _id = _id, allNames = SimilarSkills, type = TypeOfSkill, isSkillNew = IsSkillNew };
        }

        public new void ToString()
        {
            Console.WriteLine(NameOfSkill + " " + CodeOfSkill);
            foreach(TrainSkill skill in Skills)
            {
                Console.WriteLine("\t" + skill.NameOfSkill + " ___ " + skill.CodeOfSkill);
            }
        }
    }
}
