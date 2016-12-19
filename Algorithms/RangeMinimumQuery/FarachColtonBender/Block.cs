using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    internal struct Block
    {
        private readonly int firstIndex;
        private readonly int mask;

        public Block(int[] source, int firstIndex, int maskSize)
        {
            this.firstIndex = firstIndex;

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

        public Minimum FindMin(int[] source, int[][][] blockMinIndexes, int i, int j)
        {
            i -= firstIndex;
            j -= firstIndex;

            if (i != 0)
            {
                int minIndex = firstIndex + blockMinIndexes[mask][i - 1][j - i];
                return new Minimum(minIndex, source[minIndex]);
            }

            if (j == 0)
            {
                return new Minimum(firstIndex, source[firstIndex]);
            }

            int index = firstIndex + blockMinIndexes[mask][0][j - 1];

            return Minimum.Min(new Minimum(firstIndex, source[firstIndex]), new Minimum(index, source[index]));
        }

        public override string ToString()
        {
            return $"{firstIndex},{Convert.ToString(mask, 2)}";
        }
    }
}