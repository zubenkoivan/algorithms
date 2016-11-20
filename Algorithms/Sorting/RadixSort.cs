using System;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        public static void Sort<TElement>(TElement[] array, Func<TElement, int> toInt)
        {
            TElement[] source = array;
            var dest = new TElement[source.Length];
            var digitCounts = new int[10];
            bool hasDigits = true;

            for (int digitNumber = 0; hasDigits; ++digitNumber)
            {
                hasDigits = false;
                int m = (int)Math.Pow(10, digitNumber + 1);
                int n = m / 10;

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

                Swap(ref source, ref dest);
                Array.Clear(digitCounts, 0, 10);
            }

            if (source != array)
            {
                Array.Copy(source, array, source.Length);
            }
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }
    }
}
