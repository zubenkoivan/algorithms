namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class Range
    {
        public readonly int Start;
        public readonly int Length;

        public Range(int start, int length)
        {
            Start = start;
            Length = length;
        }

        public override string ToString()
        {
            return $"Start: {Start}, Length: {Length}";
        }

        public Range Reduce(int skip, int count)
        {
            return new Range(Start + skip, count);
        }

        public int[] Indexes()
        {
            var indexes = new int[Length];

            for (int i = 0; i < Length; ++i)
            {
                indexes[i] = Start + i;
            }

            return indexes;
        }
    }
}