using System;

namespace Algorithms.TextProcessing.SuffixTrees.UkkonenAlgorithm
{
    internal class Edge
    {
        private readonly string text;

        public Node Start { get; }
        public int StartIndex { get; }

        public int EndIndex { get; private set; }
        public Node End { private set; get; }

        public Edge(string text, Node start, int startIndex)
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
            EndIndex = text.Length - 1;
        }

        public char StartSymbol => text[StartIndex];

        public int Length => EndIndex - StartIndex + 1;

        public Edge NextEdge(char symbol) => End?.GetEdge(symbol);

        public bool HasSymbol(int activeLength, char symbol)
        {
            if (activeLength < 0 || activeLength >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(activeLength));
            }

            return text[StartIndex + activeLength] == symbol;
        }

        public Node Split(int edgeLength)
        {
            if (edgeLength <= 0 || edgeLength >= Length)
            {
                throw new ArgumentOutOfRangeException(nameof(edgeLength));
            }

            var newNode = new Node(this);
            var newEdge = new Edge(text, newNode, StartIndex + edgeLength)
            {
                End = End,
                EndIndex = EndIndex
            };
            newNode.AddEdge(newEdge);

            if (End != null)
            {
                End.IncomingEdge = newEdge;
            }

            End = newEdge.Start;
            EndIndex = newEdge.StartIndex - 1;

            return newNode;
        }

        public override string ToString()
        {
            if (Length == 1)
            {
                return StartSymbol.ToString();
            }

            return Length <= 50
                ? $"{Length}: '{text.Substring(StartIndex, Length)}'"
                : $"{Length}: '{text.Substring(StartIndex, 25)}...{text.Substring(text.Length - 25)}'";
        }
    }
}