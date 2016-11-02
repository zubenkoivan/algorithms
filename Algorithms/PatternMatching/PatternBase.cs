using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.PatternMatching
{
    public abstract class PatternBase
    {
        protected readonly string Pattern;
        protected readonly int PatternLength;

        protected PatternBase(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("is empty", nameof(pattern));
            }

            Pattern = pattern;
            PatternLength = pattern.Length;
        }

        protected abstract IEnumerable<int> IndexesInImpl(string text);

        public IEnumerable<int> IndexesIn(string text)
        {
            if (string.IsNullOrEmpty(text) || text.Length < PatternLength)
            {
                return Enumerable.Empty<int>();
            }

            return IndexesInImpl(text);
        }
    }
}