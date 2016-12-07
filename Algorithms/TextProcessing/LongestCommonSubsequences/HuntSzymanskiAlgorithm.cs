using System.Collections.Generic;
using Algorithms.TextProcessing.LongestIncreasingSubsequences;

namespace Algorithms.TextProcessing.LongestCommonSubsequences
{
    public class HuntSzymanskiAlgorithm<T>
    {
        private readonly IEqualityComparer<T> comparer;

        public HuntSzymanskiAlgorithm(IEqualityComparer<T> comparer)
        {
            this.comparer = comparer;
        }

        public T[] FindLCS(T[] sequence1, T[] sequence2)
        {
            int[] matchIndexes = CreateSequencesMatchIndexes(sequence1, sequence2);
            int[] lis = LIS<int>.Find(matchIndexes);

            var lcs = new T[lis.Length];

            for (int i = 0; i < lis.Length; ++i)
            {
                lcs[i] = sequence2[lis[i]];
            }

            return lcs;
        }

        private int[] CreateSequencesMatchIndexes(T[] sequence1, T[] sequence2)
        {
            var indexes = new Dictionary<T, List<int>>(comparer);

            for (int i = 0; i < sequence1.Length; ++i)
            {
                T key = sequence1[i];

                if (!indexes.ContainsKey(key))
                {
                    indexes[key] = new List<int>();
                }
            }

            for (int i = sequence2.Length - 1; i >= 0; --i)
            {
                T key = sequence2[i];

                if (indexes.ContainsKey(key))
                {
                    indexes[key].Add(i);
                }
            }

            var result = new List<int>();

            for (int i = 0; i < sequence1.Length; ++i)
            {
                result.AddRange(indexes[sequence1[i]]);
            }

            return result.ToArray();
        }
    }
}