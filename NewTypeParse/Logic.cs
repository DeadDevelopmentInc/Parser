#define OLD_pARCE_DEBUG
#define NEW_PARCE_DEBUG
using NewTypeParse.ForNewTemplate;
using NewTypeParse.ForOldTemplate;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewTypeParse
{
    static class Logic
    {
        /// <summary>
        /// Method for chouse type of parsing
        /// </summary>
        /// <param name="source">file destination</param>
        /// <param name="type">template type</param>
        public static void ParseDoc(string source)
        { 
            try
            {
                //Open doc
                Document doc = new Document();
                doc.LoadFromFile(source);
                //Find section with table
                Section section = doc.Sections[0];
                Console.WriteLine("Read complete");
                //Know type of template
                int type = doc.Sections[0].Tables.Count;
                switch (type)
                {
                    case 8:
                        {
                            ProccessNewTemplate.Responce(section);
                        }
                        break;
                    case 2:
                        {
                            PrccssOldTmplt.Responce(section);
                        }
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Can't load document");
            }            
        }

        /// <summary>
        /// Parse old template
        /// </summary>
        /// <param name="source">file destination</param>
        private static void ParseOldTemplate(Section section)
        {
            try
            {
                //Create list of skills
                List<string> skillsList = new List<string>();
                List<string> expList = new List<string>();

                List<Exp> sortExpSkills = new List<Exp>();
                //Get text from tables
                skillsList = GetTextFromTable(section.Tables[0]); //table with skills
                expList = GetTextFromTable(section.Tables[1]); // table with exp 
#if OLD_PARCE_DEBUG
                foreach(string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
                Console.WriteLine("Parse complete");
                //Split and save skills
                Helpers.ProccExp(ref expList);
                Helpers.SaveSkills(skillsList, sortExpSkills);
                //Processing and save expearence
                Console.WriteLine("Create json model complete");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        /// <summary>
        /// Parce new type of template
        /// </summary>
        /// <param name="source">file destination</param>
        private static void ParseNewTemplate(Section section)
        {
            try
            {
                List<string> skillsList = new List<string>();
                List<String> expList = new List<String>();
                //Now need for all tables with different skills 
                //Read table, handle, and save json model
                //Number of table in section 7
                expList = GetTextFromTable(section.Tables[7]);
#if NEW_PARCE_DEBUG
                foreach (string s in skillsList) { Console.WriteLine(s); }
                foreach (string s in expList) { Console.WriteLine(s); }
                Console.ReadKey();
#endif
                Helpers.ProccExp(ref expList);
                for (int i = 0; i < 6; i++)
                {
                    skillsList.Clear();
                    ITable table = section.Tables[i];
                    skillsList = GetTextFromTable(table);
                    Console.WriteLine(@"Parse {0} table complete", i + 1);
                    //After reading, create json model
                    //ToJson.CreateJsonModelNew(skillsList);
                }

                Console.WriteLine("Create json model complete");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }

        private static List<string> GetTextFromTable(ITable table)
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
    }
}
