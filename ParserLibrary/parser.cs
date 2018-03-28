using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary
{
    public static class Parser
    {
        public static void StartParse(string destination)
        {
            try
            {
                //Open doc
                Document doc = new Document();
                doc.LoadFromFile(destination);
                //Find section with table
                Section section = doc.Sections[0];
                Console.WriteLine("Complete read " + destination + " file");
                //Know type of template
                int type = doc.Sections[0].Tables.Count;
                switch (type)
                {
                    case 8:
                        {
                            //ProccessNewTemplate.Responce(section);
                        }
                        break;
                    case 2:
                        {
                            Logic_for_old_template.Logic_for_old_template.Responce(section);
                        }
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Can't load document");
            }
        }
    }
}
