using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    internal struct Block
    {
        private readonly int first;
        private readonly int mask;

        public Block(int[] source, int firstIndex, int maskSize)
        {
            first = source[firstIndex];

            int blockSize = Math.Min(maskSize + 1, source.Length - firstIndex);
            int nextLeftBorder = firstIndex + blockSize;
            mask = 0;

            for (int i = firstIndex + 1; i < nextLeftBorder; ++i)
            {
                mask <<= 1;
                int diff = source[i] - source[i - 1];
                mask += (diff + 1) >> 1;
            }

            int lengthDiff = maskSize - blockSize + 1;
            mask <<= lengthDiff;
            mask |= ~(-1 >> lengthDiff);
        }

        public int FindMin(int[][][] blockMinHeights, int i, int j)
        {
            if (i == 0)
            {
                return j == 0
                    ? first
                    : Math.Min(first, first + blockMinHeights[mask][0][j - 1]);
            }

            return first + blockMinHeights[mask][i - 1][j - i];
        }

        public override string ToString()
        {
            return $"{first},{Convert.ToString(mask, 2)}";
        }
    }
}