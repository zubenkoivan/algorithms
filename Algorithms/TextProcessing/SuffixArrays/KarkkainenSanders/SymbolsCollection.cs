using System;
using Algorithms.Sorting;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    internal class SymbolsCollection
    {
        private static readonly Func<int, int[], int> Symbol1 = (i, symbols) => symbols[i];
        private static readonly Func<int, int[], int> Symbol2 = (i, symbols) => i < symbols.Length - 1 ? symbols[i + 1] : 0;
        private static readonly Func<int, int[], int> Symbol3 = (i, symbols) => i < symbols.Length - 2 ? symbols[i + 2] : 0;

        private readonly int[] symbols;

        public readonly int Length01;
        public readonly int Length0;
        public readonly int Length2;

        public int Length => symbols.Length;
        public int this[int i] => symbols[i];

        private SymbolsCollection(int[] symbols)
        {
            this.symbols = symbols;
            Length01 = symbols.Length - symbols.Length / 3;
            Length0 = Length01 - Length01 / 2;
            Length2 = symbols.Length - Length01;
        }

        public static SymbolsCollection FromText(string text)
        {
            var symbols = new int[text.Length];

            for (int i = 0; i < symbols.Length; ++i)
            {
                symbols[i] = text[i];
            }

            return new SymbolsCollection(symbols);
        }

        public bool MarkTriplesWithLabels(int[] labels, int[] indexes)
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

        public SymbolsCollection CreateTriples01(int[] labels, int[] indexes)
        {
            var triples01 = new int[Length01];

            for (int i = 0; i < symbols.Length; ++i)
            {
                int currentIndex = indexes[i];

                if (currentIndex % 3 == 2)
                {
                    continue;
                }

                triples01[ToTriples01Index(currentIndex)] = labels[i];
            }

            return new SymbolsCollection(triples01);
        }

        public int ToTriples01Index(int index)
        {
            return index / 3 + index % 3 * Length0;
        }

        public int FromTriples01Index(int index)
        {
            return index % Length0 * 3 + index / Length0;
        }
    }
}