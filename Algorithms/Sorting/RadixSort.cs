using System;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        [DebuggerStepThrough]
        public static void Sort<T>(T[] array, Func<T, int> toInt)
        {
            Sort(array, 0, new T[array.Length], array.Length, toInt);
        }

        [DebuggerStepThrough]
        public static void Sort<T>(T[] array, T[] buffer, Func<T, int> toInt)
        {
            Sort(array, 0, buffer, array.Length, toInt);
        }

        [DebuggerStepThrough]
        public static void Sort<T>(T[] array, T[] buffer, int length, Func<T, int> toInt)
        {
            Sort(array, 0, buffer, length, toInt);
        }

        public static void Sort<T>(T[] array, int start, T[] buffer, int length, Func<T, int> toInt)
        {
            if (start < 0 || start >= array.Length)
            {
                throw new IndexOutOfRangeException($"{nameof(start)} is out of range");
            }

            if (array.Length < start + length)
            {
                throw new ArgumentException("is out of range", nameof(length));
            }

            if (buffer.Length < length)
            {
                throw new ArgumentException($"is less than {nameof(length)}", nameof(buffer));
            }

            if (toInt == null)
            {
                throw new ArgumentNullException(nameof(toInt));
            }

            SortImpl(array, start, buffer, length, toInt);
        }

        private static void SortImpl<T>(T[] array, int start, T[] buffer, int length, Func<T, int> toInt)
        {
            var context = new Context<T>(toInt);

            while (true)
            {
                CountingSort(context, array, start, buffer, 0, length);

                if (context.CanStop)
                {
                    Array.Copy(buffer, 0, array, start, length);
                    return;
                }

                context.NextDigit();
                CountingSort(context, buffer, 0, array, start, length);

                if (context.CanStop)
                {
                    return;
                }

                context.NextDigit();
            }
        }

        private static void CountingSort<T>(Context<T> context, T[] source, int sourceStart,
            T[] dest, int destStart, int length)
        {
            int sourceEnd = sourceStart + length;

            for (int i = sourceStart; i < sourceEnd; ++i)
            {
                int digit = context.GetDigit(source[i]);
                ++context.Counts[digit];
            }

            context.SumCounts(length);

            for (int i = sourceStart; i < sourceEnd; ++i)
            {
                int digit = context.GetDigit(source[i]);
                dest[destStart + context.Counts[digit]] = source[i];
                ++context.Counts[digit];
            }
        }

        private class Context<T>
        {
            private const int Base = 1000;

            private readonly Func<T, int> toInt;
            private int m = Base;
            private int n = 1;
            private bool canStop = true;

            public readonly int[] Counts = new int[Base];

            public bool CanStop => canStop;

            public Context(Func<T, int> toInt)
            {
                this.toInt = toInt;
            }

            public int GetDigit(T element)
            {
                int number = toInt(element);
                canStop &= number / m == 0;
                return number % m / n;
            }

            public void NextDigit()
            {
                n = m;
                m *= Base;
                canStop = true;
                Array.Clear(Counts, 0, Counts.Length);
            }

            public void SumCounts(int count)
            {
                for (int i = Counts.Length - 1; i >= 0; --i)
                {
                    count -= Counts[i];
                    Counts[i] = count;
                }
            }
        }
    }
}
