using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;
using UniversalParserLibrary.Helpers;
using Spire.Doc.Interface;
using UniversalParserLibrary.Models.Algorithms;

namespace UniversalParserLibrary.Training
{
    internal static class LogicForTraining
    {
       internal static List<TrainSkill> TrainList { get; set; } = new List<TrainSkill>();
        private static object locker = new object();

        public static void NewTrain(string destination)
        {
            try
            {
                Document doc = new Document();
                doc.LoadFromFile(destination);
                //Find section with table
                Section section = doc.Sections[0];
                //Get type of template
                int type = doc.Sections[0].Tables.Count;
                new Models.Exceptions_and_Events.Info("Resume Parsing", "INFO", "current user", destination, 1);
                switch (type)
                {
                    case 8: { AddToList(ParseNewDoc(section)); } break;
                    case 2: { AddToList(ParseOldDoc(section)); } break;
                }
            }
            catch(Exception e) { new Models.Exceptions_and_Events.Exception("Resume Parsing", "ERROR", e.Message, destination); }

        }

        /// <summary>
        /// Method for getting all names of technology in old format document
        /// </summary>
        /// <param name="section">Section with tables</param>
        /// <returns></returns>
        private static List<TrainSkill> ParseOldDoc(Section section)
        {
            List<TrainSkill> list = new List<TrainSkill>();
            list.AddRange(Readers.GetNamesOfExpsFromTable(section.Tables[1]));
            list.AddRange(Readers.GetNamesOfSkillsFromOldTable(section.Tables[0]));
            PreproccessTech(ref list);
            //var LevList = LevenshteinAlg.Start(list);
            //GenerateData(list);
            return list;
        }

        internal static void GenerateData(List<TrainSkill> list)
        {
            DahmerauLevenshteinAlg.Start(list);
            foreach (TrainSkill skill in list)
            {
                if (skill.TypeOfSkill == null)
                {
                    foreach(var similarSkill in skill.Skills)
                    {
                        if (similarSkill.TypeOfSkill != null)
                        {
                            skill.TypeOfSkill = similarSkill.TypeOfSkill;
                            break;
                        }
                    }
                }
            }
#if DEBUGX
            foreach (TrainSkill skill in list)
            {
                skill.PostProccessing();
                skill.ToString();
            }
#endif
        }

        /// <summary>
        /// Method for getting all names of technology in old format document
        /// </summary>
        /// <param name="section">Section with tables</param>
        /// <returns></returns>
        private static List<TrainSkill> ParseNewDoc(Section section)
        {
            List<TrainSkill> list = new List<TrainSkill>();
            list.AddRange(Readers.GetNamesOfExpsFromTable(section.Tables[7]));
            for (int i = 0; i < 7; i++) { list.AddRange(Readers.GetNamesOfSkillsFromNewTable(section.Tables[i])); }
            PreproccessTech(ref list);
            return list;
        }

        internal static void PreproccessTech(ref List<TrainSkill> skills)
        {
            for(int i = 0; i < skills.Count; i++)
            {
                skills[i] = Rules.CreateRules(skills[i]);
#if DEBUGX
                HelpMeth.PrintTrainCodeForTest(skills[i].CodeOfSkill, skills[i].NameOfSkill);
#endif
            }
        }

        private static void AddToList(List<TrainSkill> skills)
        {
            lock (locker) { TrainList.AddRange(skills); }
        }

        internal static List<TrainSkill> GenerateTrains(List<TrainSkill> afterTrain)
        {
            var tempList = new List<TrainSkill>();
            var fromDB = PrivateDictionary.GetDataFromDB<Skill>("skills");
            foreach (var s in fromDB)
            {
                var temp = s.ForTrain();
                Rules.CreateRules(temp);
                foreach (string str in temp.SimilarSkills)
                {
                    temp.Skills.Add(new TrainSkill { NameOfSkill = str, CodeOfSkill = str });
                    Rules.CreateRules(temp.Skills[temp.Skills.Count - 1]);
                }
                tempList.Add(temp);
            }
            PreproccessTech(ref tempList);
            tempList.AddRange(afterTrain);
            GenerateData(tempList);
            return tempList;
        }

    }
}
