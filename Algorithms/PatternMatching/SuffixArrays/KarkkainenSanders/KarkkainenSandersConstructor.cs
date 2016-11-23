using Algorithms.Sorting;

namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : ISuffixArrayConstructor
    {
        public int[] Create(string text)
        {
            var labels = new Label[text.Length];
            var ranks = new int[text.Length + 1];

            for (int i = 0; i < labels.Length; ++i)
            {
                labels[i] = new Label(i, text[i]);
            }

            return CreateSuffixArray(labels, ranks);
        }

        private static int[] CreateSuffixArray(Label[] previousLabels, int[] ranks01)
        {
            if (previousLabels.Length < 3)
            {
                return CreateSimpleSuffixArray(previousLabels);
            }

            Label[] labels = CreateLabels(previousLabels);
            Label[] labels01 = CreateLabels01(labels);

            int[] suffixArray01 = CreateSuffixArray(labels01, ranks01);

            for (int i = 0; i < suffixArray01.Length; i++)
            {
                suffixArray01[i] = labels01[suffixArray01[i]].SourceIndex;
            }

            FillRanks(ranks01, suffixArray01);

            int[] suffixArray2 = CreateSuffixArray2(previousLabels, ranks01);

            return Merge(suffixArray01, suffixArray2, ranks01, previousLabels);
        }

        private static Label[] CreateLabels(Label[] labels)
        {
            Triple[] triples = CreateTriples(labels);
            var sortBuffer = new Triple[triples.Length];

            RadixSort.Sort(triples, sortBuffer, x => x.Label3(labels));
            RadixSort.Sort(triples, sortBuffer, x => x.Label2(labels));
            RadixSort.Sort(triples, sortBuffer, x => x.Label1(labels));

            var result = new Label[triples.Length];
            int currentLabael = 0;
            Triple previous = triples[0];

            for (int i = 0; i < result.Length; ++i)
            {
                Triple triple = triples[i];

                if (!previous.Equals(triple, labels))
                {
                    previous = triple;
                    ++currentLabael;
                }

                result[i] = new Label(triple.Start, currentLabael);
            }

            RadixSort.Sort(result, x => x.SourceIndex);

            return result;
        }

        private static Triple[] CreateTriples(Label[] labels)
        {
            var triples = new Triple[labels.Length];

            for (int i = 0; i < labels.Length; ++i)
            {
                triples[i] = new Triple(i, i + 2);
            }

            return triples;
        }

        private static Label[] CreateLabels01(Label[] labels)
        {
            var result = new Label[labels.Length - labels.Length / 3];

            int index = 0;

            for (int i = 0; i < labels.Length; i += 3)
            {
                result[index] = labels[i];
                ++index;
            }

            for (int i = 1; i < labels.Length; i += 3)
            {
                result[index] = labels[i];
                ++index;
            }

            return result;
        }

        private static void FillRanks(int[] ranks, int[] suffixArray)
        {
            for (int i = 0; i < suffixArray.Length; ++i)
            {
                ranks[suffixArray[i]] = i;
            }
        }

        private static int[] CreateSuffixArray2(Label[] labels, int[] ranks01)
        {
            var suffixArray2 = new int[labels.Length / 3];

            for (int i = 2; i < labels.Length; i += 3)
            {
                suffixArray2[i / 3] = i;
            }

            RadixSort.Sort(suffixArray2, i => ranks01[i + 1]);
            RadixSort.Sort(suffixArray2, i => labels[i].Value);

            return suffixArray2;
        }

        private static int[] CreateSimpleSuffixArray(Label[] labels)
        {
            if (labels.Length == 1)
            {
                return new[] { labels[0].SourceIndex };
            }

            return labels[0].Value < labels[1].Value
                ? new[] { 0, 1 }
                : new[] { 1, 0 };
        }

        private static int[] Merge(int[] suffixArray01, int[] suffixArray2, int[] ranks01, Label[] labels)
        {
            var suffixArray = new int[labels.Length];
            int index01 = 0;
            int index2 = 0;

            for (int i = 0; i < suffixArray.Length; i++)
            {
                if (index2 == suffixArray2.Length)
                {
                    suffixArray[i] = suffixArray01[index01];
                    ++index01;
                    continue;
                }
                if (index01 == suffixArray01.Length)
                {
                    suffixArray[i] = suffixArray2[index2];
                    ++index2;
                    continue;
                }

                int suffix01 = suffixArray01[index01];
                int suffix2 = suffixArray2[index2];

                if (labels[suffix01].Value < labels[suffix2].Value)
                {
                    ++index01;
                    suffixArray[i] = suffix01;
                }
                else if (labels[suffix01].Value > labels[suffix2].Value)
                {
                    ++index2;
                    suffixArray[i] = suffix2;
                }
                else if (suffix01 == labels.Length - 1)
                {
                    ++index01;
                    suffixArray[i] = suffix01;
                }
                else if (suffix2 == labels.Length - 1)
                {
                    ++index2;
                    suffixArray[i] = suffix2;
                }
                else
                {
                    if (suffix01 % 3 == 0)
                    {
                        if (ranks01[suffix01 + 1] < ranks01[suffix2 + 1])
                        {
                            ++index01;
                            suffixArray[i] = suffix01;
                        }
                        else
                        {
                            ++index2;
                            suffixArray[i] = suffix2;
                        }
                    }
                    else
                    {
                        suffix01 += 1;
                        suffix2 += 1;

                        if (labels[suffix01].Value < labels[suffix2].Value
                            || suffix01 == labels.Length - 1)
                        {
                            ++index01;
                            suffixArray[i] = suffix01 - 1;
                        }
                        else if (labels[suffix01].Value > labels[suffix2].Value
                                 || suffix2 == labels.Length - 1)
                        {
                            ++index2;
                            suffixArray[i] = suffix2 - 1;
                        }
                        else
                        {
                            if (ranks01[suffix01 + 1] < ranks01[suffix2 + 1])
                            {
                                ++index01;
                                suffixArray[i] = suffix01 - 1;
                            }
                            else
                            {
                                ++index2;
                                suffixArray[i] = suffix2 - 1;
                            }
                        }
                    }
                }
            }

            return suffixArray;
        }
    }
}