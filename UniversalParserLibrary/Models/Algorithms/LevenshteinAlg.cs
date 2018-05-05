using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Algorithms
{
    internal static class LevenshteinAlg
    {
        public static List<TrainSkill> Start(List<TrainSkill> trains)
        {
            for(int i = 0; i < trains.Count - 1; i++)
            {
                for(int j = i + 1; j < trains.Count;)
                {
                    int temp = Compute(trains[i].CodeOfSkill, trains[j].CodeOfSkill);
                    if (temp > 0)
                    {
                        if(temp <= CalcExp(trains[i].CodeOfSkill.Length, trains[j].CodeOfSkill.Length))
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
            return (int)(((l1 + l2) / 2) * 0.36);
        }

        public static int Compute(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; d[i, 0] = i++) { }

            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
