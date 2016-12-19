﻿using System;
using Algorithms.RangeMinimumQuery.FarachColtonBender;
using Algorithms.Trees.Cartesian;

namespace Algorithms.RangeMinimumQuery.RMQToLCA
{
    public class ComplexRMQ : RMQ
    {
        private readonly int[] source;
        private readonly int[] firstIndexes;
        private readonly int[] toSourceRanks;
        private readonly PlusMinus1RMQ rmq;

        public ComplexRMQ(int[] source)
        {
            this.source = source;
            rmq = new PlusMinus1RMQ(CreateEulerTour(source, out firstIndexes, out toSourceRanks));
        }

        private static int[] CreateEulerTour(int[] source, out int[] firstIndexes, out int[] toSourceRanks)
        {
            CartesianNode[] tree = CartesianTree.Create(source, out int root);
            var eulerTour = new int[source.Length * 2 - 1];
            int currentNode = root;
            int lastVisitedNode = -1;
            int level = 0;

            firstIndexes = InitFirstIndexes(source.Length);
            toSourceRanks = new int[eulerTour.Length];

            for (int i = 0; i < eulerTour.Length; ++i)
            {
                CartesianNode treeNode = tree[currentNode];
                int firstIndex = firstIndexes[currentNode];
                int nextNode = -1;

                firstIndexes[currentNode] = firstIndex == -1 ? i : firstIndex;
                eulerTour[i] = level;
                toSourceRanks[i] = currentNode;

                if (treeNode.Left != lastVisitedNode && treeNode.Right != lastVisitedNode && treeNode.Left != -1)
                {
                    nextNode = treeNode.Left;
                    ++level;
                }
                else if (treeNode.Right != lastVisitedNode && treeNode.Right != -1)
                {
                    nextNode = treeNode.Right;
                    ++level;
                }

                lastVisitedNode = currentNode;
                currentNode = nextNode;

                if (currentNode == -1)
                {
                    currentNode = treeNode.Parent;
                    --level;
                }
            }

            return eulerTour;
        }

        private static int[] InitFirstIndexes(int length)
        {
            var firstIndexes = new int[length];

            for (int i = 0; i < firstIndexes.Length; ++i)
            {
                firstIndexes[i] = -1;
            }

            return firstIndexes;
        }

        public override Minimum this[int i, int j]
        {
            get
            {
                if (i > j || j >= source.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                int firstI = firstIndexes[i];
                int firstJ = firstIndexes[j];
                Minimum min = firstI < firstJ ? rmq[firstI, firstJ] : rmq[firstJ, firstI];
                int sourceIndex = toSourceRanks[min.Index];

                return new Minimum(sourceIndex, source[sourceIndex]);
            }
        }
    }
}