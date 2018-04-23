using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Logic_for_new_template
{
    internal          class ProcessSkills
    {
        internal static List<BufferClass> ProccSkills(List<string> skillsList)
        {
            List<BufferClass> list = new List<BufferClass>();
            string type = skillsList[0];
            for (int i = 1; i < skillsList.Count - 1; i+=2)
            {
                if (skillsList[i] == "") continue;
                list.Add(new BufferClass
                {
                    name = skillsList[i],
                    level = skillsList[i + 1],
                    type = type
                });
            }
            return list;
        }
    }
}
