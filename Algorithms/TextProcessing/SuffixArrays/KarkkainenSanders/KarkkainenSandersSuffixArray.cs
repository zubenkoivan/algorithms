using System;
using System.Collections.Generic;
using Algorithms.SortedArraysMerging;
using Algorithms.Sorting;

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

            FillSuffixArray(SymbolsCollection.FromText(text));

            buffer = null;
            suffixArray2 = null;
            suffixArray2SortBuffer = null;
        }

        private void FillSuffixArray(SymbolsCollection symbols)
        {
            if (symbols.Length < 3)
            {
                CreateSimpleSuffixArray(symbols);
                return;
            }

            int[] labels = SuffixArray;
            int[] labelIndexes = buffer;

            if (symbols.MarkTriplesWithLabels(labels, labelIndexes))
            {
                Array.Copy(labelIndexes, 0, SuffixArray, SuffixArray.Length - symbols.Length, symbols.Length);
                return;
            }

            FillSuffixArray(symbols.CreateTriples01(labels, labelIndexes));

            int[] ranks01 = buffer;
            int suffixArray01Start = SuffixArray.Length - symbols.Length01;

            for (int i = suffixArray01Start; i < SuffixArray.Length; ++i)
            {
                int originalIndex = symbols.FromTriples01Index(SuffixArray[i]);
                SuffixArray[i] = originalIndex;
                ranks01[originalIndex] = i;
            }

            FillSuffixArray2(symbols, ranks01);

            SortedArrays.MergeInPlace(SuffixArray, suffixArray01Start, symbols.Length01,
                suffixArray2, 0, symbols.Length2, Comparer<int>.Create((s01, s2) => Compare(s01, s2, symbols, ranks01)));
        }

        private void CreateSimpleSuffixArray(SymbolsCollection symbols)
        {
            if (symbols.Length == 1)
            {
                SuffixArray[SuffixArray.Length - 1] = 0;
            }

            if (symbols[0] < symbols[1])
            {
                SuffixArray[SuffixArray.Length - 2] = 0;
                SuffixArray[SuffixArray.Length - 1] = 1;
                return;
            }

            SuffixArray[SuffixArray.Length - 2] = 1;
            SuffixArray[SuffixArray.Length - 1] = 0;
        }

        private void FillSuffixArray2(SymbolsCollection symbols, int[] ranks01)
        {
            for (int i = 2; i < symbols.Length; i += 3)
            {
                suffixArray2[i / 3] = i;
            }

            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, symbols.Length2, i => ranks01[i + 1]);
            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, symbols.Length2, i => symbols[i]);
        }

        private static int Compare(int index01, int index2, SymbolsCollection symbols, int[] ranks01)
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
    }
}