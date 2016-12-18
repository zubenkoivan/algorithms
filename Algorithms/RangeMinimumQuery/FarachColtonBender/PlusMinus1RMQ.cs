using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    public class PlusMinus1RMQ : RMQ
    {
        private readonly int blockSize;
        private readonly SparseTableRMQ blockSparseTable;
        private readonly int[][][] blockMinHeights;
        private readonly Block[] blocks;

        public PlusMinus1RMQ(int[] source)
        {
            blockSize = (int) Math.Round(Math.Log(source.Length, 2.0) / 2.0);
            blockSparseTable = CreateBlockSparseTable(source, blockSize);

            if (blockSize > 1)
            {
                blockMinHeights = CreateBlockMinHeights(blockSize);
                blocks = CreateBlocks(source, blockSize);
            }
        }

        private static SparseTableRMQ CreateBlockSparseTable(int[] source, int blockSize)
        {
            if (blockSize <= 1)
            {
                return new SparseTableRMQ(source);
            }

            var blockMins = new int[(source.Length - 1) / blockSize + 1];

            for (int block = 0; block < blockMins.Length; ++block)
            {
                int min = int.MaxValue;
                int leftBorder = block * blockSize;
                int blockLength = Math.Min(blockSize, source.Length - leftBorder);
                int nextLeftBorder = leftBorder + blockLength;

                for (int i = leftBorder; i < nextLeftBorder; ++i)
                {
                    min = Math.Min(min, source[i]);
                }

                blockMins[block] = min;
            }

            return new SparseTableRMQ(blockMins);
        }

        private static int[][][] CreateBlockMinHeights(int blockSize)
        {
            int maskSize = blockSize - 1;
            int mask = (int) Math.Pow(2.0, maskSize) - 1;
            var blockRMQ = new int[mask + 1][][];

            while (mask >= 0)
            {
                blockRMQ[mask] = CalculateMinHeights(mask, maskSize);
                --mask;
            }

            return blockRMQ;
        }

        private static int[][] CalculateMinHeights(int mask, int maskSize)
        {
            var result = new int[maskSize][];
            int baseHeight = 0;

            for (int i = 0; i < result.Length; ++i)
            {
                int arrayLength = maskSize - i;
                var minHeights = new int[arrayLength];
                int minHeight = int.MaxValue;
                int currentHeight = 0;

                for (int j = 0; j < arrayLength; ++j)
                {
                    int currentBit = (mask >> (arrayLength - j - 1)) & 1;
                    currentHeight += currentBit + ~(currentBit ^ 1) + 1;
                    minHeight = Math.Min(minHeight, currentHeight);
                    minHeights[j] = baseHeight + minHeight;
                }

                result[i] = minHeights;
                int firstBit = (mask >> (arrayLength - 1)) & 1;
                baseHeight += firstBit + ~(firstBit ^ 1) + 1;
            }

            return result;
        }

        private static Block[] CreateBlocks(int[] source, int blockSize)
        {
            int blocksCount = (source.Length - 1) / blockSize + 1;
            var blocks = new Block[blocksCount];
            int maskSize = blockSize - 1;

            for (int i = 0; i < blocksCount; ++i)
            {
                blocks[i] = new Block(source, i * blockSize, maskSize);
            }

            return blocks;
        }

        public override int this[int i, int j]
        {
            get
            {
                if (i > j)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                return blockSize == 1
                    ? blockSparseTable[i, j]
                    : FindRMQ(i, j);
            }
        }

        private int FindRMQ(int i, int j)
        {
            int leftMostBlock = i / blockSize;
            int blockI = i - leftMostBlock * blockSize;
            int rightMostBlock = j / blockSize;
            int blockJ = j - rightMostBlock * blockSize;

            if (leftMostBlock == rightMostBlock)
            {
                return blocks[leftMostBlock].FindMin(blockMinHeights, blockI, blockJ);
            }

            int min = blocks[leftMostBlock].FindMin(blockMinHeights, blockI, blockSize - 1);
            min = Math.Min(min, blocks[rightMostBlock].FindMin(blockMinHeights, 0, blockJ));

            if (rightMostBlock - leftMostBlock > 1)
            {
                min = Math.Min(min, blockSparseTable[leftMostBlock + 1, rightMostBlock - 1]);
            }

            return min;
        }
    }
}