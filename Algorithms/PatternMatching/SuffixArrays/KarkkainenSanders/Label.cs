namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    internal struct Label
    {
        public readonly int SourceIndex;
        public readonly int Value;

        public Label(int sourceIndex, int value)
        {
            SourceIndex = sourceIndex;
            Value = value;
        }
    }
}