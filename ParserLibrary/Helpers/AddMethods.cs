using MongoDB.Bson;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParserLibrary.Helpers
{
    internal static class AddMethods
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

        internal static void DeleteSimilarSkills(ref List<BufferClass> expModelList)
        {
            for (int i = 0; i < expModelList.Count - 1; i++)
            {
                for (int j = i + 1; j < expModelList.Count; j++)
                {
                    if (PrivateDictionary.CheckTwoValues(expModelList[i].name, expModelList[j].name))
                    {
                        if (expModelList[i].name == expModelList[j].name)
                        {
                            expModelList.RemoveAt(j); j--;
                            expModelList[i].AddLevel(expModelList[j].level);
                        }
                        else if (expModelList[i].allNames.Contains(expModelList[j].name)) { expModelList.RemoveAt(j); j--; continue; }
                        else
                        {
                            expModelList[i].allNames.Add(expModelList[j].name);
                            expModelList[i].allNames.AddRange(expModelList[j].allNames);
                            expModelList[i].AddLevel(expModelList[j].level);
                            expModelList.RemoveAt(j);
                            j--;
                        }
                    }
                }
                var temp = PrivateDictionary.GetTypeTechByKey(expModelList[i].name);
                if (temp != null) { expModelList[i].type = temp; }
                
            }
        }

        internal static void CheckLeadSkill(ref List<BufferClass> expModelList)
        {
            foreach (BufferClass bs in expModelList)
            {
                bs.HeadState();
            }
        }

        internal static bool ExName(List<string> array, string name)
        {
            foreach (string s in array)
            {
                if (s.Equals(name)) return true;
            }
            return false;
        }

        internal static bool ExCollection(List<BsonDocument> bsons, string name)
        {
            foreach (BsonDocument doc in bsons)
            {
                if (doc["name"] == name) { return true; }
            }
            return false;
        }

        internal static Tuple<List<ModelSkill>, List<SkillLevel>> ToModelSkills(List<BufferClass> buffer)
        {
            List<ModelSkill> skills = new List<ModelSkill>();
            List<SkillLevel> levels = new List<SkillLevel>();
            foreach (BufferClass bc in buffer)
            {
                string id = Convert.ToString(Guid.NewGuid());
                skills.Add(new ModelSkill
                {
                    _id = id,
                    name = bc.name,
                    type = bc.type,
                    allNames = bc.allNames
                });
                levels.Add(new SkillLevel
                {
                    _id = id,
                    level = bc.level
                });
            }
            return new Tuple<List<ModelSkill>, List<SkillLevel>>(skills, levels);
        }
    }
}



