using System;
using System.Collections.Generic;
using Algorithms.Sorting;
using Algorithms.Sorting.ArrayMerging;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    internal class KarkkainenSandersSuffixArray
    {
        public readonly int[] SuffixArray;

        private readonly int[] buffer;
        private readonly int[] suffixArray2;
        private readonly int[] suffixArray2SortBuffer;

        public KarkkainenSandersSuffixArray(string text)
        {
            SuffixArray = new int[text.Length];
            buffer = new int[text.Length + 1];
            suffixArray2 = new int[text.Length / 3];
            suffixArray2SortBuffer = new int[text.Length / 3];

            CreateSuffixArray(Characters.FromText(text));

            buffer = null;
            suffixArray2 = null;
            suffixArray2SortBuffer = null;
        }

        private void CreateSuffixArray(Characters chars)
        {
            if (chars.Length < 3)
            {
                CreateSmallestSuffixArray(chars);
                return;
            }

            CreateOnly01SuffixArray(chars, out bool isSorted);

            if (isSorted)
            {
                return;
            }

            int[] charRanks = buffer;

            CreateOnly2SuffixArray(chars, charRanks);

            Merging.Merge(SuffixArray, SuffixArray.Length - chars.Only01Length, chars.Only01Length,
                suffixArray2, 0, chars.Only2Length,
                SuffixArray, SuffixArray.Length - chars.Length,
                Comparer<int>.Create((s01, s2) => Compare(s01, s2, chars, charRanks)));
        }

        private void CreateOnly01SuffixArray(Characters chars, out bool isSorted)
        {
            int[] labels = SuffixArray;
            int[] sortedTriples = buffer;

            chars.AssignLabelsToTriples(labels, sortedTriples);
            isSorted = labels[labels.Length - 1] == labels.Length - 1;

            if (isSorted)
            {
                Array.Copy(sortedTriples, 0, SuffixArray, SuffixArray.Length - chars.Length, chars.Length);
                return;
            }

            CreateSuffixArray(chars.CreateOnly01Triples(labels, sortedTriples));
        }

        private void CreateSmallestSuffixArray(Characters chars)
        {
            if (chars.Length == 1)
            {
                SuffixArray[SuffixArray.Length - 1] = 0;
                return;
            }

            if (chars[0] >= chars[1])
            {
                SuffixArray[SuffixArray.Length - 2] = 1;
                SuffixArray[SuffixArray.Length - 1] = 0;
            }
            else
            {
                SuffixArray[SuffixArray.Length - 2] = 0;
                SuffixArray[SuffixArray.Length - 1] = 1;
            }
        }

        private void CreateOnly2SuffixArray(Characters chars, int[] charRanks)
        {
            for (int i = SuffixArray.Length - chars.Only01Length; i < SuffixArray.Length; ++i)
            {
                int originalIndex = chars.ToOriginalIndex(SuffixArray[i]);
                SuffixArray[i] = originalIndex;
                charRanks[originalIndex] = i;
            }

            for (int i = 2; i < chars.Length; i += 3)
            {
                suffixArray2[i / 3] = i;
            }

            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, chars.Only2Length, i => charRanks[i + 1]);
            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, chars.Only2Length, i => chars[i]);
        }

        private static int Compare(int index01, int index2, Characters symbols, int[] ranks01)
        {
            int cmp = symbols[index01].CompareTo(symbols[index2]);

            if (cmp != 0)
            {
                return cmp;
            }

            if (index01 == symbols.Length - 1)
            {
                return -1;
            }

            if (index2 == symbols.Length - 1)
            {
                return 1;
            }

            if (index01 % 3 == 0)
            {
                return ranks01[index01 + 1] < ranks01[index2 + 1] ? -1 : 1;
            }

            return -Compare(index2 + 1, index01 + 1, symbols, ranks01);
        }

        private class Arrays
        {
            public int[] SuffixArray;
            public int[] Buffer;
            public int[] Only2SuffixArray;
            public int[] Only2Buffer;
        }
    }
}