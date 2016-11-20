using System;
using System.Linq;
using Algorithms.Sorting;

namespace Algorithms.PatternMatching.SuffixArrays
{
    public class SuffixArray
    {
        private readonly int[] suffixArray;

        public SuffixArray(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException(nameof(text));
            }

            suffixArray = CreateSuffixArray(text);

            string[] suffixes = suffixArray.Select(x => text.Substring(x)).ToArray();
        }

        private static int[] CreateSuffixArray(string text)
        {
            var previousLabels = new LabelsPair[text.Length];
            var labels = new LabelsPair[text.Length];

            for (int i = 0; i < text.Length; ++i)
            {
                previousLabels[i] = new LabelsPair(i, text[i]);
            }

            RadixSort.Sort(previousLabels, x => x.Label1);

            LabelsPair previous = previousLabels[0];
            int currentLabel = 0;

            for (int i = 0; i < previousLabels.Length; ++i)
            {
                LabelsPair label = previousLabels[i];

                if (previous.Label1 != label.Label1 || previous.Label2 != label.Label2)
                {
                    ++currentLabel;
                    previous = label;
                }

                previousLabels[i] = new LabelsPair(label.SourceIndex, currentLabel);
            }

            var ranks = new int[previousLabels.Length];
            for (int i = 0; i < previousLabels.Length; ++i)
            {
                ranks[previousLabels[i].SourceIndex] = i;
            }

            for (int length = 2; ; length *= 2)
            {
                length = Math.Min(length, text.Length);

                for (int i = 0; i < ranks.Length - length / 2; ++i)
                {
                    int index1 = ranks[i];
                    int index2 = ranks[i + length / 2];
                    labels[i] = new LabelsPair(i, previousLabels[index1].Label1, previousLabels[index2].Label1);
                }

                for (int i = ranks.Length - length / 2; i < ranks.Length; ++i)
                {
                    int index1 = ranks[i];
                    labels[i] = new LabelsPair(i, previousLabels[index1].Label1);
                }

                RadixSort.Sort(labels, x => x.Label2);
                RadixSort.Sort(labels, x => x.Label1);

                previous = labels[0];
                currentLabel = 0;

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

                for (int i = 0; i < labels.Length; ++i)
                {
                    ranks[labels[i].SourceIndex] = i;
                }

                Swap(ref previousLabels, ref labels);

                if (length == text.Length)
                {
                    break;
                }
            }

            int[] result = ranks;

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = previousLabels[i].SourceIndex;
            }

            return result;
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}