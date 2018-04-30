using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Algorithms
{
    internal class DahmerauLevenshteinAlg
    {
        public static List<TrainSkill> Start(List<TrainSkill> trains)
        {
            for (int i = 0; i < trains.Count - 1; i++)
            {
                for (int j = i + 1; j < trains.Count;)
                {
                    int temp = Compute(trains[i].CodeOfSkill, trains[j].CodeOfSkill);
                    if (temp > 0)
                    {
                        if (temp <= CalcExp(trains[i].CodeOfSkill.Length, trains[j].CodeOfSkill.Length))
                        {
                            trains[i].Skills.Add(trains[j]);
                            trains.RemoveAt(j);
                        }
                        else { j++; }
                        continue;
                    }
                    trains.RemoveAt(j);
                }
            }
            return trains;
        }
        private static int CalcExp(int l1, int l2)
        {
            return (int)(((l1 + l2) / 2) * 0.45);
        }

        internal static int Compute(string first, string second)
        {
            if (first == second)
                return 0;

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
