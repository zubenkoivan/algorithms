using System.Collections.Generic;

namespace Algorithms.TextProcessing.PatternMatching.KnuthMorrisPrattAlgorithm
{
    public class PrefixFunction
    {
        private readonly string pattern;
        private readonly int patternLength;
        private readonly int[] prefixFunction;

        public PrefixFunction(string pattern)
        {
            this.pattern = pattern;
            patternLength = pattern.Length;
            prefixFunction = CreatePrefixFunction(pattern);
        }

        private static int[] CreatePrefixFunction(string pattern)
        {
            var prefixFunction = new int[pattern.Length];

            for (int i = 1; i < pattern.Length; ++i)
            {
                for (int k = i - 1; k > 0; k = prefixFunction[k] - 1)
                {
                    if (pattern[prefixFunction[k]] == pattern[i])
                    {
                        prefixFunction[i] = prefixFunction[k] + 1;
                        break;
                    }
                }
            }

            return prefixFunction;
        }

        public IEnumerable<int> IndexesIn(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < patternLength)
            {
                yield break;
            }

            int patternPrefixLength = 0;

            for (int textIndex = 0; textIndex < text.Length; ++textIndex)
            {
                if (patternPrefixLength < patternLength && pattern[patternPrefixLength] == text[textIndex])
                {
                    patternPrefixLength += 1;
                }
                else
                {
                    patternPrefixLength = FindPrefix(text[textIndex], patternPrefixLength);
                }

                if (patternPrefixLength == patternLength)
                {
                    yield return textIndex - patternLength + 1;
                }
            }
        }

        private int FindPrefix(char nextTextSymbol, int maxPrefixLength)
        {
            for (int k = maxPrefixLength - 1; k >= 0; k = prefixFunction[k] - 1)
            {
                if (pattern[prefixFunction[k]] == nextTextSymbol)
                {
                    return prefixFunction[k] + 1;
                }
            }

            return 0;
        }
    }
}