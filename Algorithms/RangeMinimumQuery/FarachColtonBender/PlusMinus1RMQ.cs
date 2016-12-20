using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    public class PlusMinus1RMQ : RMQ
    {
        private readonly int sourceLength;
        private readonly int[] sparseTableRanks;
        private readonly SparseTableRMQ sparseTable;
        private readonly Blocks blocks;

        public PlusMinus1RMQ(int[] source)
        {
            sourceLength = source.Length;
            int blockSize = (int) Math.Round(Math.Log(source.Length, 2.0) / 2.0);

            if (blockSize > 1)
            {
                sparseTable = CreateSparseTable(source, blockSize, out sparseTableRanks);
                blocks = new Blocks(source, blockSize);
            }
            else
            {
                sparseTable = new SparseTableRMQ(source);
            }
        }

        private static SparseTableRMQ CreateSparseTable(int[] source, int blockSize, out int[] ranks)
        {
            var blockMins = new int[(source.Length - 1) / blockSize + 1];
            ranks = new int[blockMins.Length];

            for (int block = 0; block < blockMins.Length; ++block)
            {
                int blockStart = block * blockSize;
                int nextBlockStart = blockStart + Math.Min(blockSize, source.Length - blockStart);
                int min = source[blockStart];
                int minIndex = blockStart;

                for (int i = blockStart; i < nextBlockStart; ++i)
                {
                    if (source[i] < min)
                    {
                        min = Math.Min(min, source[i]);
                        minIndex = i;
                    }
                }

                blockMins[block] = min;
                ranks[block] = minIndex;
            }

            return new SparseTableRMQ(blockMins);
        }

        public override Minimum this[int i, int j]
        {
            get
            {
                if (i > j || j >= sourceLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                return blocks == null ? sparseTable[i, j] : FindMin(i, j);
            }
        }

        public Minimum FindMin(int i, int j)
        {
            int leftMostBlock = i / blocks.BlockSize;
            int rightMostBlock = j / blocks.BlockSize;
            int blockI = i - leftMostBlock * blocks.BlockSize;
            int blockJ = j - rightMostBlock * blocks.BlockSize;

            if (leftMostBlock == rightMostBlock)
            {
                return blocks.Min(leftMostBlock, blockI, blockJ);
            }

            Minimum minPrefix = blocks.Min(leftMostBlock, blockI, blocks.BlockSize - 1);
            Minimum min = minPrefix;

            if (rightMostBlock - leftMostBlock > 1)
            {
                Minimum minBody = sparseTable[leftMostBlock + 1, rightMostBlock - 1];
                min = minBody.Value < min.Value
                    ? new Minimum(sparseTableRanks[minBody.Index], minBody.Value)
                    : min;
            }

            Minimum minSuffix = blocks.Min(rightMostBlock, 0, blockJ);

            return Minimum.Min(min, minSuffix);
        }
    }
}