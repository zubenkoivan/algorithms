namespace Algorithms.PatternMatching.SuffixArrays
{
    public class SuffixArray
    {
        private readonly int[] suffixArray;

        public SuffixArray(ISuffixArrayConstructor constructor, string text)
        {
            suffixArray = constructor.Create(text);
        }
    }
}