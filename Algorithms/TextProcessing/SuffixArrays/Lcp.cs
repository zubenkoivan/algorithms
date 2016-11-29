using System;

namespace Algorithms.TextProcessing.SuffixArrays
{
    internal struct Lcp
    {
        public readonly int Left;
        public readonly int Right;

        public int Max => Math.Max(Left, Right);
        public int Min => Math.Min(Left, Right);

        public Lcp(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"L: {Left}, R: {Right}";
        }

        public bool ShouldGoLeft(Lcp middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? middleLcp.Left < Left
                : middleLcp.Left <= Right && Right < middleLcp.Right;
        }

        public bool ShouldGoRight(Lcp middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? middleLcp.Right <= Left && Left < middleLcp.Left
                : middleLcp.Right < Right;
        }

        public Lcp GoLeft(Lcp middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left ? new Lcp(Left, middleLcp.Left) : this;
        }

        public Lcp GoLeft(Lcp middleLcp, int lcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? new Lcp(middleLcp.Left, lcp)
                : new Lcp(Left, lcp);
        }

        public Lcp GoRight(Lcp middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left ? this : new Lcp(middleLcp.Right, Right);
        }

        public Lcp GoRight(Lcp middleLcp, int lcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? new Lcp(lcp, Right)
                : new Lcp(lcp, middleLcp.Right);
        }
    }
}