using Algorithms.TextProcessing.LcpArrays;

namespace Algorithms.TextProcessing.SuffixArrays
{
    public class SuffixArray
    {
        private readonly string text;
        private readonly int[] suffixArray;
        private readonly LcpTree lcpTree;

        public SuffixArray(ISuffixArrayConstructor suffixArrayConstructor, ILcpArrayConstructor lcpArrayConstructor,
            string text)
        {
            this.text = text;
            suffixArray = suffixArrayConstructor.Create(text);
            lcpTree = new LcpTree(lcpArrayConstructor.Create(text, suffixArray));
        }

        public bool HasPattern(string pattern)
        {
            Lcp patternLcp;

            if (!CanContainPattern(pattern, out patternLcp))
            {
                return false;
            }

            if (patternLcp.Left == pattern.Length || patternLcp.Right == pattern.Length)
            {
                return true;
            }

            return HasPattern(pattern, patternLcp);
        }

        private bool HasPattern(string pattern, Lcp patternLcp)
        {
            LcpNode currentNode = lcpTree.Root;
            var currentRange = new Range(0, suffixArray.Length - 1);

            while (currentRange.Length > 2)
            {
                var middleLcp = lcpTree.Lcp(currentNode, currentRange);

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

        private bool CanContainPattern(string pattern, out Lcp patternLcp)
        {
            patternLcp = default(Lcp);

            if (pattern.Length > text.Length)
            {
                return false;
            }

            patternLcp = new Lcp(ComputeLcp(suffixArray[0], pattern),
                ComputeLcp(suffixArray[suffixArray.Length - 1], pattern));

            var leftTextIndex = suffixArray[0] + patternLcp.Left;
            var rightTextIndex = suffixArray[suffixArray.Length - 1] + patternLcp.Right;

            return patternLcp.Left == pattern.Length
                   || patternLcp.Right == pattern.Length
                   || (text[leftTextIndex] < pattern[patternLcp.Left]
                       && pattern[patternLcp.Right] < text[rightTextIndex]);
        }

        private bool TryFindPattern(string pattern, Lcp middleLcp, ref LcpNode currentNode,
            ref Range currentRange, ref Lcp patternLcp)
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