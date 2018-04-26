using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models
{
    internal class TrainSkill
    {
        public string NameOfSkill { get; set; }
        public string CodeOfSkill { get; set; } = null;
        public string TypeOfSkill { get; set; } = null;
        public List<string> SimilarSkills { get; set; } = new List<string>();
        public List<TrainSkill> Skills { get; set; } = new List<TrainSkill>();

        public void PostProccessing()
        {
            for(int i = 0; i < Skills.Count - 1; i++)
            {
                if(Skills[i].NameOfSkill == NameOfSkill) { Skills.RemoveAt(i); }
                for(int j = i + 1; j < Skills.Count; j++)
                {
                    if(Skills[j].NameOfSkill == Skills[i].NameOfSkill || Skills[j].NameOfSkill == NameOfSkill)
                    {
                        Skills.RemoveAt(j); j--;
                    }
                }
            }
        }

        public new void ToString()
        {
            Console.WriteLine(NameOfSkill + " " + CodeOfSkill);
            foreach(TrainSkill skill in Skills)
            {
                Console.WriteLine("\t" + skill.NameOfSkill);
            }
        }
    }
}
