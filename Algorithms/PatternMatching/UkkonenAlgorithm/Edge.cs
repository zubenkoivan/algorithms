using System;
using System.Collections.Generic;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Edge
    {
        private readonly Node start;
        private readonly Node end;
        private readonly int startIndex;
        private readonly int endIndex;
        private readonly string text;

        public Edge(string text, int startIndex)
        {
            if (startIndex < 0 || startIndex >= text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            this.text = text;
            this.startIndex = startIndex;
            endIndex = int.MaxValue;
        }

        public bool HasEnd => endIndex != -1;

        public int Length()
        {
            return startIndex == -1
                ? int.MaxValue
                : endIndex - startIndex + 1;
        }

        public int Length(int textIndex)
        {
            return textIndex - startIndex + 1;
        }

        public bool Contains(int textIndex)
        {
            return startIndex <= textIndex && textIndex <= endIndex;
        }

        public bool StartsFrom(int textIndex)
        {
            return startIndex == textIndex;
        }

        public TreeVertice GetVerticeWith(int textIndex)
        {
            TreeVertice result = null;

            while (true)
            {
                if (textIndex <= endIndex)
                {
                    return result;
                }

                result = nextNodes[text[endIndex + 1]];
            }
        }
    }
}