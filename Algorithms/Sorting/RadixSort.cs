using System;
using System.Buffers;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        private const int Base = 1000000;
        private static readonly ArrayPool<int> NumbersCountsPool = ArrayPool<int>.Create();

        public static void Sort<TElement>(TElement[] array, Func<TElement, int> toInt)
        {
            Sort(array, new TElement[array.Length], array.Length, toInt);
        }

        public static void Sort<TElement>(TElement[] array, TElement[] buffer, Func<TElement, int> toInt)
        {
            Sort(array, buffer, array.Length, toInt);
        }

        public static void Sort<TElement>(TElement[] array, TElement[] buffer, int length, Func<TElement, int> toInt)
        {
            if (array.Length < length)
            {
                throw new ArgumentException($"Array is less than {length}", nameof(array));
            }

            if (buffer.Length < length)
            {
                throw new ArgumentException($"Buffer array is less than {length}", nameof(buffer));
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
                canContinue = CountingSort(source, buffer, length, numbersCounts, toInt, i);
                Swap(ref source, ref buffer);
                Array.Clear(numbersCounts, 0, Base);
            }

            NumbersCountsPool.Return(numbersCounts);

            if (source != array)
            {
                Array.Copy(source, array, length);
            }
        }

        private static bool CountingSort<TElement>(TElement[] source, TElement[] dest, int length,
            int[] numbersCounts, Func<TElement, int> toInt, int iteration)
        {
            int m = (int) Math.Pow(Base, iteration + 1);
            int n = m / Base;
            int maxNumber = 0;

            for (int i = 0; i < length; ++i)
            {
                int number = toInt(source[i]) % m / n;
                ++numbersCounts[number];
                maxNumber = Math.Max(maxNumber, number);
            }

            for (int i = 1; i <= maxNumber; i++)
            {
                numbersCounts[i] += numbersCounts[i - 1];
            }

            for (int i = length - 1; i >= 0; --i)
            {
                int number = toInt(source[i]) % m / n;
                --numbersCounts[number];
                dest[numbersCounts[number]] = source[i];
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
