using System;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        public static void Sort<T>(T[] array, Func<T, int> toInt)
        {
            Sort(array, new T[array.Length], array.Length, toInt);
        }

        public static void Sort<T>(T[] array, T[] buffer, Func<T, int> toInt)
        {
            Sort(array, buffer, array.Length, toInt);
        }

        public static void Sort<T>(T[] array, T[] buffer, int length, Func<T, int> toInt)
        {
            if (array.Length < length)
            {
                throw new ArgumentException($"{nameof(array)}.Length is less than {nameof(length)}", nameof(array));
            }

            if (buffer.Length < length)
            {
                throw new ArgumentException($"{nameof(buffer)}.Length is less than {nameof(length)}", nameof(buffer));
            }

            if (toInt == null)
            {
                throw new ArgumentNullException(nameof(toInt));
            }

            T[] source = array;
            bool canContinue = true;
            var sortContext = new Context<T>(toInt);

            Swap(ref source, ref buffer);

            while (canContinue)
            {
                Swap(ref source, ref buffer);
                canContinue = CountingSort(source, buffer, length, sortContext);
                sortContext.NextDigit();
            }

            if (source != array)
            {
                Array.Copy(source, array, length);
            }
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T temp = arg1;
            arg1 = arg2;
            arg2 = temp;
        }

        private static bool CountingSort<T>(T[] source, T[] dest, int length,
            Context<T> context)
        {
            for (int i = 0; i < length; ++i)
            {
                int digit = context.GetDigit(source[i]);
                ++context.Counts[digit];
            }

            if (context.Counts[0] == length)
            {
                return false;
            }

            for (int i = 1; i < Context<T>.Base; ++i)
            {
                context.Counts[i] += context.Counts[i - 1];
            }

            for (int i = length - 1; i >= 0; --i)
            {
                T element = source[i];
                int number = context.GetDigit(element);
                --context.Counts[number];
                dest[context.Counts[number]] = element;
            }

            return true;
        }

        private class Context<T>
        {
            public const int Base = 1000;

            private readonly Func<T, int> toInt;
            private int m;
            private int n;
            public readonly int[] Counts;

            public Context(Func<T, int> toInt)
            {
                this.toInt = toInt;
                n = 1;
                m = Base;
                Counts = new int[Base];
            }

            public int GetDigit(T element) => toInt(element) % m / n;

            public void NextDigit()
            {
                n = m;
                m *= Base;
                Array.Clear(Counts, 0, Base);
            }
        }
    }
}
