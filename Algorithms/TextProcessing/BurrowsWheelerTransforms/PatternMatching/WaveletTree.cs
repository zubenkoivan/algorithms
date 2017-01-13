namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class WaveletTree
    {
        private readonly char[] sigma;
        private readonly TreeNode root;

        public WaveletTree(string text, SigmaRanges sigmaRanges, char[] sigma)
        {
            this.sigma = sigma;
            root = new TreeNode(text, sigmaRanges, sigma);
        }

        public int Rank(char symbol, int prefixLength)
        {
            int start = 0;
            int end = sigma.Length - 1;
            TreeNode currentNode = root;

            while (true)
            {
                int middle = (start + end) / 2;
                int binarySymbol = symbol > sigma[middle] ? 1 : 0;
                int binarySymbolRank = currentNode.Rank(binarySymbol, prefixLength);

                prefixLength = binarySymbolRank;

                if (prefixLength == 0)
                {
                    return 0;
                }

                if (binarySymbol == 0 && currentNode.LeftChild != null)
                {
                    currentNode = currentNode.LeftChild;
                    end = middle;
                }
                else if (binarySymbol == 1 && currentNode.RightChild != null)
                {
                    currentNode = currentNode.RightChild;
                    start = middle + 1;
                }
                else
                {
                    return binarySymbolRank;
                }
            }
        }
    }
}