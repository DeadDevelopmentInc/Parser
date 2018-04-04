using MongoDB.Driver;
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
        public static void StartParse(string destination, string name)
        {
            try
            {
                //Open doc
                Document doc = new Document();
                doc.LoadFromFile(destination);
                //Find section with table
                Section section = doc.Sections[0];
                Console.WriteLine("Complete read " + destination + " file");
                //Get type of template
                int type = doc.Sections[0].Tables.Count;
                switch (type)
                {
                    case 8: { Logic_for_new_template.Logic_for_new_template.Response(section, name); } break;
                    case 2: { Logic_for_old_template.Logic_for_old_template.Response(section, name); } break;
                }
            }
            catch
            {
                Console.WriteLine("Can't load document " + name);
            }
        }

        
    }
}
