using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static void ParseDoc(string source, int type)
        {
            switch(type)
            {
                case 1:
                    {
                        ParseNewTemplate(source);
                    }
                    break;
                case 2:
                    {
                        ParseOldTemplate(source);
                    }
                    break;
            }
        }

        /// <summary>
        /// Parse old template
        /// </summary>
        /// <param name="source">file destination</param>
        private static void ParseOldTemplate(string source)
        {
            //Open doc
            Document doc = new Document();
            try
            {
                doc.LoadFromFile(source);
                Console.WriteLine("Read complete");
                //Create list of skills
                List<string> mainList = new List<string>();
                //Find section with table
                Section section = doc.Sections[0];
                //Find table with skills(number 0)
                ITable table = section.Tables[0];
                foreach (TableRow row in table.Rows)
                {
                    foreach (TableCell cell in row.Cells)
                    {
                        //For each cell read value
                        foreach (Paragraph paragraph in cell.Paragraphs)
                        {
                            //Delete stuff from line
                            string s = paragraph.Text.Trim(':');
                            mainList.Add(s);
                        }
                    }
                }
                Console.WriteLine("Parse complete");
                //Split lines with skills 
                Helpers.SplitSkills(mainList);
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
        private static void ParseNewTemplate(string source)
        {
            Document doc = new Document();
            try
            {
                doc.LoadFromFile(source);
                Console.WriteLine("Read complete");

                List<string> mainList = new List<string>();
                StringBuilder sb = new StringBuilder();

                Section section = doc.Sections[0];
                //Now need for all tables with different skills 
                //Read table, handle, and save json model
                //Number of table in section 7
                for(int i = 0; i < 7; i++)
                {
                    mainList.Clear();
                    ITable table = section.Tables[i];
                    foreach (TableRow row in table.Rows)
                    {
                        foreach (TableCell cell in row.Cells)
                        {
                            foreach (Paragraph paragraph in cell.Paragraphs)
                            {
                                if (paragraph.Text != "")
                                {
                                    string s = paragraph.Text.Trim(':');
                                    mainList.Add(s);
                                }
                            }
                        }
                    }
                    Console.WriteLine(@"Parse {0} table complete", i+1);
                    //After reading, create json model
                    ToJson.CreateJsonModelNew(mainList);
                }
                Console.WriteLine("Create json model complete");
            }
            catch
            {
                Console.WriteLine("Invalid document");
            }
        }
    }
}
