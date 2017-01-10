namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : ISuffixArrayConstructor
    {
        public int[] Create(string text)
        {
            return new KarkkainenSandersSuffixArray(text).SuffixArray;
        }
    }
}