using System;

namespace Algorithms.TextProcessing.SuffixArrays
{
    internal struct LCP
    {
        public readonly int Left;
        public readonly int Right;

        public int Max => Math.Max(Left, Right);
        public int Min => Math.Min(Left, Right);

        public LCP(int left, int right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"L: {Left}, R: {Right}";
        }

        public bool ShouldGoLeft(LCP middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? middleLcp.Left < Left
                : middleLcp.Left <= Right && Right < middleLcp.Right;
        }

        public bool ShouldGoRight(LCP middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? middleLcp.Right <= Left && Left < middleLcp.Left
                : middleLcp.Right < Right;
        }

        public LCP GoLeft(LCP middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left ? new LCP(Left, middleLcp.Left) : this;
        }

        public LCP GoLeft(LCP middleLcp, int lcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? new LCP(middleLcp.Left, lcp)
                : new LCP(Left, lcp);
        }

        public LCP GoRight(LCP middleLcp)
        {
            return middleLcp.Right <= middleLcp.Left ? this : new LCP(middleLcp.Right, Right);
        }

        public LCP GoRight(LCP middleLcp, int lcp)
        {
            return middleLcp.Right <= middleLcp.Left
                ? new LCP(lcp, Right)
                : new LCP(lcp, middleLcp.Right);
        }
    }
}