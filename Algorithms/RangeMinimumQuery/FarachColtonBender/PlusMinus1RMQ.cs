using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    public class PlusMinus1RMQ : RMQ
    {
        private readonly int[][][] blockMinIndexes;

        private readonly int[] source;
        private readonly int blockSize;
        private readonly int[] blockMinRanks;
        private readonly SparseTableRMQ blockSparseTable;
        private readonly Block[] blocks;

        public PlusMinus1RMQ(int[] source)
        {
            this.source = source;
            blockSize = (int) Math.Round(Math.Log(source.Length, 2.0) / 2.0);
            blockSparseTable = CreateBlockSparseTable(source, blockSize, out blockMinRanks);

            if (blockSize > 1)
            {
                blockMinIndexes = CreateBlockMinIndexes(blockSize);
                blocks = CreateBlocks(source, blockSize);
            }
        }

        private static SparseTableRMQ CreateBlockSparseTable(int[] source, int blockSize, out int[] blockMinRanks)
        {
            if (blockSize <= 1)
            {
                blockMinRanks = null;
                return new SparseTableRMQ(source);
            }

            var blockMins = new int[(source.Length - 1) / blockSize + 1];
            blockMinRanks = new int[blockMins.Length];

            for (int block = 0; block < blockMins.Length; ++block)
            {
                int blockStart = block * blockSize;
                int min = source[blockStart];
                int minIndex = blockStart;
                int blockLength = Math.Min(blockSize, source.Length - blockStart);
                int nextLeftBorder = blockStart + blockLength;

                for (int i = blockStart; i < nextLeftBorder; ++i)
                {
                    if (source[i] < min)
                    {
                        min = Math.Min(min, source[i]);
                        minIndex = i;
                    }
                }

                blockMins[block] = min;
                blockMinRanks[block] = minIndex;
            }

            return new SparseTableRMQ(blockMins);
        }

        private static int[][][] CreateBlockMinIndexes(int blockSize)
        {
            int maskSize = blockSize - 1;
            int mask = (int) Math.Pow(2.0, maskSize) - 1;
            var blockRMQ = new int[mask + 1][][];

            while (mask >= 0)
            {
                blockRMQ[mask] = CalculateMinIndexes(mask, maskSize);
                --mask;
            }

            return blockRMQ;
        }

        private static int[][] CalculateMinIndexes(int mask, int maskSize)
        {
            var result = new int[maskSize][];

            for (int i = 0; i < result.Length; ++i)
            {
                int arrayLength = maskSize - i;
                var minIndexes = new int[arrayLength];
                int minHeight = int.MaxValue;
                int minIndex = int.MaxValue;
                int currentHeight = 0;

                for (int j = 0; j < arrayLength; ++j)
                {
                    int currentBit = (mask >> (arrayLength - j - 1)) & 1;
                    currentHeight += currentBit + ~(currentBit ^ 1) + 1;

                    if (currentHeight < minHeight)
                    {
                        minHeight = currentHeight;
                        minIndex = i + j + 1;
                    }

                    minIndexes[j] = minIndex;
                }

                result[i] = minIndexes;
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

        public override Minimum this[int i, int j]
        {
            get
            {
                if (i > j || j >= source.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                return blockSize == 1 ? blockSparseTable[i, j] : FindMin(i, j);
            }
        }

        public Minimum FindMin(int i, int j)
        {
            int leftMostBlock = i / blockSize;
            int rightMostBlock = j / blockSize;

            if (leftMostBlock == rightMostBlock)
            {
                return blocks[leftMostBlock].FindMin(source, blockMinIndexes, i, j);
            }

            Minimum minPrefix = blocks[leftMostBlock].FindMin(source, blockMinIndexes, i, (leftMostBlock + 1) * blockSize - 1);
            Minimum min = minPrefix;

            if (rightMostBlock - leftMostBlock > 1)
            {
                Minimum minBody = blockSparseTable[leftMostBlock + 1, rightMostBlock - 1];

                min = minBody.Value < min.Value
                    ? new Minimum(blockMinRanks[minBody.Index], minBody.Value)
                    : min;
            }

            Minimum minSuffix = blocks[rightMostBlock].FindMin(source, blockMinIndexes, rightMostBlock * blockSize, j);

            return Minimum.Min(min, minSuffix);
        }
    }
}