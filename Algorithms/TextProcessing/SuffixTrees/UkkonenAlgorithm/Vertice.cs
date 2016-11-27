using System;

namespace Algorithms.TextProcessing.SuffixTrees.UkkonenAlgorithm
{
    internal class Vertice
    {
        private readonly Node startNode;
        private readonly Edge edge;
        private readonly int activeLength;

        public Vertice(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            startNode = node;
        }

        public Vertice(Edge edge, int activeLength)
        {
            if (activeLength < 0 || activeLength > edge.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLength));
            }

            startNode = activeLength == edge.Length ? edge.End : edge.Start;

            if (activeLength != edge.Length && activeLength != 0)
            {
                this.edge = edge;
                this.activeLength = activeLength;
            }
        }

        public bool IsExplicitVertice => activeLength == 0;

        public Node ExplicitVertice => IsExplicitVertice ? startNode : null;

        public bool IsLeaf(int currentPrefixLength) =>
            !IsExplicitVertice
            && edge.End == null
            && (edge.StartIndex + activeLength >= currentPrefixLength);

        public Vertice GetNext(char symbol)
        {
            if (!IsExplicitVertice)
            {
                return edge.HasSymbol(activeLength, symbol)
                    ? new Vertice(edge, activeLength + 1)
                    : null;
            }

            Edge nextEdge = startNode.GetEdge(symbol);
            return nextEdge == null ? null : new Vertice(nextEdge, 1);
        }

        public Node AddExplicitVertice(string text, int symbolIndex)
        {
            Node node = ExplicitVertice ?? edge.Split(activeLength);
            node.AddEdge(new Edge(text, node, symbolIndex));
            return node;
        }

        public Vertice SuffixLink(string text)
        {
            if (IsExplicitVertice)
            {
                return new Vertice(startNode.SuffixLink);
            }

            if (startNode.IsRoot && activeLength == 1)
            {
                return new Vertice(startNode);
            }

            int linkActiveLength = edge.Start.IsRoot ? activeLength - 1 : activeLength;
            char startSymbol = edge.Start.IsRoot ? text[edge.StartIndex + 1] : edge.StartSymbol;
            Edge currentEdge = edge.Start.SuffixLink.GetEdge(startSymbol);

            while (currentEdge.Length < linkActiveLength)
            {
                linkActiveLength -= currentEdge.Length;
                startSymbol = text[edge.StartIndex + activeLength - linkActiveLength];
                currentEdge = currentEdge.NextEdge(startSymbol);
            }

            return new Vertice(currentEdge, linkActiveLength);
        }

        public override string ToString()
        {
            return ExplicitVertice?.ToString() ?? $"edge: {edge}, activeLength: {activeLength}";
        }
    }
}
