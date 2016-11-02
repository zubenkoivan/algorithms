using System.Collections.Generic;

namespace Algorithms.PatternMatching.KnuthMorrisPrattAlgorithm
{
    public sealed class Pattern : PatternBase
    {
        private readonly PrefixFunction prefixFunction;

        public Pattern(string pattern) : base(pattern)
        {
            prefixFunction = new PrefixFunction(pattern);
        }

        protected override IEnumerable<int> IndexesInImpl(string text)
        {
            int patternPrefixLength = 0;

            for (int textIndex = 0; textIndex < text.Length; ++textIndex)
            {
                if (patternPrefixLength < PatternLength && Pattern[patternPrefixLength] == text[textIndex])
                {
                    patternPrefixLength += 1;
                }
                else
                {
                    patternPrefixLength = prefixFunction.FindPrefix(text[textIndex], patternPrefixLength);
                }

                if (patternPrefixLength == PatternLength)
                {
                    yield return textIndex - PatternLength + 1;
                }
            }
        }
    }
}