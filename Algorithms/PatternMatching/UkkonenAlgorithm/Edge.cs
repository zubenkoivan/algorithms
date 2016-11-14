using System;

namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class Edge
    {
        private readonly string text;

        public int StartIndex { get; }
        public int EndIndex { get; private set; }
        public Node Start { get; }
        public Node End { get; private set; }

        public Edge(Node start, string text, int startIndex)
        {
            if (start == null)
            {
                throw new ArgumentNullException(nameof(start));
            }

            if (startIndex < 0 || startIndex >= text.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            this.text = text;
            StartIndex = startIndex;
            Start = start;
            EndIndex = -1;
        }

        public override string ToString()
        {
            if (Length == 1)
            {
                return StartSymbol.ToString();
            }

            return StartSymbol + "->" + (EndIndex == -1 ? '#' : text[EndIndex]);
        }

        public char StartSymbol => text[StartIndex];

        public int Length => EndIndex == -1
            ? int.MaxValue
            : EndIndex - StartIndex + 1;

        public bool HasSymbol(int activeLength, char symbol)
        {
            if (activeLength <= 0 || activeLength >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLength));
            }

            return text[StartIndex + activeLength] == symbol;
        }

        public Edge NextEdge(char symbol)
        {
            return End.GetEdge(symbol);
        }

        public Node Split(int firstEdgeLength)
        {
            if (firstEdgeLength <= 0 || firstEdgeLength >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(firstEdgeLength));
            }

            var newNode = new Node(this);
            var edge = new Edge(newNode, text, StartIndex + firstEdgeLength)
            {
                End = End,
                EndIndex = EndIndex
            };

            if (End != null)
            {
                End.IncomingEdge = edge;
            }

            End = edge.Start;
            EndIndex = edge.StartIndex - 1;

            return newNode;
        }
    }
}