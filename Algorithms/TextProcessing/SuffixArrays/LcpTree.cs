using System;

namespace Algorithms.TextProcessing.SuffixArrays
{
    internal class LCPTree
    {
        private readonly int[] lcpArray;
        private readonly LCPNode[] nodes;

        public LCPNode Root => nodes[0];

        public LCPTree(int[] lcpArray)
        {
            this.lcpArray = lcpArray;
            nodes = CreateTree();
        }

        private LCPNode[] CreateTree()
        {
            var lcpTree = new LCPNode[lcpArray.Length];
            var range = new Range(0, lcpArray.Length);
            int treeIndex = 1;

            lcpTree[0] = CreateNode(lcpTree, range, ref treeIndex);

            return lcpTree;
        }

        private LCPNode CreateNode(LCPNode[] lcpTree, Range range, ref int treeIndex)
        {
            if (range.Length == 3)
            {
                return new LCPNode(Math.Min(lcpArray[range.Start], lcpArray[range.Right.Start]));
            }

            int i = treeIndex;

            if (range.Length == 4)
            {
                ++treeIndex;
                lcpTree[i] = CreateNode(lcpTree, range.Right, ref treeIndex);

                return new LCPNode(i, Math.Min(lcpArray[range.Start], lcpTree[i].Lcp));
            }

            treeIndex += 2;

            lcpTree[i] = CreateNode(lcpTree, range.Left, ref treeIndex);
            lcpTree[i + 1] = CreateNode(lcpTree, range.Right, ref treeIndex);

            return new LCPNode(i, Math.Min(lcpTree[i].Lcp, lcpTree[i + 1].Lcp));
        }

        public LCP Lcp(LCPNode currentNode, Range currentRange)
        {
            if (currentRange.Length == 2)
            {
                throw new InvalidOperationException("range is too small");
            }

            if (currentRange.Right.Length == 2)
            {
                return new LCP(lcpArray[currentRange.Start], lcpArray[currentRange.Right.Start]);
            }

            return currentRange.Left.Length == 2
                ? new LCP(lcpArray[currentRange.Start], nodes[currentNode.LeftChild].Lcp)
                : new LCP(nodes[currentNode.LeftChild].Lcp, nodes[currentNode.LeftChild + 1].Lcp);
        }

        public LCPNode GoLeft(LCPNode currentNode, Range currentRange)
        {
            return currentRange.Left.Length == 2
                ? new LCPNode(lcpArray[currentRange.Start])
                : nodes[currentNode.LeftChild];
        }

        public LCPNode GoRight(LCPNode currentNode, Range currentRange)
        {
            if (currentRange.Right.Length == 2)
            {
                return new LCPNode(lcpArray[currentRange.Right.Start]);
            }

            return currentRange.Left.Length == 2
                ? nodes[currentNode.LeftChild]
                : nodes[currentNode.LeftChild + 1];
        }
    }
}