namespace Algorithms.TextProcessing.PatternMatching.KnuthMorrisPrattAlgorithm
{
    internal class PrefixFunction
    {
        private readonly string pattern;
        private readonly int[] prefixFunction;

        public PrefixFunction(string pattern)
        {
            this.pattern = pattern;
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

        public int FindPrefix(char nextTextSymbol, int maxPrefixLength)
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