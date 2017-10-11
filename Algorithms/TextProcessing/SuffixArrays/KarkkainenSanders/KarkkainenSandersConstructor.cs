using Algorithms.TextProcessing.Abstractions;

namespace Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : SuffixArrayConstructor
    {
        public override int[] Construct(string text)
        {
            return new KarkkainenSandersSuffixArray(text).SuffixArray;
        }
    }
}