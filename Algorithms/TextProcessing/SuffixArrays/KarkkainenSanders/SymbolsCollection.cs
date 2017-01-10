using Algorithms.Sorting;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    internal class SymbolsCollection
    {
        private readonly int[] symbols;

        public readonly int Length;
        public readonly int Length01;
        public readonly int Length0;
        public readonly int Length2;

        public int this[int i] => symbols[i];

        private SymbolsCollection(int[] symbols)
        {
            this.symbols = symbols;
            Length = symbols.Length - 2;
            Length01 = Length - Length / 3;
            Length0 = Length01 - Length01 / 2;
            Length2 = Length - Length01;
        }

        public static SymbolsCollection FromText(string text)
        {
            var symbols = new int[text.Length + 2];

            for (int i = 0; i < text.Length; ++i)
            {
                symbols[i] = text[i];
            }

            return new SymbolsCollection(symbols);
        }

        public SymbolsCollection CreateTriples01(int[] labels, int[] labelIndexes)
        {
            var triples01 = new int[Length01 + 2];

            for (int i = 0; i < Length; ++i)
            {
                int currentIndex = labelIndexes[i];

                if (currentIndex % 3 == 2)
                {
                    continue;
                }

                triples01[ToTriples01Index(currentIndex)] = labels[i];
            }

            return new SymbolsCollection(triples01);
        }

        private int ToTriples01Index(int index)
        {
            return index / 3 + index % 3 * Length0;
        }

        public int FromTriples01Index(int index)
        {
            return index % Length0 * 3 + index / Length0;
        }

        public bool MarkTriplesWithLabels(int[] labels, int[] labelIndexes)
        {
            int[] sortBuffer = labels;

            for (int i = 0; i < Length; ++i)
            {
                labelIndexes[i] = i;
            }

            RadixSort.Sort(labelIndexes, sortBuffer, Length, i => symbols[i + 2]);
            RadixSort.Sort(labelIndexes, sortBuffer, Length, i => symbols[i + 1]);
            RadixSort.Sort(labelIndexes, sortBuffer, Length, i => symbols[i]);

            int currentLabel = 0;
            int previousIndex = labelIndexes[0];
            labels[0] = 0;

            for (int i = 1; i < Length; ++i)
            {
                int currentIndex = labelIndexes[i];

                if (symbols[previousIndex] != symbols[currentIndex]
                    || symbols[previousIndex + 1] != symbols[currentIndex + 1]
                    || symbols[previousIndex + 2] != symbols[currentIndex + 2])
                {
                    ++currentLabel;
                }

                labels[i] = currentLabel;
                previousIndex = currentIndex;
            }

            return currentLabel == Length - 1;
        }
    }
}