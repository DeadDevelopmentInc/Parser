using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ParserLibrary.Helpers
{
    internal static class PrivateDictionary
    {
        const string path = @"SimilarSkills\";
        static Dictionary<string, Tuple<string, string, string>> dictionary = new Dictionary<string, Tuple<string, string, string>>();

        static PrivateDictionary()
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(dictionary.GetType());
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate))
                {
                    var temp = (Dictionary<string, Tuple<string, string, string>>)jsonSerializer.ReadObject(fs);
                    AddRange(temp, ref dictionary);
                }

            }
            //using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            //{
            //    dictionary.Add("C#", new Tuple<string, string, string>("C#", "Programming Languages", 1));
            //    jsonSerializer.WriteObject(fs, dictionary);
            //}
        }

        private static void AddRange(Dictionary<string, Tuple<string, string, string>> newData, ref Dictionary<string, Tuple<string, string, string>> oldDictionary)
        {
            foreach(KeyValuePair<string, Tuple<string, string, string>> value in newData)
            {
                oldDictionary.Add(value.Key, value.Value);
            }
        }

        internal static string GetTypeTechByKey(string value)
        {
            if (dictionary.ContainsKey(value.ToUpper())) return dictionary[value.ToUpper()].Item2;
            return null;
        }

        internal static bool CheckTwoValues(string valueA, string valueB)
        {
            valueA = valueA.ToUpperInvariant();
            valueB = valueB.ToUpperInvariant();
            return dictionary.Keys.Contains(valueA) && dictionary.Keys.Contains(valueB) 
                ? (dictionary[valueA].Item1 == dictionary[valueB].Item1 
                    & (dictionary[valueA] != null & dictionary[valueB] != null) 
                ? true : false) : false;
        }

        internal static bool CheckHead(string valueA, string valueB)
        {
            valueA = valueA.ToUpperInvariant();
            valueB = valueB.ToUpperInvariant();
            return dictionary.Keys.Contains(valueA) && dictionary.Keys.Contains(valueB)
                ? (int.Parse(dictionary[valueA].Item3) > int.Parse(dictionary[valueB].Item3) 
                    & (dictionary[valueA] != null & dictionary[valueB] != null)
                ? true : false) : false;
        }
    }
}
