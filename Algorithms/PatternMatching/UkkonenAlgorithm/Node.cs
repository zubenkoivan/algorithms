using System;
using System.Collections.Generic;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Node
    {
        private readonly string text;
        private readonly Dictionary<char, Edge> edges = new Dictionary<char, Edge>();
        public Node SuffixLink;

        public Node(string text)
        {
            this.text = text;
            outgoingEdge = new Edge(text, 0);
        }
    }
}
