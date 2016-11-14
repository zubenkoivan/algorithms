using System;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Vertice
    {
        private readonly Node node;
        private readonly int index;

        public Vertice(Node node, int index)
        {
            this.node = node;
            this.index = index;
        }

        public bool IsLeaf => !node.HasNodesNext;

        public Vertice SuffixLink
        {
            get
            {
                return new Vertice(node.SuffixLink);
            }
        }

        public Vertice? GetNext(char symbol)
        { }

        public void AddExplicit(char symbol, int symbolIndex)
        { }
    }
}
