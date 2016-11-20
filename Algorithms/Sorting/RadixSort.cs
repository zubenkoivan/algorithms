using System;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        public static void Sort<TElement>(TElement[] array, TElement[] buffer, Func<TElement, int> toInt)
        {
            TElement[] source = array;
            var digitCounts = new int[10];
            bool hasDigits = true;

            for (int digitNumber = 0; hasDigits; ++digitNumber)
            {
                hasDigits = CountingSort(source, buffer, toInt, digitCounts, digitNumber);
                Swap(ref source, ref buffer);
                Array.Clear(digitCounts, 0, 10);
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
            int[] digitCounts, int digitNumber)
        {
            int m = (int)Math.Pow(10, digitNumber + 1);
            int n = m / 10;
            bool hasDigits = false;

            for (int i = 0; i < source.Length; ++i)
            {
                int digit = toInt(source[i]) % m / n;
                ++digitCounts[digit];
                hasDigits |= digit > 0;
            }

            for (int i = 1; i < 10; i++)
            {
                digitCounts[i] += digitCounts[i - 1];
            }

            for (int i = source.Length - 1; i >= 0; --i)
            {
                int digit = toInt(source[i]) % m / n;
                --digitCounts[digit];
                dest[digitCounts[digit]] = source[i];
            }

            return hasDigits;
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}
