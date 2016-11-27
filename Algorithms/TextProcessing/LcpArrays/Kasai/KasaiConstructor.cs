using System;

namespace Algorithms.TextProcessing.LcpArrays.Kasai
{
    public class KasaiConstructor : ILcpArrayConstructor
    {
        public int[] Create(string text, int[] suffixArray)
        {
            var ranks = new int[suffixArray.Length];

            for (int i = 0; i < suffixArray.Length; ++i)
            {
                ranks[suffixArray[i]] = i;
            }

            var lcpArray = new int[text.Length - 1];
            int lcp = 0;

            for (int suffix1Index = 0; suffix1Index < text.Length; ++suffix1Index)
            {
                if (ranks[suffix1Index] + 1 == suffixArray.Length)
                {
                    lcp = Math.Max(0, lcp - 1);
                    continue;
                }

                int suffix2Index = suffixArray[ranks[suffix1Index] + 1];

                while (suffix1Index + lcp < text.Length
                    && suffix2Index + lcp < text.Length
                    && text[suffix1Index + lcp] == text[suffix2Index + lcp])
                {
                    ++lcp;
                }

                lcpArray[ranks[suffix1Index]] = lcp;
                lcp = Math.Max(0, lcp - 1);
            }

            return lcpArray;
        }
    }
}