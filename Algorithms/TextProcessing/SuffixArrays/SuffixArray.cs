using Algorithms.TextProcessing.LCPArrays;

namespace Algorithms.TextProcessing.SuffixArrays
{
    public class SuffixArray
    {
        private readonly string text;
        private readonly int[] suffixArray;
        private readonly LCPTree lcpTree;

        public SuffixArray(ISuffixArrayConstructor suffixArrayConstructor, ILCPArrayConstructor ilcpArrayConstructor,
            string text)
        {
            this.text = text;
            suffixArray = suffixArrayConstructor.Create(text);
            lcpTree = new LCPTree(ilcpArrayConstructor.Create(text, suffixArray));
        }

        public bool HasPattern(string pattern)
        {
            if (!CanContainPattern(pattern, out LCP patternLcp))
            {
                return false;
            }

            if (patternLcp.Left == pattern.Length || patternLcp.Right == pattern.Length)
            {
                return true;
            }

            return HasPattern(pattern, patternLcp);
        }

        private bool HasPattern(string pattern, LCP patternLcp)
        {
            LCPNode currentNode = lcpTree.Root;
            var currentRange = new Range(0, suffixArray.Length - 1);

            while (currentRange.Length > 2)
            {
                LCP middleLcp = lcpTree.Lcp(currentNode, currentRange);

                if (patternLcp.Min < middleLcp.Min)
                {
                    return false;
                }

                if (patternLcp.ShouldGoLeft(middleLcp))
                {
                    currentNode = lcpTree.GoLeft(currentNode, currentRange);
                    currentRange = currentRange.Left;
                    patternLcp = patternLcp.GoLeft(middleLcp);
                }
                else if (patternLcp.ShouldGoRight(middleLcp))
                {
                    currentNode = lcpTree.GoRight(currentNode, currentRange);
                    currentRange = currentRange.Right;
                    patternLcp = patternLcp.GoRight(middleLcp);
                }
                else if (TryFindPattern(pattern, middleLcp, ref currentNode, ref currentRange, ref patternLcp))
                {
                    return true;
                }
            }

            return false;
        }

        private bool CanContainPattern(string pattern, out LCP patternLcp)
        {
            patternLcp = default(LCP);

            if (pattern.Length > text.Length)
            {
                return false;
            }

            patternLcp = new LCP(ComputeLcp(suffixArray[0], pattern),
                ComputeLcp(suffixArray[suffixArray.Length - 1], pattern));

            int leftTextIndex = suffixArray[0] + patternLcp.Left;
            int rightTextIndex = suffixArray[suffixArray.Length - 1] + patternLcp.Right;

            return patternLcp.Left == pattern.Length
                   || patternLcp.Right == pattern.Length
                   || (text[leftTextIndex] < pattern[patternLcp.Left]
                       && pattern[patternLcp.Right] < text[rightTextIndex]);
        }

        private bool TryFindPattern(string pattern, LCP middleLcp, ref LCPNode currentNode,
            ref Range currentRange, ref LCP patternLcp)
        {
            int lcp = patternLcp.Max;
            lcp += ComputeLcp(suffixArray[currentRange.Middle] + lcp, pattern, lcp);

            if (lcp == pattern.Length)
            {
                return true;
            }

            int textIndex = suffixArray[currentRange.Middle] + lcp;

            if (textIndex >= text.Length || pattern[lcp] > text[textIndex])
            {
                currentNode = lcpTree.GoRight(currentNode, currentRange);
                currentRange = currentRange.Right;
                patternLcp = patternLcp.GoRight(middleLcp, lcp);
            }
            else
            {
                currentNode = lcpTree.GoLeft(currentNode, currentRange);
                currentRange = currentRange.Left;
                patternLcp = patternLcp.GoLeft(middleLcp, lcp);
            }

            return false;
        }

        private int ComputeLcp(int textFrom, string pattern, int patternFrom = 0)
        {
            int lcp = 0;

            while (textFrom + lcp < text.Length
                && patternFrom + lcp < pattern.Length
                && text[textFrom + lcp] == pattern[patternFrom + lcp])
            {
                ++lcp;
            }

            return lcp;
        }
    }
}