using System;
using Algorithms.TextProcessing.LcpArrays;

namespace Algorithms.TextProcessing.SuffixArrays
{
    public class SuffixArray
    {
        private readonly string text;
        private readonly int[] suffixArray;
        private readonly int[] lcpTree;

        public SuffixArray(ISuffixArrayConstructor suffixArrayConstructor, ILcpArrayConstructor lcpArrayConstructor,
            string text)
        {
            this.text = text;
            suffixArray = suffixArrayConstructor.Create(text);
            lcpTree = CreateLcpTree(lcpArrayConstructor.Create(text, suffixArray));
        }

        private static int[] CreateLcpTree(int[] lcpArray)
        {
            int length = lcpArray.Length * (lcpArray.Length + 1) / 2 - 1;
            var lcpTree = new int[length];

            Array.Copy(lcpArray, 0, lcpTree, length - lcpArray.Length, lcpArray.Length);

            for (int i = lcpArray.Length - 1; i > 1; --i)
            {
                int skip = i * (i - 1) / 2 - 1;

                for (int j = 0; j < i; ++j)
                {
                    int leftIndex = skip + j + i;
                    lcpTree[skip + j] = Math.Min(lcpTree[leftIndex], lcpTree[leftIndex + 1]);
                }
            }

            return lcpTree;
        }

        public bool HasPattern(string pattern)
        {
            var range = new Range(0, suffixArray.Length - 1);

            var patternLcp = new Lcp(Lcp(suffixArray[range.Start], pattern, 0),
                Lcp(suffixArray[range.End], pattern, 0));

            if (patternLcp.Left == pattern.Length || patternLcp.Right == pattern.Length)
            {
                return true;
            }

            while (range.Length > 2)
            {
                var middleLcp = new Lcp(Lcp(range.Left), Lcp(range.Right));

                if (patternLcp.ShouldGoLeft(middleLcp))
                {
                    range = range.Left;
                    patternLcp = patternLcp.GoLeft(middleLcp);
                }
                else if (patternLcp.ShouldGoRight(middleLcp))
                {
                    range = range.Right;
                    patternLcp = patternLcp.GoRight(middleLcp);
                }
                else if (TryFindPattern(pattern, middleLcp, ref patternLcp, ref range))
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryFindPattern(string pattern, Lcp middleLcp, ref Lcp patternLcp, ref Range range)
        {
            int lcp = patternLcp.Max;
            lcp += Lcp(suffixArray[range.Middle] + lcp, pattern, lcp);

            if (lcp == pattern.Length)
            {
                return true;
            }

            int textIndex = suffixArray[range.Middle] + lcp;

            if (textIndex >= text.Length || pattern[lcp] > text[textIndex])
            {
                range = range.Right;
                patternLcp = patternLcp.GoRight(middleLcp, lcp);
            }
            else
            {
                range = range.Left;
                patternLcp = patternLcp.GoLeft(middleLcp, lcp);
            }

            return false;
        }

        private int Lcp(Range range)
        {
            int levelLength = suffixArray.Length - range.Length + 1;
            int index = levelLength * (levelLength - 1) / 2 - 1 + range.Start;
            return lcpTree[index];
        }

        private int Lcp(int textFrom, string pattern, int patternFrom)
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