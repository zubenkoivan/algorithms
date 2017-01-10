using System;

namespace Algorithms.Sorting
{
    internal class NumbersCounts<TElement>
    {
        private readonly int radixSortBase;
        private readonly Func<TElement, int> toInt;

        private int m;
        private int n;

        public int[] Counts { get; }

        public NumbersCounts(int radixSortBase, Func<TElement, int> toInt)
        {
            this.radixSortBase = radixSortBase;
            this.toInt = toInt;
            m = radixSortBase;
            n = 1;
            Counts = new int[radixSortBase];
        }

        public int GetNumber(TElement element)
        {
            return toInt(element) % m / n;
        }

        public void Next()
        {
            n = m;
            m *= radixSortBase;
            Array.Clear(Counts, 0, radixSortBase);
        }
    }
}