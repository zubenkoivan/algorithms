namespace Algorithms.TextProcessing.SuffixArrays
{
    internal struct Range
    {
        public readonly int Start;
        public readonly int Middle;
        public readonly int End;

        public int Length => End - Start + 1;

        public Range Left => new Range(Start, Middle);

        public Range Right => new Range(Middle, End);

        public Range(int start, int end)
        {
            Start = start;
            End = end;
            Middle = (start + end) / 2;
        }

        public override string ToString()
        {
            return $"Start: {Start}, Middle: {Middle}, End: {End}";
        }
    }
}