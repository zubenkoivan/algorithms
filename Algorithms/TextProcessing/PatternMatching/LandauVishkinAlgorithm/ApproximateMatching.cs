using System;
using System.Text;
using Algorithms.RangeMinimumQuery;
using Algorithms.RangeMinimumQuery.RMQToLCA;
using Algorithms.TextProcessing.LCPArrays.Kasai;
using Algorithms.TextProcessing.SuffixArrays.KarpMillerRosenberg;

namespace Algorithms.TextProcessing.PatternMatching.LandauVishkinAlgorithm
{
    public class ApproximateMatching
    {
        private readonly int patternLength;
        private readonly int[] suffixArrayRanks;
        private readonly RMQ lcpRmq;
        private readonly Diagonals diagonals;

        private DiagonalLengths lengths;
        private int[] diagonalOrigins;

        public ApproximateMatching(string text, string pattern)
        {
            patternLength = pattern.Length;
            diagonals = new Diagonals(text.Length, patternLength);

            string combinedText = Combine(text, pattern);
            int[] suffixArray = new KarpMillerRosenbergConstructor().Create(combinedText);
            int[] lcpArray = new KasaiConstructor().Create(combinedText, suffixArray);
            suffixArrayRanks = CreateRanks(suffixArray);
            lcpRmq = new ComplexRMQ(lcpArray);
        }

        private static string Combine(string text, string pattern)
        {
            var builder = new StringBuilder(text.Length + pattern.Length + 1);

            builder.Append(text);
            builder.Append(char.MinValue);
            builder.Append(pattern);

            return builder.ToString();
        }

        private static int[] CreateRanks(int[] array)
        {
            var ranks = new int[array.Length];

            for (int i = 0; i < ranks.Length; ++i)
            {
                ranks[array[i]] = i;
            }

            return ranks;
        }

        public PatternMatch FindPattern(int maxDistance)
        {
            if (maxDistance >= patternLength)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDistance));
            }

            lengths = new DiagonalLengths(diagonals, suffixArrayRanks, lcpRmq);
            diagonalOrigins = new int[diagonals.Count];

            PatternMatch result = FindPatternImpl(maxDistance);

            lengths = null;
            diagonalOrigins = null;

            return result;
        }

        private PatternMatch FindPatternImpl(int maxDistance)
        {
            for (int k = 0; k <= maxDistance; ++k)
            {
                int i = patternLength - k - 1;

                for (; i < diagonals.Count && diagonals.IsStartingDistance(i, k); ++i)
                {
                    lengths.SkipLCP(i);
                    diagonalOrigins[i] = i;

                    if (lengths.IsMax(i) && diagonals.CanMatch(i))
                    {
                        return CreatePatternMatch(i);
                    }
                }

                int currentLength = i == diagonals.Count ? 0 : lengths[i];

                for (; i < diagonals.Count; ++i)
                {
                    int? diagonalMatch = IncreaseLengths(i, ref currentLength);

                    if (diagonalMatch.HasValue)
                    {
                        return CreatePatternMatch(diagonalMatch.Value);
                    }
                }
            }

            return null;
        }

        private PatternMatch CreatePatternMatch(int diagonalMatch)
        {
            int diagonalOrigin = diagonalOrigins[diagonalMatch];
            int length = diagonals.MaxLength(diagonalOrigin) + diagonalMatch - diagonalOrigin;
            return new PatternMatch(diagonals.TextIndex(diagonalOrigin), length);
        }

        private int? IncreaseLengths(int diagonal, ref int currentLength)
        {
            int diagonalOrigin = diagonalOrigins[diagonal];
            int nextLength = currentLength + 1;
            int leftDiagonal = diagonal - 1;
            int leftNextLength = nextLength
                                 - diagonals.StartingDistance(leftDiagonal)
                                 + diagonals.StartingDistance(diagonal);
            int rightDiagonal = diagonal + 1;

            currentLength = rightDiagonal == diagonals.Count ? 0 : lengths[rightDiagonal];

            return IncreaseLength(diagonalOrigin, leftDiagonal, leftNextLength) ??
                   IncreaseLength(diagonalOrigin, diagonal, nextLength) ??
                   IncreaseLength(diagonalOrigin, rightDiagonal, nextLength - 1);
        }

        private int? IncreaseLength(int diagonalOrigin, int diagonal, int nextLength)
        {
            if (!diagonals.IsInRange(diagonal)
                || nextLength <= lengths[diagonal]
                || nextLength > diagonals.MaxLength(diagonal))
            {
                return null;
            }

            lengths[diagonal] = nextLength;
            lengths.SkipLCP(diagonal);
            diagonalOrigins[diagonal] = diagonalOrigin;

            bool matched = lengths.IsMax(diagonal) && diagonals.CanMatch(diagonal);

            return matched ? diagonal : (int?) null;
        }
    }
}
