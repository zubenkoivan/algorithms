using System;
using System.Buffers;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        private const int Base = 1000;
        private static readonly ArrayPool<int> NumbersCountsPool = ArrayPool<int>.Create();

        public static void Sort<TElement>(TElement[] array, TElement[] buffer, Func<TElement, int> toInt)
        {
            if (buffer.Length != array.Length)
            {
                throw new ArgumentException(nameof(buffer), "Buffer array must be of the same size as sorted array");
            }

            if (toInt == null)
            {
                throw new ArgumentNullException(nameof(toInt));
            }

            TElement[] source = array;
            int[] numbersCounts = NumbersCountsPool.Rent(Base);
            bool canContinue = true;

            for (int i = 0; canContinue; ++i)
            {
                canContinue = CountingSort(source, buffer, toInt, numbersCounts, i);
                Swap(ref source, ref buffer);
                Array.Clear(numbersCounts, 0, Base);
            }

            if (source != array)
            {
                Array.Copy(source, array, source.Length);
            }
        }

        public static void Sort<TElement>(TElement[] array, Func<TElement, int> toInt)
        {
            Sort(array, new TElement[array.Length], toInt);
        }

        private static bool CountingSort<TElement>(TElement[] source, TElement[] dest, Func<TElement, int> toInt,
            int[] numbersCounts, int iteration)
        {
            int m = (int)Math.Pow(Base, iteration + 1);
            int n = m / Base;
            int maxNumber = 0;

            for (int i = 0; i < source.Length; ++i)
            {
                int number = toInt(source[i]) % m / n;
                ++numbersCounts[number];
                maxNumber = Math.Max(maxNumber, number);
            }

            for (int i = 1; i <= maxNumber; i++)
            {
                numbersCounts[i] += numbersCounts[i - 1];
            }

            for (int i = source.Length - 1; i >= 0; --i)
            {
                int digit = toInt(source[i]) % m / n;
                --numbersCounts[digit];
                dest[numbersCounts[digit]] = source[i];
            }

            return maxNumber >= Base / 10;
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}
