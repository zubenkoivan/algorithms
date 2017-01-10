using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    internal class Blocks
    {
        private readonly int[] source;
        private readonly MaskMins maskMins;
        private readonly int[] blockMasks;

        public readonly int BlockSize;

        public Blocks(int[] source, int blockSize)
        {
            this.source = source;
            maskMins = new MaskMins(blockSize - 1);
            blockMasks = CreateBlockMasks(source, blockSize);
            BlockSize = blockSize;
        }

        private static int[] CreateBlockMasks(int[] source, int blockSize)
        {
            int blocksCount = (source.Length - 1) / blockSize + 1;
            var blocks = new int[blocksCount];
            int maskSize = blockSize - 1;

            for (int i = 0; i < blocksCount; ++i)
            {
                blocks[i] = CalculateBlockMask(source, i * blockSize, maskSize);
            }

            return blocks;
        }

        private static int CalculateBlockMask(int[] source, int blockStart, int maskSize)
        {
            int blockSize = Math.Min(maskSize + 1, source.Length - blockStart);
            int nextLeftBorder = blockStart + blockSize;
            int mask = 0;

            for (int i = blockStart + 1; i < nextLeftBorder; ++i)
            {
                mask <<= 1;
                int diff = source[i] - source[i - 1];
                mask += (diff + 1) >> 1;
            }

            int lengthDiff = maskSize - blockSize + 1;
            mask <<= lengthDiff;
            mask |= ~(-1 << lengthDiff);

            return mask;
        }

        public Minimum Min(int block, int blockI, int blockJ)
        {
            int blockMinIndex = 0;
            int blockStart = block * BlockSize;

            if (blockI != 0)
            {
                blockMinIndex = maskMins[blockMasks[block], blockI - 1, blockJ - 1] + 1;
            }
            else if (blockJ != 0)
            {
                blockMinIndex = maskMins[blockMasks[block], 0, blockJ - 1] + 1;
                blockMinIndex = source[blockStart + blockMinIndex] < source[blockStart] ? blockMinIndex : 0;
            }

            int sourceMinIndex = blockStart + blockMinIndex;

            return new Minimum(sourceMinIndex, source[sourceMinIndex]);
        }
    }
}