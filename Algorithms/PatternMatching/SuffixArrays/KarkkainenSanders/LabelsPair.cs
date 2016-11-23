namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    internal struct LabelsPair
    {
        public readonly int SourceIndex;
        public readonly int Label1;
        public readonly int Label2;

        public LabelsPair(int sourceIndex, int label1, int label2)
        {
            SourceIndex = sourceIndex;
            Label1 = label1;
            Label2 = label2;
        }
    }
}