using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Algorithms
{
    internal class DahmerauLevenshteinAlg
    {
        /// <summary>
        /// Start training
        /// </summary>
        /// <param name="trains"></param>
        /// <returns></returns>
        public static List<TrainSkill> Start(List<TrainSkill> trains)
        {
            bool? t = null;
            for (int i = 0; i < trains.Count - 1; i++)
            {
                for (int j = i + 1; j < trains.Count;)
                {
                    t = CheckAllNames(trains[i], trains[j]);
                    if (t == true) { trains[i].Skills.Add(trains[j]); trains.RemoveAt(j); }
                    else if(t == null) { trains.RemoveAt(j); }
                    else { j++; }
                    t = null;
                }
            }
            return trains;
        }

        /// <summary>
        /// Check all names of main and second skill
        /// </summary>
        /// <param name="mainTrainSkill"></param>
        /// <param name="secondTrainSkill"></param>
        /// <returns>Desigion</returns>
        private static bool? CheckAllNames(TrainSkill mainTrainSkill, TrainSkill secondTrainSkill)
        {
            bool? fl = null;
            if (mainTrainSkill.Skills.Count == 0 && secondTrainSkill.Skills.Count == 0) { return CheckTwoValue(mainTrainSkill.CodeOfSkill, secondTrainSkill.CodeOfSkill); }
            else
            {
                List<string> mainCodes = new List<string>();
                List<string> secondCodes = new List<string>();

                mainCodes.Add(mainTrainSkill.CodeOfSkill);
                secondCodes.Add(secondTrainSkill.CodeOfSkill);

                foreach (TrainSkill skill in mainTrainSkill.Skills) { mainCodes.Add(skill.CodeOfSkill); }
                foreach (TrainSkill skill in secondTrainSkill.Skills) { secondCodes.Add(skill.CodeOfSkill); }

                foreach(string str1 in mainCodes)
                {
                    foreach(string str2 in secondCodes)
                    {
                        var temp = CheckTwoValue(str1, str2);
                        if (temp == true) { mainTrainSkill.Skills.AddRange(secondTrainSkill.Skills); return true; }
                        if (temp == false) { fl = false; }
                    }
                }
            }
            return fl;
        }

        /// <summary>
        /// Methods for check distance between two words
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private static bool? CheckTwoValue(string first, string second)
        {
            int temp = Compute(first, second);
            if (temp > 0)
            {
                if (temp <= CalcExp(first.Length, second.Length)) { return true; }
                return false;
            }
            else { return null; }
        }

        /// <summary>
        /// Method of calculating the quantity of mistakes between two words
        /// </summary>
        /// <param name="l1">Length of first string</param>
        /// <param name="l2">Length of second string</param>
        /// <returns>Quantity of mistakes</returns>
        private static int CalcExp(int l1, int l2)
        {
            return (int)(((l1 + l2) / 2) * 0.45);
        }
        
        /// <summary>
        /// Main logic for dahmerau-levenshtein
        /// </summary>
        /// <param name="first">First string</param>
        /// <param name="second">Second string</param>
        /// <returns>Distance between two strings</returns>
        internal static int Compute(string first, string second)
        {
            if (first == second)
                return 0;
            if (first == null)
                return second.Length;
            if (second == null)
                return first.Length;
            int len_orig = first.Length;
            int len_diff = second.Length;         

            var matrix = new int[len_orig + 1, len_diff + 1];

            for (int i = 1; i <= len_orig; i++)
            {
                matrix[i, 0] = i;
                for (int j = 1; j <= len_diff; j++)
                {
                    int cost = second[j - 1] == first[i - 1] ? 0 : 1;
                    if (i == 1)
                        matrix[0, j] = j;

                    var vals = new int[] {
                    matrix[i - 1, j] + 1,
                    matrix[i, j - 1] + 1,
                    matrix[i - 1, j - 1] + cost
                };
                    matrix[i, j] = vals.Min();
                    if (i > 1 && j > 1 && first[i - 1] == second[j - 2] && first[i - 2] == second[j - 1])
                        matrix[i, j] = Math.Min(matrix[i, j], matrix[i - 2, j - 2] + cost);
                }
            }
            return matrix[len_orig, len_diff];
        }

    }
}
