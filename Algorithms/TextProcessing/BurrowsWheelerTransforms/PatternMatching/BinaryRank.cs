using System;
using System.Text;
using Algorithms.BitOperations;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching
{
    internal class BinaryRank
    {
        private const int LargeBlockLength = 64;
        private const int LargeBlockPower = 6;
        private const int LargeBlockModMask = 0x3F;
        private const int SmallBlockPower = 3;
        private const int SmallBlockModMask = 0x7;

        private readonly ulong[] binaryText;
        private readonly int[] largeBlocks;
        private readonly byte[][] smallBlocks;

        public BinaryRank(string text, char sigmaMiddle)
        {
            int largeBlocksCount = ((text.Length - 1) >> LargeBlockPower) + 1;
            binaryText = CreateBinaryText(text, sigmaMiddle, largeBlocksCount);
            largeBlocks = new int[largeBlocksCount];
            smallBlocks = new byte[largeBlocksCount][];

            FillBlocks();
        }

        private static ulong[] CreateBinaryText(string text, char sigmaMiddle, int largeBlocksCount)
        {
            var binaryText = new ulong[largeBlocksCount];
            int lastBlockLength = text.Length & LargeBlockModMask;

            for (int i = 0; i < text.Length; i += LargeBlockLength)
            {
                int length = Math.Min(LargeBlockLength, lastBlockLength);
                int nextBlockStart = i + length;
                ulong textBlock = 0;

                for (int j = i; j < nextBlockStart; ++j)
                {
                    textBlock <<= 1;
                    textBlock |= text[j] > sigmaMiddle ? 1UL : 0UL;
                }

                textBlock <<= LargeBlockLength - length;
                binaryText[i >> LargeBlockPower] = textBlock;
            }

            return binaryText;
        }

        private void FillBlocks()
        {
            int largeBlocksSum = 0;
            const int highBits = unchecked((int) 0xFF000000);

            for (int i = 0; i < binaryText.Length; ++i)
            {
                int textBlockHigh = (int) (binaryText[i] >> 32);
                int textBlockLow = (int) binaryText[i];

                largeBlocks[i] = largeBlocksSum;
                smallBlocks[i] = new byte[8];
                smallBlocks[i][1] = (byte) BitsSet.Count(textBlockHigh & highBits);
                smallBlocks[i][2] = (byte) (smallBlocks[i][1] + BitsSet.Count(textBlockHigh & 0xFF0000));
                smallBlocks[i][3] = (byte) (smallBlocks[i][2] + BitsSet.Count(textBlockHigh & 0xFF00));
                smallBlocks[i][4] = (byte) (smallBlocks[i][3] + BitsSet.Count(textBlockHigh & 0xFF));
                smallBlocks[i][5] = (byte) (smallBlocks[i][4] + BitsSet.Count(textBlockLow & highBits));
                smallBlocks[i][6] = (byte) (smallBlocks[i][5] + BitsSet.Count(textBlockLow & 0xFF0000));
                smallBlocks[i][7] = (byte) (smallBlocks[i][6] + BitsSet.Count(textBlockLow & 0xFF00));
                largeBlocksSum = smallBlocks[i][7] + BitsSet.Count(textBlockLow & 0xFF);
            }
        }

        public int Rank(int binarySymbol, int prefixLength)
        {
            int largeBlockIndex = prefixLength >> LargeBlockPower;
            int smallBlockIndex = (prefixLength & LargeBlockModMask) >> SmallBlockPower;
            int rank = largeBlocks[largeBlockIndex] + smallBlocks[largeBlockIndex][smallBlockIndex];

            int bitsToProcess = prefixLength & SmallBlockModMask;

            if (bitsToProcess > 0)
            {
                ulong textBlock = binaryText[largeBlockIndex];
                int bitsProcessed = smallBlockIndex << SmallBlockPower;
                textBlock <<= bitsProcessed;
                textBlock >>= LargeBlockLength - bitsToProcess;

                rank += BitsSet.Count((int) textBlock);
            }

            return binarySymbol == 1 ? rank : prefixLength - rank;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < binaryText.Length; ++i)
            {
                builder.Append(Convert.ToString((long) binaryText[i], 2));
            }

            return builder.ToString();
        }
    }
}