using System;

namespace Algorithms.Sorting
{
    internal class RadixSortContext<TElement>
    {
        private readonly int sortBase;
        private readonly Func<TElement, int> toInt;

        private int m;
        private int n;

        public int[] Counts { get; }

        public RadixSortContext(int sortBase, Func<TElement, int> toInt)
        {
            this.sortBase = sortBase;
            this.toInt = toInt;
            m = sortBase;
            n = 1;
            Counts = new int[sortBase];
        }

        public int GetDigit(TElement element)
        {
            return toInt(element) % m / n;
        }

        public void NextDigit()
        {
            n = m;
            m *= sortBase;
            Array.Clear(Counts, 0, sortBase);
        }
    }
}