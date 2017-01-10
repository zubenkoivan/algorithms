using System;
using System.Text;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class TreeNode
    {
        private readonly BinaryRank binaryRank;

        public readonly TreeNode LeftChild;
        public readonly TreeNode RightChild;

        public TreeNode(string text, SigmaRanges sigmaRanges, char[] sigma)
            : this(text, sigmaRanges, sigma, 0, sigma.Length - 1)
        {
        }

        private TreeNode(string text, SigmaRanges sigmaRanges, char[] sigma, int start, int end)
        {
            int middle = (start + end) / 2;
            char sigmaMiddle = sigma[middle];
            binaryRank = new BinaryRank(text, sigmaMiddle);

            int length = end - start + 1;

            if (length == 2)
            {
                return;
            }

            int leftTextSize = TextSize(sigmaRanges, sigma, start, middle);
            string leftText = ReduceText(text, leftTextSize, x => x <= sigmaMiddle);

            LeftChild = new TreeNode(leftText, sigmaRanges, sigma, start, middle);

            if (length == 3)
            {
                return;
            }

            int rightTextSize = text.Length - leftTextSize;
            string rightText = ReduceText(text, rightTextSize, x => x > sigmaMiddle);

            RightChild = new TreeNode(rightText, sigmaRanges, sigma, middle + 1, end);
        }

        private static int TextSize(SigmaRanges sigmaRanges, char[] sigma, int start, int end)
        {
            int size = 0;

            for (int i = start; i <= end; ++i)
            {
                size += sigmaRanges[sigma[i]].Length;
            }

            return size;
        }

        private static string ReduceText(string text, int newTextSize, Predicate<char> predicate)
        {
            var textBuilder = new StringBuilder(newTextSize);

            for (int i = 0; i < text.Length; ++i)
            {
                char symbol = text[i];

                if (predicate(symbol))
                {
                    textBuilder.Append(symbol);
                }
            }

            return textBuilder.ToString();
        }

        public int Rank(int binarySymbol, int prefixLength)
        {
            return binaryRank.Rank(binarySymbol, prefixLength);
        }
    }
}