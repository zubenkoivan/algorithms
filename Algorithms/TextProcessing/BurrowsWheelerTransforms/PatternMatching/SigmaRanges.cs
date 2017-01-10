using System.Collections.Generic;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class SigmaRanges
    {
        private readonly Dictionary<char, Range> ranges;

        public readonly int SigmaSize;

        public Range this[char symbol] => ranges[symbol];

        public SigmaRanges(string transformedText, int[] firstToLastMap)
        {
            if (transformedText.Length == 0)
            {
                ranges = new Dictionary<char, Range>();
                return;
            }

            SigmaSize = GetSigmaSize(transformedText, firstToLastMap);
            ranges = CreateRanges(transformedText, firstToLastMap, SigmaSize);
        }

        private static Dictionary<char, Range> CreateRanges(string transformedText, int[] firstToLastMap, int sigmaSize)
        {
            var ranges = new Dictionary<char, Range>(sigmaSize);
            char currentSymbol = transformedText[firstToLastMap[0]];
            int length = 1;

            for (int i = 1; i < firstToLastMap.Length; ++i)
            {
                char symbol = transformedText[firstToLastMap[i]];

                if (currentSymbol == symbol)
                {
                    ++length;
                    continue;
                }

                ranges.Add(currentSymbol, new Range(i - length, length));
                length = 1;
                currentSymbol = symbol;
            }

            ranges.Add(currentSymbol, new Range(firstToLastMap.Length - length, length));

            return ranges;
        }

        private static int GetSigmaSize(string transformedText, int[] firstToLastMap)
        {
            int sigmaSize = 0;
            char currentSymbol = char.MaxValue;

            for (int i = 0; i < firstToLastMap.Length; ++i)
            {
                char symbol = transformedText[firstToLastMap[i]];

                if (currentSymbol == symbol)
                {
                    continue;
                }

                ++sigmaSize;
                currentSymbol = symbol;
            }

            return sigmaSize;
        }
    }
}