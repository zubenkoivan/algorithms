using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.TextProcessing.PatternMatching.AhoCorasickAlgorithm
{
    public class PatternsCollection
    {
        private readonly Trie trie;

        public PatternsCollection(params string[] patterns)
        {
            if (patterns == null || patterns.Length == 0)
            {
                throw new ArgumentException("is empty", nameof(patterns));
            }

            if (patterns.Any(string.IsNullOrEmpty))
            {
                throw new ArgumentException("one of patterns is empty", nameof(patterns));
            }

            trie = new Trie(patterns);
        }

        public IEnumerable<PatternLocation> LocationsIn(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("is empty", nameof(text));
            }

            return trie.LocationsIn(text);
        }
    }
}