using System;
using System.Collections.Generic;

namespace Algorithms.TextProcessing.PatternMatching.ZFunctionAlgorithm
{
    public class ZFunction
    {
        private readonly string pattern;
        private readonly int patternLength;
        private readonly int[] zFunction;

        public ZFunction(string pattern)
        {
            this.pattern = pattern;
            patternLength = pattern.Length;
            zFunction = new int[pattern.Length];

            int index = 1;
            foreach (int matchLength in Calculate(pattern, 1))
            {
                zFunction[index++] = matchLength;
            }
        }

        private IEnumerable<int> Calculate(string text, int startIndex)
        {
            var rightBlock = new RightBlock();

            for (int i = startIndex; i < text.Length; ++i)
            {
                int matchLength;

                if (rightBlock.Covers(i))
                {
                    int k = i - rightBlock.LeftBorder;
                    int rightPartLength = rightBlock.RightPartLength(i);

                    matchLength = zFunction[k] == rightPartLength
                        ? Lcp(text, rightBlock.NextIndex, k + zFunction[k])
                        : Math.Min(rightPartLength, zFunction[k]);
                }
                else
                {
                    matchLength = Lcp(text, i);
                }

                rightBlock.Update(i, matchLength);

                yield return matchLength;
            }
        }

        private int Lcp(string text, int textFrom, int patternFrom = 0)
        {
            int result = 0;

            while (patternFrom < patternLength && textFrom < text.Length && pattern[patternFrom] == text[textFrom])
            {
                ++result;
                ++patternFrom;
                ++textFrom;
            }

            return result;
        }

        public IEnumerable<int> IndexesIn(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < patternLength)
            {
                yield break;
            }
            int textIndex = 0;

            foreach (int matchLength in Calculate(text, 0))
            {
                if (matchLength == patternLength)
                {
                    yield return textIndex;
                }

                ++textIndex;
            }
        }
    }
}
