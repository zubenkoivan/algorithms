using System;
using Algorithms.Sorting;

namespace Algorithms.TextProcessing.SuffixArrays.KarpMillerRosenberg
{
    public class KarpMillerRosenbergConstructor : ISuffixArrayConstructor
    {
        public int[] Create(string text)
        {
            int textLength = text.Length;
            var labels = new int[textLength];
            var ranks = new int[textLength];

            for (int i = 0; i < textLength; ++i)
            {
                labels[i] = text[i];
                ranks[i] = i;
            }

            var nextLabels = new LabelsPair[textLength];
            var sortBuffer = new LabelsPair[textLength];

            for (int length = 1; length < textLength;)
            {
                length = Math.Min(length * 2, textLength);

                for (int i = 0; i < ranks.Length - length / 2; ++i)
                {
                    int half1 = ranks[i];
                    int half2 = ranks[i + length / 2];
                    nextLabels[i] = new LabelsPair(i, labels[half1], labels[half2]);
                }

                for (int i = ranks.Length - length / 2; i < ranks.Length; ++i)
                {
                    int half1 = ranks[i];
                    nextLabels[i] = new LabelsPair(i, labels[half1]);
                }

                RadixSort.Sort(nextLabels, sortBuffer, x => x.Label2);
                RadixSort.Sort(nextLabels, sortBuffer, x => x.Label1);

                if (length != textLength)
                {
                    MarkWithLabels(labels, nextLabels);
                    UpdateRanks(ranks, nextLabels);
                }
            }

            int[] suffixArray = ranks;

            for (int i = 0; i < textLength; ++i)
            {
                suffixArray[i] = nextLabels[i].SourceIndex;
            }

            return suffixArray;
        }

        private static void MarkWithLabels(int[] labels, LabelsPair[] nextLabels)
        {
            LabelsPair previous = nextLabels[0];
            int currentLabel = 0;

            for (int i = 0; i < nextLabels.Length; ++i)
            {
                LabelsPair label = nextLabels[i];

                if (previous.Label1 != label.Label1 || previous.Label2 != label.Label2)
                {
                    ++currentLabel;
                    previous = label;
                }

                labels[i] = currentLabel;
            }
        }

        private static void UpdateRanks(int[] ranks, LabelsPair[] labels)
        {
            for (int i = 0; i < labels.Length; ++i)
            {
                ranks[labels[i].SourceIndex] = i;
            }
        }
    }
}