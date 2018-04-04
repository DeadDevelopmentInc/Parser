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
        const string path = @"SimilarSkills\similarSkills.json";
        static Dictionary<string, Tuple<string, string>> dictionary = new Dictionary<string, Tuple<string, string>>();

        static PrivateDictionary()
        {
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(dictionary.GetType());
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                dictionary = (Dictionary<string, Tuple<string, string>>)jsonSerializer.ReadObject(fs);
            }
            //using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            //{
            //    dictionary.Add("C#", new Tuple<string, string>("C#", "Programming Languages"));
            //    jsonSerializer.WriteObject(fs, dictionary);
            //}
        }

        internal static string GetTypeTechByKey(string value)
        {
            if (dictionary.ContainsKey(value)) return dictionary[value].Item2;
            return null;
        }

        internal static bool CheckTwoValues(string valueA, string valueB)
        {
            bool fl = dictionary.Keys.Contains(valueA);
            bool newFl = dictionary.Keys.Contains(valueB);
            if (dictionary.Keys.Contains(valueA) && dictionary.Keys.Contains(valueB))
            {
                if (dictionary[valueA].Item1 == dictionary[valueB].Item1 & (dictionary[valueA] != null & dictionary[valueB] != null)) return true;
            }
            return false;
        }
    }
}
