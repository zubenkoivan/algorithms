namespace Algorithms.TextProcessing.SuffixArrays
{
    internal struct LcpNode
    {
        public readonly int LeftChild;
        public readonly int Lcp;

        public LcpNode(int leftChild, int lcp)
        {
            LeftChild = leftChild;
            Lcp = lcp;
        }

        public LcpNode(int lcp)
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