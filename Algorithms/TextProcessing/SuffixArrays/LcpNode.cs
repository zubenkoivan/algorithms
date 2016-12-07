namespace Algorithms.TextProcessing.SuffixArrays
{
    internal struct LCPNode
    {
        public readonly int LeftChild;
        public readonly int Lcp;

        public LCPNode(int leftChild, int lcp)
        {
            LeftChild = leftChild;
            Lcp = lcp;
        }

        public LCPNode(int lcp)
        {
            LeftChild = -1;
            Lcp = lcp;
        }

        public override string ToString()
        {
            return $"LCP: {Lcp}, Left child: {LeftChild}";
        }
    }
}