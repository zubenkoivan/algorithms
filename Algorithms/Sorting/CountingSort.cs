using System;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public class CountingSort
    {
        private const int Range = 1024;

        [DebuggerStepThrough]
        public static void Sort(int[] array)
        {
            Sort(array, 0, array.Length, x => x);
        }

        [DebuggerStepThrough]
        public static void Sort(int[] array, int startIndex, int length)
        {
            Sort(array, startIndex, length, x => x);
        }

        public static void Sort<T>(T[] array, int startIndex, int length, Func<T, int> toInt)
        {
            var counts = new int[Range];
            int endIndex = startIndex + length - 1;

            for (int i = startIndex; i <= endIndex; ++i)
            {
                ++counts[toInt(array[i])];
            }

            int count = array.Length;

            for (int i = counts.Length - 1; i >= 0; --i)
            {
                counts[i] = count - counts[i];
                count = counts[i];
            }

            var output = new T[length];

            for (int i = startIndex; i <= endIndex; ++i)
            {
                int number = toInt(array[i]);
                output[counts[number]] = array[i];
                ++counts[number];
            }

            Array.Copy(output, 0, array, startIndex, length);
        }
    }
}