using System;

namespace Algorithms.RangeMinimumQuery.FarachColtonBender
{
    internal class MaskMins
    {
        private readonly int[][][] maskMins;

        public MaskMins(int maskSize)
        {
            maskMins = CreateMaskMins(maskSize);
        }

        public int this[int mask, int maskI, int maskJ] => maskMins[mask][maskI][maskJ - maskI];

        private static int[][][] CreateMaskMins(int maskSize)
        {
            int mask = (int) Math.Pow(2.0, maskSize) - 1;
            var maskMins = new int[mask + 1][][];

            while (mask >= 0)
            {
                maskMins[mask] = CalculateMaskMins(maskSize, mask);
                --mask;
            }

            return maskMins;
        }
        private static int[][] CalculateMaskMins(int maskSize, int mask)
        {
            var maskMins = new int[maskSize][];

            for (int i = 0; i < maskMins.Length; ++i)
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
                        minIndex = i + j;
                    }

                    minIndexes[j] = minIndex;
                }

                maskMins[i] = minIndexes;
            }

            return maskMins;
        }
    }
}