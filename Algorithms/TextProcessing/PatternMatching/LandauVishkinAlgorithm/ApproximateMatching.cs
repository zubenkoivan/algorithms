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

        private DiagonalLengths currentLengths;
        private DiagonalLengths nextLengths;
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

            currentLengths = new DiagonalLengths(diagonals, suffixArrayRanks, lcpRmq);
            nextLengths = new DiagonalLengths(diagonals, suffixArrayRanks, lcpRmq);
            diagonalOrigins = new int[diagonals.Count];

            PatternMatch result = FindPatternImpl(maxDistance);

            currentLengths = null;
            nextLengths = null;
            diagonalOrigins = null;

            return result;
        }

        private PatternMatch FindPatternImpl(int maxDistance)
        {
            for (int k = 0; k <= maxDistance; ++k)
            {
                for (int i = patternLength - k - 1; i < diagonals.Count; ++i)
                {
                    if (diagonals.IsStartingDistance(i, k))
                    {
                        nextLengths.SkipLCP(i);
                        diagonalOrigins[i] = i;

                        if (nextLengths.IsMax(i) && diagonals.CanMatch(i))
                        {
                            return CreatePatternMatch(i);
                        }

                        continue;
                    }

                    int? diagonalMatch = IncreaseLengths(i);

                    if (diagonalMatch.HasValue)
                    {
                        return CreatePatternMatch(diagonalMatch.Value);
                    }
                }

                Swap(ref currentLengths, ref nextLengths);
            }

            return null;
        }

        private PatternMatch CreatePatternMatch(int diagonalMatch)
        {
            int diagonalOrigin = diagonalOrigins[diagonalMatch];
            int length = diagonalOrigin == diagonalMatch
                ? diagonals.MaxLength(diagonalOrigin)
                : diagonalMatch + patternLength - diagonalOrigin;
            return new PatternMatch(diagonals.TextIndex(diagonalOrigin), length);
        }

        private int? IncreaseLengths(int diagonal)
        {
            int leftDiagonal = diagonal - 1;
            int nextLength = currentLengths[diagonal] + 1;
            int leftNextLength = nextLength
                                 - diagonals.StartingDistance(leftDiagonal)
                                 + diagonals.StartingDistance(diagonal);
            int diagonalOrigin = diagonalOrigins[diagonal];

            return IncreaseLength(diagonalOrigin, leftDiagonal, leftNextLength) ??
                   IncreaseLength(diagonalOrigin, diagonal, nextLength) ??
                   IncreaseLength(diagonalOrigin, diagonal + 1, nextLength - 1);
        }

        private int? IncreaseLength(int diagonalOrigin, int diagonal, int nextLength)
        {
            if (!diagonals.IsInRange(diagonal)
                || nextLength <= currentLengths[diagonal]
                || nextLength > diagonals.MaxLength(diagonal))
            {
                return null;
            }

            nextLengths[diagonal] = nextLength;
            nextLengths.SkipLCP(diagonal);
            diagonalOrigins[diagonal] = diagonalOrigin;

            bool matched = nextLengths.IsMax(diagonal) && diagonals.CanMatch(diagonal);

            return matched ? diagonal : (int?) null;
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T tmp = arg1;
            arg1 = arg2;
            arg2 = tmp;
        }
    }
}
