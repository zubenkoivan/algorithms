namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    internal struct Triple
    {
        public readonly int Start;
        public readonly int End;

        public Triple(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Class => Start % 3;
    }
}