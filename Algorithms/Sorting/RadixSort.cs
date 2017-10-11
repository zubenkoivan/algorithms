using System;

namespace Algorithms.Sorting
{
    public static class RadixSort
    {
        private const int Base = 1000;

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
                throw new ArgumentException($"Array is less than {nameof(length)}({length})", nameof(array));
            }

            if (buffer.Length < length)
            {
                throw new ArgumentException($"Buffer array is less than {nameof(length)}({length})", nameof(buffer));
            }

            if (toInt == null)
            {
                throw new ArgumentNullException(nameof(toInt));
            }

            TElement[] source = array;
            bool canContinue = true;
            var sortContext = new RadixSortContext<TElement>(Base, toInt);

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

        private static bool CountingSort<TElement>(TElement[] source, TElement[] dest, int length,
            RadixSortContext<TElement> context)
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

            for (int i = 1; i < Base; ++i)
            {
                context.Counts[i] += context.Counts[i - 1];
            }

            for (int i = length - 1; i >= 0; --i)
            {
                TElement element = source[i];
                int number = context.GetDigit(element);
                --context.Counts[number];
                dest[context.Counts[number]] = element;
            }

            return true;
        }
    }
}
