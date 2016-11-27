using System.Linq;

namespace Algorithms.TextProcessing.SuffixArrays
{
    public class SuffixArray
    {
        private readonly int[] suffixArray;

        public SuffixArray(ISuffixArrayConstructor constructor, string text)
        {
            suffixArray = constructor.Create(text);

            int distinct = suffixArray.Distinct().Count();
            string[] suffixes = suffixArray.Select(x => text.Substring(x)).ToArray();
        }
    }
}