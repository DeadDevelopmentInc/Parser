using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UniversalParserLibrary.Models;
using UniversalParserLibrary.Models.Algorithms;
using UniversalParserLibrary.Training;

namespace TestUniversalLibrary
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            TrainSkill MySQL1 = new TrainSkill { CodeOfSkill = "mysql", NameOfSkill = "MySQL"};
            TrainSkill MySQL2 = new TrainSkill { CodeOfSkill = "mysql", NameOfSkill = "MySQL" };
            TrainSkill MSSQL = new TrainSkill { CodeOfSkill = "mssql", NameOfSkill = "MS SQL"};
            TrainSkill SQL = new TrainSkill { CodeOfSkill = "sql", NameOfSkill = "SQL"};
            SQL.Skills.Add(MySQL2);
            List<TrainSkill> skills = new List<TrainSkill>();
            skills.AddRange(new List<TrainSkill> {SQL, MSSQL });
            DahmerauLevenshteinAlg.Start(skills);

        }
    }
}
