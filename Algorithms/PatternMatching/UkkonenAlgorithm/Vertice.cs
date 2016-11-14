using System;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Vertice
    {
        private readonly Node startNode;
        private readonly Edge edge;
        private readonly int activeLength;

        public Vertice(Node node)
        {
            startNode = node;

            if (node.IsRoot)
            {
                return;
            }

            edge = node.IncomingEdge;
            activeLength = node.IncomingEdge.Length;
        }

        public override string ToString()
        {
            if (IsRoot)
            {
                return "root";
            }

            return $"Edge: {edge}, active: {activeLength}";
        }

        public Vertice(Edge edge, int activeLength)
        {
            if (edge == null)
            {
                throw new NullReferenceException(nameof(edge));
            }

            if (activeLength < 0 || activeLength > edge.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLength));
            }

            startNode = edge.Start;
            this.edge = edge;
            this.activeLength = activeLength;
        }

        public bool IsRoot => activeLength == 0 && startNode.IsRoot;

        public bool IsLeaf(int currentPrefixLength) =>
            !IsRoot
            && edge.End == null
            && (edge.StartIndex + activeLength >= currentPrefixLength);

        public Node ExplicitNode
        {
            get
            {
                if (activeLength == 0)
                {
                    return startNode;
                }

                return activeLength == edge.Length ? edge.End : null;
            }
        }

        public Vertice GetNext(char symbol)
        {
            if (activeLength == 0)
            {
                Edge nextEdge = startNode.GetEdge(symbol);
                return nextEdge == null ? null : new Vertice(nextEdge, 1);
            }

            if (activeLength == edge.Length)
            {
                Edge nextEdge = edge.NextEdge(symbol);
                return nextEdge == null ? null : new Vertice(nextEdge, 1);
            }

            return edge.HasSymbol(activeLength, symbol) ? new Vertice(edge, activeLength + 1) : null;
        }

        public Node AddExplicitly(string text, int symbolIndex)
        {
            Node node = ExplicitNode ?? edge.Split(activeLength);

            node.AddEdge(new Edge(node, text, symbolIndex));
            return node;
        }

        public Vertice SuffixLink(string text)
        {
            if (activeLength == 0)
            {
                return new Vertice(startNode.SuffixLink);
            }

            if (activeLength == edge.Length)
            {
                return new Vertice(edge.End.SuffixLink);
            }

            return CalculateSuffixLink(text);
        }

        private Vertice CalculateSuffixLink(string text)
        {
            if (startNode.IsRoot && activeLength == 1)
            {
                return new Vertice(edge, 0);
            }

            int linkActiveLength = activeLength;
            char startSymbol = edge.Start.IsRoot ? text[1] : edge.StartSymbol;
            Node currentNode = edge.Start.SuffixLink;
            Edge currentEdge = currentNode.GetEdge(startSymbol);

            while (currentEdge.Length < linkActiveLength)
            {
                linkActiveLength -= currentEdge.Length;
                startSymbol = text[currentEdge.EndIndex + 1];
                currentEdge = currentEdge.NextEdge(startSymbol);
            }

            return new Vertice(currentEdge, linkActiveLength);
        }
    }
}
