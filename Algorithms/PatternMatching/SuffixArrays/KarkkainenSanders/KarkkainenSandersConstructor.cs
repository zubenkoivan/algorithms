using System;
using Algorithms.SortedArraysMerging;
using Algorithms.Sorting;

namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : ISuffixArrayConstructor
    {
        private static readonly Func<int, int[], int> Symbol1 = (i, symbols) => symbols[i];
        private static readonly Func<int, int[], int> Symbol2 = (i, symbols) => i < symbols.Length - 1 ? symbols[i + 1] : 0;
        private static readonly Func<int, int[], int> Symbol3 = (i, symbols) => i < symbols.Length - 2 ? symbols[i + 2] : 0;

        private int[] suffixArray;
        private int[] buffer;
        private int[] suffixArray2;
        private int[] suffixArray2SortBuffer;

        public int[] Create(string text)
        {
            int length = text.Length;
            int[] symbols = ToIntArray(text);
            suffixArray = new int[length];
            buffer = new int[length];
            suffixArray2 = new int[length / 3];
            suffixArray2SortBuffer = new int[length / 3];

            int[] result = suffixArray;
            FillSuffixArray(symbols);

            suffixArray = null;
            buffer = null;
            suffixArray2 = null;
            suffixArray2SortBuffer = null;

            return result;
        }

        private static int[] ToIntArray(string text)
        {
            var symbols = new int[text.Length];

            for (int i = 0; i < symbols.Length; ++i)
            {
                symbols[i] = text[i];
            }

            return symbols;
        }

        private void FillSuffixArray(int[] symbols)
        {
            if (symbols.Length < 3)
            {
                CreateSimpleSuffixArray(symbols);
                return;
            }

            int length01 = symbols.Length - symbols.Length / 3;
            int suffixArray01Start = suffixArray.Length - length01;
            int length0 = length01 - length01 / 2;
            int length2 = symbols.Length - length01;

            int[] labels = suffixArray;
            int[] indexes = buffer;

            if (MarkTriplesWithLabels(symbols, labels, indexes))
            {
                Array.Copy(indexes, 0, suffixArray, suffixArray.Length - symbols.Length, symbols.Length);
                return;
            }

            FillSuffixArray(CreateTriples01(labels, indexes, symbols.Length, length0, length01));

            int[] ranks01 = buffer;

            for (int i = suffixArray01Start; i < suffixArray.Length; ++i)
            {
                int originalIndex = suffixArray[i] % length0 * 3 + suffixArray[i] / length0;
                suffixArray[i] = originalIndex;
                ranks01[originalIndex] = i;
            }

            FillSuffixArray2(symbols, ranks01, length2);

            SortedArrays.Merge(suffixArray, suffixArray01Start, length01,
                suffixArray2, 0, length2,
                (s01, s2) => Compare(s01, s2, symbols, ranks01));
        }

        private void CreateSimpleSuffixArray(int[] symbols)
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

        private static bool MarkTriplesWithLabels(int[] symbols, int[] labels, int[] indexes)
        {
            int[] sortBuffer = labels;

            for (int i = 0; i < symbols.Length; ++i)
            {
                indexes[i] = i;
            }

            RadixSort.Sort(indexes, sortBuffer, symbols.Length, i => Symbol3(i, symbols));
            RadixSort.Sort(indexes, sortBuffer, symbols.Length, i => Symbol2(i, symbols));
            RadixSort.Sort(indexes, sortBuffer, symbols.Length, i => Symbol1(i, symbols));

            int currentLabel = 0;
            int previousIndex = indexes[0];
            bool allSymbolsDifferent = true;
            labels[0] = 0;

            for (int i = 1; i < symbols.Length; ++i)
            {
                int currentIndex = indexes[i];
                bool firstSymbolsDifferent = Symbol1(previousIndex, symbols) != Symbol1(currentIndex, symbols);

                if (firstSymbolsDifferent
                    || Symbol2(previousIndex, symbols) != Symbol2(currentIndex, symbols)
                    || Symbol3(previousIndex, symbols) != Symbol3(currentIndex, symbols))
                {
                    ++currentLabel;
                }

                labels[i] = currentLabel;
                allSymbolsDifferent &= firstSymbolsDifferent;
                previousIndex = currentIndex;
            }

            return allSymbolsDifferent;
        }

        private static int[] CreateTriples01(int[] labels, int[] indexes, int length, int length0, int length01)
        {
            var triples01 = new int[length01];

            for (int i = 0; i < length; ++i)
            {
                int currentIndex = indexes[i];
                int @class = currentIndex % 3;

                if (@class == 2)
                {
                    continue;
                }

                int index = currentIndex / 3 + @class * length0;
                triples01[index] = labels[i];
            }

            return triples01;
        }

        private void FillSuffixArray2(int[] symbols, int[] ranks01, int length2)
        {
            for (int i = 2; i < symbols.Length; i += 3)
            {
                suffixArray2[i / 3] = i;
            }

            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, length2, i => ranks01[i + 1]);
            RadixSort.Sort(suffixArray2, suffixArray2SortBuffer, length2, i => symbols[i]);
        }

        private static int Compare(int suffix01, int suffix2, int[] symbols, int[] ranks01)
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