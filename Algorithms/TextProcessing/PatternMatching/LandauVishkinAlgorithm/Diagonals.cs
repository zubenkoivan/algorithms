using System;

namespace Algorithms.TextProcessing.PatternMatching.LandauVishkinAlgorithm
{
    internal class Diagonals
    {
        private readonly int textLength;
        private readonly int patternLength;
        private readonly int lastDiagonal;

        public readonly int Count;

        public Diagonals(int textLength, int patternLength)
        {
            this.textLength = textLength;
            this.patternLength = patternLength;
            lastDiagonal = textLength + patternLength - 2;
            Count = patternLength - 1 + textLength;
        }

        public int TextIndex(int diagonal)
        {
            return Math.Max(0, diagonal - patternLength + 1);
        }

        public int PatternIndex(int diagonal)
        {
            return StartingDistance(diagonal) + textLength + 1;
        }

        public bool IsStartingDistance(int diagonal, int distance)
        {
            return StartingDistance(diagonal) == distance;
        }

        public int StartingDistance(int diagonal)
        {
            return Math.Max(0, patternLength - diagonal - 1);
        }

        public bool CanMatch(int diagonal)
        {
            return diagonal < textLength;
        }

        public int MaxLength(int diagonal)
        {
            return Math.Min(Math.Min(diagonal, lastDiagonal - diagonal) + 1, patternLength);
        }

        public bool IsInRange(int diagonal)
        {
            return diagonal >= 0 && diagonal < Count;
        }
    }
}