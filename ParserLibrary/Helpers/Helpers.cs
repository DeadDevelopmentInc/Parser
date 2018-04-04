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
    internal static class Helpers
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

        //internal static Dictionary<string, Tuple<bool, string>> GetDictionars()
        //{
        //    Dictionary<string, Tuple<bool, string>> dictionary = new Dictionary<string, Tuple<bool, string>>();
        //    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Dictionary<string, Tuple<bool, string>>));
        //    using (FileStream fs = new FileStream("dictionary.json", FileMode.Open))
        //    {
        //        dictionary = (Dictionary<string, Tuple<bool, string>>)jsonSerializer.ReadObject(fs);
        //    }
        //    return dictionary;
        //}
    }
}



