using System;
using System.Text;
using Algorithms.BitOperations;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class BinaryRank
    {
        private const int SmallBlockLength = 64;
        private const int SmallBlockPower = 6;
        private const int SmallBlockModMask = 0x3F;

        private readonly int largeBlockLength;
        private readonly int largeBlockPower;
        private readonly int largeBlockModMask;
        private readonly int bitsPerSmallCounter;
        private readonly int smallCounterMask;

        private readonly ulong[] binaryText;
        private readonly int[] largeBlocks;
        private readonly byte[][] smallBlocks;

        public BinaryRank(string text, char sigmaMiddle)
        {
            largeBlockPower = 2 * LogBase2.Find(LogBase2.Find(text.Length) + 1);
            largeBlockPower = Math.Max(SmallBlockPower + 1, largeBlockPower);
            largeBlockLength = 1 << largeBlockPower;
            largeBlockModMask = ~(-1 << largeBlockPower);
            bitsPerSmallCounter = ((largeBlockPower >> 2) + 1) << 2;
            smallCounterMask = ~(-1 << bitsPerSmallCounter);

            int largeBlocksCount = ((text.Length - 1) >> largeBlockPower) + 1;

            binaryText = CreateBinaryText(text, sigmaMiddle);
            largeBlocks = new int[largeBlocksCount];
            smallBlocks = new byte[largeBlocksCount][];

            FillBlocks(largeBlocksCount);
        }

        private static ulong[] CreateBinaryText(string text, char sigmaMiddle)
        {
            var binaryText = new ulong[((text.Length - 1) >> SmallBlockPower) + 1];

            for (int i = 0; i < text.Length; i += SmallBlockLength)
            {
                int length = Math.Min(SmallBlockLength, text.Length - i);
                int nextBlockStart = i + length;
                ulong textBlock = 0;

                for (int j = i; j < nextBlockStart; ++j)
                {
                    textBlock <<= 1;
                    textBlock |= text[j] > sigmaMiddle ? 1UL : 0UL;
                }

                textBlock <<= SmallBlockLength - length;
                binaryText[i >> SmallBlockPower] = textBlock;
            }

            return binaryText;
        }

        private void FillBlocks(int largeBlocksCount)
        {
            int blocksPowerDiff = largeBlockPower - SmallBlockPower;
            int maxTextBlocksCount = largeBlockLength >> SmallBlockPower;
            int largeBlocksSum = 0;

            for (int large = 0; large < largeBlocksCount; ++large)
            {
                int textBlocksStart = large << blocksPowerDiff;
                int textBlocksCount = Math.Min(maxTextBlocksCount, binaryText.Length - textBlocksStart);
                int nextTextBlocksStart = textBlocksStart + textBlocksCount;
                var currentSmallBlocks = new byte[((textBlocksCount * bitsPerSmallCounter - 1) >> 3) + 1];
                int smallBlocksSum = 0;
                int skipSmallBlocksBits = 0;

                for (int small = textBlocksStart; small < nextTextBlocksStart; ++small)
                {
                    int byteIndex = skipSmallBlocksBits >> 3;

                    skipSmallBlocksBits += bitsPerSmallCounter;

                    int mod8 = skipSmallBlocksBits & 7;
                    int bytesCount = ((bitsPerSmallCounter + mod8 - 1) >> 3) + 1;
                    int value = smallBlocksSum << mod8;

                    smallBlocksSum += BitsSet.Count(binaryText[small]);

                    for (int i = 0; i < bytesCount; ++i)
                    {
                        byte currentByte = currentSmallBlocks[byteIndex];

                        currentByte |= (byte) (value >> ((bytesCount - i - 1) << 3));
                        currentSmallBlocks[byteIndex + i] = currentByte;
                    }
                }

                smallBlocks[large] = currentSmallBlocks;
                largeBlocks[large] = largeBlocksSum;
                largeBlocksSum += smallBlocksSum;
            }
        }

        public int Rank(int binarySymbol, int prefixLength)
        {
            int largeBlockIndex = prefixLength >> largeBlockPower;
            int skipSmallBlocksBits = ((prefixLength & largeBlockModMask) >> SmallBlockPower) * bitsPerSmallCounter;
            int byteStartIndex = skipSmallBlocksBits >> 3;
            int bytesCount = ((((skipSmallBlocksBits & 7) + bitsPerSmallCounter) - 1) >> 3) + 1;
            int nextByteStartIndex = byteStartIndex + bytesCount;
            int smallRank = 0;

            for (int i = byteStartIndex; i < nextByteStartIndex; ++i)
            {
                smallRank <<= 8;
                smallRank |= smallBlocks[largeBlockIndex][i];
            }

            smallRank >>= (skipSmallBlocksBits + bitsPerSmallCounter) & 7;
            smallRank &= smallCounterMask;

            int rank = largeBlocks[largeBlockIndex] + smallRank;
            int bitsToProcess = prefixLength & SmallBlockModMask;

            if (bitsToProcess > 0)
            {
                ulong textBlock = binaryText[prefixLength >> SmallBlockPower];
                textBlock &= ulong.MaxValue << (SmallBlockLength - bitsToProcess);

                rank += BitsSet.Count(textBlock);
            }

            return binarySymbol == 1 ? rank : prefixLength - rank;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < binaryText.Length; ++i)
            {
                string value = Convert.ToString((long) binaryText[i], 2);
                value = new string('0', SmallBlockLength - value.Length) + value;

                builder.Append(value);
            }

            return builder.ToString();
        }
    }
}