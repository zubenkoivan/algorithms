using System;

namespace Algorithms.TextProcessing.SuffixArrays
{
    internal class LcpTree
    {
        private readonly int[] lcpArray;
        private readonly LcpNode[] nodes;

        public LcpNode Root => nodes[0];

        public LcpTree(int[] lcpArray)
        {
            this.lcpArray = lcpArray;
            nodes = CreateTree();
        }

        private LcpNode[] CreateTree()
        {
            var lcpTree = new LcpNode[lcpArray.Length];
            var range = new Range(0, lcpArray.Length);
            int treeIndex = 1;

            lcpTree[0] = CreateNode(lcpTree, range, ref treeIndex);

            return lcpTree;
        }

        private LcpNode CreateNode(LcpNode[] lcpTree, Range range, ref int treeIndex)
        {
            if (range.Length == 3)
            {
                return new LcpNode(Math.Min(lcpArray[range.Start], lcpArray[range.Right.Start]));
            }

            int i = treeIndex;

            if (range.Length == 4)
            {
                ++treeIndex;
                lcpTree[i] = CreateNode(lcpTree, range.Right, ref treeIndex);

                return new LcpNode(i, Math.Min(lcpArray[range.Start], lcpTree[i].Lcp));
            }

            treeIndex += 2;

            lcpTree[i] = CreateNode(lcpTree, range.Left, ref treeIndex);
            lcpTree[i + 1] = CreateNode(lcpTree, range.Right, ref treeIndex);

            return new LcpNode(i, Math.Min(lcpTree[i].Lcp, lcpTree[i + 1].Lcp));
        }

        public Lcp Lcp(LcpNode currentNode, Range currentRange)
        {
            if (currentRange.Length == 2)
            {
                throw new InvalidOperationException("range is too small");
            }

            if (currentRange.Right.Length == 2)
            {
                return new Lcp(lcpArray[currentRange.Start], lcpArray[currentRange.Right.Start]);
            }

            return currentRange.Left.Length == 2
                ? new Lcp(lcpArray[currentRange.Start], nodes[currentNode.LeftChild].Lcp)
                : new Lcp(nodes[currentNode.LeftChild].Lcp, nodes[currentNode.LeftChild + 1].Lcp);
        }

        public LcpNode GoLeft(LcpNode currentNode, Range currentRange)
        {
            return currentRange.Left.Length == 2
                ? new LcpNode(lcpArray[currentRange.Start])
                : nodes[currentNode.LeftChild];
        }

        public LcpNode GoRight(LcpNode currentNode, Range currentRange)
        {
            if (currentRange.Right.Length == 2)
            {
                return new LcpNode(lcpArray[currentRange.Right.Start]);
            }

            return currentRange.Left.Length == 2
                ? nodes[currentNode.LeftChild]
                : nodes[currentNode.LeftChild + 1];
        }
    }
}