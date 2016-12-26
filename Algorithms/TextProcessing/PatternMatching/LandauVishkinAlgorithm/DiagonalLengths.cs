using Algorithms.RangeMinimumQuery;

namespace Algorithms.TextProcessing.PatternMatching.LandauVishkinAlgorithm
{
    internal class DiagonalLengths
    {
        private readonly Diagonals diagonals;
        private readonly int[] suffixArrayRanks;
        private readonly RMQ lcpRmq;
        private readonly int[] lengths;

        public DiagonalLengths(Diagonals diagonals, int[] suffixArrayRanks, RMQ lcpRmq)
        {
            this.diagonals = diagonals;
            this.suffixArrayRanks = suffixArrayRanks;
            this.lcpRmq = lcpRmq;
            lengths = new int[diagonals.Count];
        }

        public int this[int index]
        {
            get { return lengths[index]; }
            set { lengths[index] = value; }
        }

        public bool IsMax(int diagonal)
        {
            return lengths[diagonal] == diagonals.MaxLength(diagonal);
        }

        public void SkipLCP(int diagonal)
        {
            if (IsMax(diagonal))
            {
                return;
            }

            int currentLength = lengths[diagonal];
            int textIndex = diagonals.TextIndex(diagonal) + currentLength;
            int patternIndex = diagonals.PatternIndex(diagonal) + currentLength;
            int lcp = LCP(suffixArrayRanks[textIndex], suffixArrayRanks[patternIndex]);

            lengths[diagonal] = currentLength + lcp;
        }

        private int LCP(int index1, int index2)
        {
            return index1 < index2
                ? lcpRmq[index1, index2 - 1].Value
                : lcpRmq[index2, index1 - 1].Value;
        }
    }
}