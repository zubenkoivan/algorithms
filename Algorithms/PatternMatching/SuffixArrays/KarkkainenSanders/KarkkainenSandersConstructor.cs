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
            suffixArray = new int[text.Length];
            buffer = new int[text.Length + 1];
            suffixArray2 = new int[text.Length / 3];
            suffixArray2SortBuffer = new int[text.Length / 3];

            int[] result = suffixArray;
            CreateSuffixArray(ToIntArray(text));

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

        private void CreateSuffixArray(int[] symbols)
        {
            if (symbols.Length < 3)
            {
                CreateSimpleSuffixArray(symbols);
                return;
            }

            int length01 = symbols.Length - symbols.Length / 3;
            int length0 = length01 - length01 / 2;
            int length2 = symbols.Length - length01;
            int suffixArray01Start = suffixArray.Length - length01;
            int[] ranks01 = buffer;

            CreateSuffixArray(CreateTriples01(symbols, length01, length0));

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

        private int[] CreateTriples01(int[] symbols, int length01, int length0)
        {
            var triples01 = new int[length01];
            int[] sortBuffer = buffer;
            int[] ranks = buffer;

            for (int i = 0; i < symbols.Length; ++i)
            {
                sortBuffer[i] = i / 2 * 3 + i % 2;
            }

            RadixSort.Sort(sortBuffer, triples01, triples01.Length, i => Symbol3(i, symbols));
            RadixSort.Sort(sortBuffer, triples01, triples01.Length, i => Symbol2(i, symbols));
            RadixSort.Sort(sortBuffer, triples01, triples01.Length, i => Symbol1(i, symbols));

            for (int i = 0; i < triples01.Length; ++i)
            {
                int @class = sortBuffer[i] % 3;
                ranks[i] = sortBuffer[i] / 3 + @class * length0;
            }

            int currentLabel = 0;
            int previous = triples01[0];

            for (int i = 0; i < length01; ++i)
            {
                int current = sortBuffer[i];

                if (Symbol1(previous, symbols) != Symbol1(current, symbols)
                    || Symbol2(previous, symbols) != Symbol2(current, symbols)
                    || Symbol3(previous, symbols) != Symbol3(current, symbols))
                {
                    previous = current;
                    ++currentLabel;
                }

                triples01[ranks[i]] = currentLabel;
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