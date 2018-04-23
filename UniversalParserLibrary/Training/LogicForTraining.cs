﻿using Spire.Doc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalParserLibrary.Models;
using UniversalParserLibrary.Helpers;
using Spire.Doc.Interface;

namespace UniversalParserLibrary.Training
{
    internal static class LogicForTraining
    {
        private static List<TrainSkill> TrainList { get; set; } = new List<TrainSkill>();
        private static object locker = new object();

        public static void NewTrain(string destination)
        {
            try
            {
                Document doc = new Document();
                doc.LoadFromFile(destination);
                //Find section with table
                Section section = doc.Sections[0];
                Console.WriteLine("Complete read " + destination + " file");
                //Get type of template
                int type = doc.Sections[0].Tables.Count;
                switch (type)
                {
                    case 8: { AddToList(ParseNewDoc(section)); } break;
                    case 2: { AddToList(ParseOldDoc(section)); } break;
                }
            }
            catch { }

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
            return list;
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
            return list;
        }

        public static void FindSimpleSkills()
        {

        }

        private static void AddToList(List<TrainSkill> skills)
        {
            lock (locker) { TrainList.AddRange(skills); }
        }


    }
}