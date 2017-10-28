using System;
using System.Buffers;
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
            using (var context = new Context<T>(toInt))
            {
                while (true)
                {
                    if (!CountingSort(context, array, start, buffer, 0, length))
                    {
                        Array.Copy(buffer, 0, array, start, length);
                        return;
                    }

                    context.NextDigit();

                    if (!CountingSort(context, buffer, 0, array, start, length))
                    {
                        return;
                    }

                    context.NextDigit();
                }
            }
        }

        private static bool CountingSort<T>(Context<T> context, T[] source, int sourceStart,
            T[] dest, int destStart, int length)
        {
            int sourceEnd = sourceStart + length;
            bool canContinue = false;

            for (int i = sourceStart; i < sourceEnd; ++i)
            {
                int number = context.ToInt(source[i]);
                int digit = number % context.M / context.N;
                canContinue |= number / context.M > 0;
                ++context.Counts[digit];
            }

            context.SumCounts(length);

            for (int i = sourceStart; i < sourceEnd; ++i)
            {
                int number = context.ToInt(source[i]);
                int digit = number % context.M / context.N;
                dest[destStart + context.Counts[digit]] = source[i];
                ++context.Counts[digit];
            }

            return canContinue;
        }

        private class Context<T> : IDisposable
        {
            private const int Base = 1000;

            private static ArrayPool<int> Pool = ArrayPool<int>.Create();

            public readonly int[] Counts = Pool.Rent(Base);
            public readonly Func<T, int> ToInt;
            public int M = Base;
            public int N = 1;

            public Context(Func<T, int> toInt)
            {
                ToInt = toInt;
            }

            public void NextDigit()
            {
                N = M;
                M *= Base;
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

            public void Dispose()
            {
                Pool.Return(Counts, true);
            }
        }
    }
}
