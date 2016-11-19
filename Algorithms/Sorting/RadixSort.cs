using System;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        public static TElement[] Sort<TElement>(TElement[] array, Func<TElement, int> toInt)
        {
            var source = new TElement[array.Length];
            var dest = new TElement[array.Length];
            var digitCounts = new int[10];
            bool hasDigits = true;

            Array.Copy(array, source, array.Length);

            for (int digitNumber = 0; hasDigits; ++digitNumber)
            {
                hasDigits = false;
                int m = (int) Math.Pow(10, digitNumber + 1);
                int n = m/10;

                for (int i = 0; i < source.Length; ++i)
                {
                    int digit = toInt(source[i])%m/n;
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

                TElement[] temp = source;
                source = dest;
                dest = temp;

                Array.Clear(digitCounts, 0, 10);
                Array.Clear(dest, 0, dest.Length);
            }

            return source;
        }
    }
}
