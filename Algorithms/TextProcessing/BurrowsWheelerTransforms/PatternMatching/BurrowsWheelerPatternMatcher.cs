using Algorithms.Sorting;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    public class BurrowsWheelerPatternMatcher
    {
        private readonly SigmaRanges sigmaRanges;
        private readonly WaveletTree waveletTree;

        public BurrowsWheelerPatternMatcher(string transformedText)
        {
            int[] firstToLastMap = RestoreFirstToLastMap(transformedText);
            sigmaRanges = new SigmaRanges(transformedText, firstToLastMap);
            char[] sigma = GetSigma(transformedText, firstToLastMap, sigmaRanges.SigmaSize);
            waveletTree = new WaveletTree(transformedText, sigmaRanges, sigma);
        }

        private static int[] RestoreFirstToLastMap(string transformedText)
        {
            var map = new int[transformedText.Length];

            for (int i = 0; i < map.Length; ++i)
            {
                map[i] = i;
            }

            RadixSort.Sort(map, i => transformedText[i]);

            return map;
        }

        private static char[] GetSigma(string transformedText, int[] firstToLastMap, int sigmaSize)
        {
            var sigma = new char[sigmaSize];
            int sigmaIndex = 0;
            char currentSymbol = char.MaxValue;

            for (int i = 0; i < firstToLastMap.Length; ++i)
            {
                char symbol = transformedText[firstToLastMap[i]];

                if (currentSymbol == symbol)
                {
                    continue;
                }

                currentSymbol = symbol;
                sigma[sigmaIndex] = symbol;
                ++sigmaIndex;
            }

            return sigma;
        }

        public int PatternCount(string pattern)
        {
            Range range = sigmaRanges[pattern[pattern.Length - 1]];

            for (int i = pattern.Length - 2; i >= 0; --i)
            {
                char symbol = pattern[i];
                int skip = waveletTree.Rank(symbol, range.Start);
                int count = waveletTree.Rank(symbol, range.Start + range.Length) - skip;

                if (count == 0)
                {
                    return 0;
                }

                range = sigmaRanges[symbol];
                range = range.Reduce(skip, count);
            }

            return range.Length;
        }
    }
}