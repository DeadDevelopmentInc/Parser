using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalParserLibrary.Models.Algorithms
{
    [Obsolete("The method is inoperative", true)]
    internal static class Bitap
    {
        
        public static List<TrainSkill> Start(List<TrainSkill> trains)
        {
            for (int i = 0; i < trains.Count - 1; i++)
            {
                for (int j = i + 1; j < trains.Count;)
                {
                    int temp = Compute(trains[i].CodeOfSkill, trains[j].CodeOfSkill, 3);
                    if (temp > 0)
                    {
                        trains[i].Skills.Add(trains[j]);
                        trains.RemoveAt(j);
                    }
                    else { j++; }
                }
            }
            return trains;
        }

        private static int CalcExp(int l1, int l2)
        {
            return (int)(((l1 + l2) / 2) * 0.45);
        }

        public static int Compute(string text, string pattern, int k)
        {
            int result = -1;
            int m = pattern.Length;
            int[] R;
            int[] patternMask = new int[128];
            int i, d;

            if (string.IsNullOrEmpty(pattern)) return 0;
            if (m > 31) return -1; //Error: The pattern is too long!

            R = new int[(k + 1) * sizeof(int)];
            for (i = 0; i <= k; ++i)
                R[i] = ~1;

            for (i = 0; i <= 127; ++i)
                patternMask[i] = ~0;

            for (i = 0; i < m; ++i)
                patternMask[pattern[i]] &= ~(1 << i);

            for (i = 0; i < text.Length; ++i)
            {
                int oldRd1 = R[0];

                R[0] |= patternMask[text[i]];
                R[0] <<= 1;

                for (d = 1; d <= k; ++d)
                {
                    int tmp = R[d];

                    R[d] = (oldRd1 & (R[d] | patternMask[text[i]])) << 1;
                    oldRd1 = tmp;
                }

                if (0 == (R[k] & (1 << m)))
                {
                    result = (i - m) + 1;
                    break;
                }
            }

            return result;
        }
    }
}
