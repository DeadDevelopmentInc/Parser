using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace NewTypeParse
{
    public static class DictionarySkills
    {
        public static List<List<string>> similarSkills = new List<List<string>>();

        static DictionarySkills()
        {
            DataContractJsonSerializer jsonFormatterSimilarExp = new DataContractJsonSerializer(typeof(List<string>));

            DirectoryInfo directory = new DirectoryInfo("SimilarSkills");
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                using (FileStream fs = new FileStream(file.Name, FileMode.Open))
                {
                    similarSkills.Add((List<string>)jsonFormatterSimilarExp.ReadObject(fs));
                }
            }
        }
    }
}
