namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    internal struct Triple
    {
        public readonly int Start;
        private readonly int end;

        public Triple(int start, int end)
        {
            Start = start;
            this.end = end;
        }

        public int Label1(Label[] labels) => labels[Start].Value;
        public int Label2(Label[] labels) => Start > labels.Length - 2 ? 0 : labels[Start + 1].Value;
        public int Label3(Label[] labels) => end > labels.Length - 1 ? 0 : labels[end].Value;

        public bool Equals(Triple other, Label[] labels)
        {
            return Label1(labels) == other.Label1(labels)
                   && Label2(labels) == other.Label2(labels)
                   && Label3(labels) == other.Label3(labels);
        }
    }
}