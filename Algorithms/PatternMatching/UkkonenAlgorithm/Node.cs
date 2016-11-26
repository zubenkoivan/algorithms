using System;
using System.Collections.Generic;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Node
    {
        private readonly Dictionary<char, Edge> edges = new Dictionary<char, Edge>();
        private Edge incomingEdge;

        public Node SuffixLink { get; private set; }

        public Node(Edge incomingEdge = null)
        {
            IncomingEdge = incomingEdge;
        }

        public Edge IncomingEdge
        {
            get { return incomingEdge; }
            set
            {
                if (incomingEdge != null && incomingEdge.End != this)
                {
                    throw new InvalidOperationException("Incoming edge does not end with this node");
                }

                incomingEdge = value;
            }
        }

        public bool IsRoot => IncomingEdge == null;

        public Edge GetEdge(char symbol)
        {
            return edges.TryGetValue(symbol, out Edge edge) ? edge : null;
        }

        public void AddEdge(Edge edge)
        {
            edges.Add(edge.StartSymbol, edge);
        }

        public void SaveSuffixLink(Node suffixLink)
        {
            if (suffixLink == null)
            {
                throw new ArgumentNullException(nameof(suffixLink));
            }

            if (SuffixLink != null && SuffixLink != suffixLink)
            {
                throw new InvalidOperationException("Suffix link cannot be changed");
            }

            SuffixLink = suffixLink;
        }

        public override string ToString()
        {
            return IsRoot ? "root" : "node";
        }
    }
}
