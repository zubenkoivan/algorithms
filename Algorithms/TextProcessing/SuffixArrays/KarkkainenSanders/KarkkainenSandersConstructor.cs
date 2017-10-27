using System;
using System.Collections.Generic;
using Algorithms.Sorting;
using Algorithms.Sorting.ArrayMerging;
using Algorithms.TextProcessing.Abstractions;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : SuffixArrayConstructor
    {
        public override int[] Construct(string text)
        {
            var arrays = new Arrays(text.Length);
            CreateSuffixArray(arrays, Characters.FromText(text));
            return arrays.SuffixArray;
        }

        private static void CreateSuffixArray(Arrays arrays, Characters chars)
        {
            if (chars.Length < 3)
            {
                CreateSmallestSuffixArray(arrays, chars);
                return;
            }

            CreateOnly01SuffixArray(arrays, chars, out bool isSorted);

            if (isSorted)
            {
                return;
            }

            int[] charRanks = arrays.Buffer;

            CreateOnly2SuffixArray(arrays, chars, charRanks);

            Merging.Merge(arrays.SuffixArray, arrays.SuffixArray.Length - chars.Only01Length, chars.Only01Length,
                arrays.Only2SuffixArray, 0, chars.Only2Length,
                arrays.SuffixArray, arrays.SuffixArray.Length - chars.Length,
                Comparer<int>.Create((s01, s2) => Compare(s01, s2, chars, charRanks)));
        }

        private static void CreateOnly01SuffixArray(Arrays arrays, Characters chars, out bool isSorted)
        {
            int[] labels = arrays.SuffixArray;
            int[] sortedTriples = arrays.Buffer;

            chars.AssignLabelsToTriples(labels, sortedTriples);
            isSorted = labels[labels.Length - 1] == labels.Length - 1;

            if (isSorted)
            {
                Array.Copy(sortedTriples, 0, arrays.SuffixArray, arrays.SuffixArray.Length - chars.Length,
                    chars.Length);
                return;
            }

            CreateSuffixArray(arrays, chars.CreateOnly01Triples(labels, sortedTriples));
        }

        private static void CreateSmallestSuffixArray(Arrays arrays, Characters chars)
        {
            if (chars.Length == 1)
            {
                arrays.SuffixArray[arrays.SuffixArray.Length - 1] = 0;
                return;
            }

            if (chars[0] >= chars[1])
            {
                arrays.SuffixArray[arrays.SuffixArray.Length - 2] = 1;
                arrays.SuffixArray[arrays.SuffixArray.Length - 1] = 0;
            }
            else
            {
                arrays.SuffixArray[arrays.SuffixArray.Length - 2] = 0;
                arrays.SuffixArray[arrays.SuffixArray.Length - 1] = 1;
            }
        }

        private static void CreateOnly2SuffixArray(Arrays arrays, Characters chars, int[] char01Ranks)
        {
            for (int i = arrays.SuffixArray.Length - chars.Only01Length; i < arrays.SuffixArray.Length; ++i)
            {
                int originalIndex = chars.ToOriginalIndex(arrays.SuffixArray[i]);
                arrays.SuffixArray[i] = originalIndex;
                char01Ranks[originalIndex] = i;
            }

            for (int i = 2; i < chars.Length; i += 3)
            {
                arrays.Only2SuffixArray[i / 3] = i;
            }

            RadixSort.Sort(arrays.Only2SuffixArray, arrays.Only2Buffer, chars.Only2Length, i => char01Ranks[i + 1]);
            RadixSort.Sort(arrays.Only2SuffixArray, arrays.Only2Buffer, chars.Only2Length, i => chars[i]);
        }

        private static int Compare(int suffix01, int suffix2, Characters chars, int[] char01Ranks)
        {
            int cmp = chars[suffix01].CompareTo(chars[suffix2]);

            if (cmp != 0)
            {
                return cmp;
            }

            if (suffix01 == chars.Length - 1)
            {
                return -1;
            }

            if (suffix2 == chars.Length - 1)
            {
                return 1;
            }

            if (suffix01 % 3 == 0)
            {
                return char01Ranks[suffix01 + 1] < char01Ranks[suffix2 + 1] ? -1 : 1;
            }

            return -Compare(suffix2 + 1, suffix01 + 1, chars, char01Ranks);
        }

        private class Arrays
        {
            public readonly int[] SuffixArray;
            public readonly int[] Buffer;
            public readonly int[] Only2SuffixArray;
            public readonly int[] Only2Buffer;

            public Arrays(int length)
            {
                SuffixArray = new int[length];
                Buffer = new int[length + 1];
                Only2SuffixArray = new int[length / 3];
                Only2Buffer = new int[length / 3];
            }
        }
    }
}