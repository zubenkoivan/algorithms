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
            int[] matchIndexes = CreateMatchIndexes(sequence1, sequence2);
            int[] lis = LIS<int>.Find(matchIndexes);

            var lcs = new T[lis.Length];

            for (int i = 0; i < lis.Length; ++i)
            {
                lcs[i] = sequence2[lis[i]];
            }

            return lcs;
        }

        private int[] CreateMatchIndexes(T[] sequence1, T[] sequence2)
        {
            Dictionary<T, List<int>> reversedIndexes = CreateReversedMatchIndexesMap(sequence1, sequence2);
            int resultLength = 0;

            for (int i = 0; i < sequence1.Length; ++i)
            {
                resultLength += reversedIndexes[sequence1[i]].Count;
            }

            var result = new int[resultLength];

            for (int i = sequence1.Length - 1; i >= 0; --i)
            {
                List<int> indexes = reversedIndexes[sequence1[i]];
                resultLength -= indexes.Count;
                indexes.CopyTo(result, resultLength);
            }

            return result;
        }

        private Dictionary<T, List<int>> CreateReversedMatchIndexesMap(T[] sequence1, T[] sequence2)
        {
            var matchIndexes = new Dictionary<T, List<int>>(comparer);

            for (int i = 0; i < sequence1.Length; ++i)
            {
                T key = sequence1[i];

                if (!matchIndexes.ContainsKey(key))
                {
                    matchIndexes[key] = new List<int>();
                }
            }

            for (int i = sequence2.Length - 1; i >= 0; --i)
            {
                T key = sequence2[i];

                if (matchIndexes.ContainsKey(key))
                {
                    matchIndexes[key].Add(i);
                }
            }

            return matchIndexes;
        }
    }
}