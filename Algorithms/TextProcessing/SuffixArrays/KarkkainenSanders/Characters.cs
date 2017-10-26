using Algorithms.Sorting;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    internal class Characters
    {
        private readonly int[] chars;

        public readonly int Length;
        public readonly int Only01Length;
        public readonly int Only0Length;
        public readonly int Only2Length;

        public int this[int i] => chars[i];

        private Characters(int[] chars)
        {
            this.chars = chars;
            Length = chars.Length - 2;
            Only01Length = Length - Length / 3;
            Only0Length = Only01Length - Only01Length / 2;
            Only2Length = Length - Only01Length;
        }

        public static Characters FromText(string text)
        {
            var symbols = new int[text.Length + 2];

            for (int i = 0; i < text.Length; ++i)
            {
                symbols[i] = text[i];
            }

            return new Characters(symbols);
        }

        public Characters CreateOnly01Triples(int[] labels, int[] sortedTriples)
        {
            var triples = new int[Only01Length + 2];

            for (int i = 0; i < Length; ++i)
            {
                int curr = sortedTriples[i];

                if (curr % 3 != 2)
                {
                    triples[curr / 3 + curr % 3 * Only0Length] = labels[i];
                }
            }

            return new Characters(triples);
        }

        public int ToOriginalIndex(int index)
        {
            return index % Only0Length * 3 + index / Only0Length;
        }

        public void AssignLabelsToTriples(int[] labels, int[] sortedTriples)
        {
            for (int i = 0; i < Length; ++i)
            {
                sortedTriples[i] = i;
            }

            int[] sortBuffer = labels;

            RadixSort.Sort(sortedTriples, sortBuffer, Length, i => chars[i + 2]);
            RadixSort.Sort(sortedTriples, sortBuffer, Length, i => chars[i + 1]);
            RadixSort.Sort(sortedTriples, sortBuffer, Length, i => chars[i]);

            int label = 0;
            int prev = sortedTriples[0];
            labels[0] = 0;

            for (int i = 1; i < Length; ++i)
            {
                int curr = sortedTriples[i];

                if (chars[prev] != chars[curr]
                    || chars[prev + 1] != chars[curr + 1]
                    || chars[prev + 2] != chars[curr + 2])
                {
                    ++label;
                }

                labels[i] = label;
                prev = curr;
            }
        }
    }
}