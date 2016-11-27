using System;
using Algorithms.SortedArraysMerging;
using Algorithms.Sorting;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : ISuffixArrayConstructor
    {
        private int[] suffixArray;
        private int[] buffer;
        private int[] suffixArray2;
        private int[] suffixArray2SortBuffer;

        public int[] Create(string text)
        {
            suffixArray = new int[text.Length];
            buffer = new int[text.Length];
            suffixArray2 = new int[text.Length / 3];
            suffixArray2SortBuffer = new int[text.Length / 3];

            int[] result = suffixArray;
            FillSuffixArray(SymbolsCollection.FromText(text));

            suffixArray = null;
            buffer = null;
            suffixArray2 = null;
            suffixArray2SortBuffer = null;

            return result;
        }

        private void FillSuffixArray(SymbolsCollection symbols)
        {
            if (symbols.Length < 3)
            {
                CreateSimpleSuffixArray(symbols);
                return;
            }

            int[] labels = suffixArray;
            int[] indexes = buffer;

            if (symbols.MarkTriplesWithLabels(labels, indexes))
            {
                Array.Copy(indexes, 0, suffixArray, suffixArray.Length - symbols.Length, symbols.Length);
                return;
            }

            FillSuffixArray(symbols.CreateTriples01(labels, indexes));

            int[] ranks01 = buffer;
            int suffixArray01Start = suffixArray.Length - symbols.Length01;

            for (int i = suffixArray01Start; i < suffixArray.Length; ++i)
            {
                int originalIndex = symbols.FromTriples01Index(suffixArray[i]);
                suffixArray[i] = originalIndex;
                ranks01[originalIndex] = i;
            }

            FillSuffixArray2(symbols, ranks01);

            SortedArrays.Merge(suffixArray, suffixArray01Start, symbols.Length01,
                suffixArray2, 0, symbols.Length2,
                (s01, s2) => Compare(s01, s2, symbols, ranks01));
        }

        private void CreateSimpleSuffixArray(SymbolsCollection symbols)
        {
            if (symbols.Length == 1)
            {
                suffixArray[suffixArray.Length - 1] = 0;
            }

            if (symbols[0] < symbols[1])
            {
                suffixArray[suffixArray.Length - 2] = 0;
                suffixArray[suffixArray.Length - 1] = 1;
                return;
            }

            suffixArray[suffixArray.Length - 2] = 1;
            suffixArray[suffixArray.Length - 1] = 0;
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

        private static int Compare(int suffix01, int suffix2, SymbolsCollection symbols, int[] ranks01)
        {
            int cmp = symbols[suffix01].CompareTo(symbols[suffix2]);

            if (cmp != 0)
            {
                return cmp;
            }

            if (suffix2 == symbols.Length - 1)
            {
                return 1;
            }

            if (suffix01 % 3 == 0)
            {
                return ranks01[suffix01 + 1] < ranks01[suffix2 + 1] ? -1 : 1;
            }

            return -Compare(suffix2 + 1, suffix01 + 1, symbols, ranks01);
        }
    }
}