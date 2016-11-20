using System;
using Algorithms.Sorting;

namespace Algorithms.PatternMatching.SuffixArrays.KarpMillerRosenberg
{
    public class KarpMillerRosenbergConstructor : ISuffixArrayConstructor
    {
        public int[] Create(string text)
        {
            var labels = new LabelsPair[text.Length];
            var nextLabels = new LabelsPair[text.Length];
            var sortBuffer = new LabelsPair[text.Length];
            var ranks = new int[text.Length];

            for (int i = 0; i < text.Length; ++i)
            {
                labels[i] = new LabelsPair(i, text[i]);
            }

            for (int i = 0; i < labels.Length; ++i)
            {
                ranks[i] = i;
            }

            for (int length = 1; length < text.Length;)
            {
                length = Math.Min(length * 2, text.Length);

                for (int i = 0; i < ranks.Length - length / 2; ++i)
                {
                    int half1 = ranks[i];
                    int half2 = ranks[i + length / 2];
                    nextLabels[i] = new LabelsPair(i, labels[half1].Label1, labels[half2].Label1);
                }

                for (int i = ranks.Length - length / 2; i < ranks.Length; ++i)
                {
                    int half1 = ranks[i];
                    nextLabels[i] = new LabelsPair(i, labels[half1].Label1);
                }

                Swap(ref labels, ref nextLabels);

                RadixSort.Sort(labels, sortBuffer, x => x.Label2);
                RadixSort.Sort(labels, sortBuffer, x => x.Label1);

                MarkWithLabels(labels);
                UpdateRanks(ranks, labels);
            }

            int[] result = ranks;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = labels[i].SourceIndex;
            }

            return result;
        }

        private static void MarkWithLabels(LabelsPair[] labels)
        {
            LabelsPair previous = labels[0];
            int currentLabel = 0;

            for (int i = 0; i < labels.Length; ++i)
            {
                LabelsPair label = labels[i];

                if (previous.Label1 != label.Label1 || previous.Label2 != label.Label2)
                {
                    ++currentLabel;
                    previous = label;
                }

                labels[i] = new LabelsPair(label.SourceIndex, currentLabel);
            }
        }

        private static void UpdateRanks(int[] ranks, LabelsPair[] labels)
        {
            for (int i = 0; i < labels.Length; ++i)
            {
                ranks[labels[i].SourceIndex] = i;
            }
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}