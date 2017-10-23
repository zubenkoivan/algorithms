using System;
using System.Collections.Generic;
using System.Diagnostics;
using Algorithms.BitOperations;
using Algorithms.Sorting.ArrayMerging;

namespace Algorithms.Sorting
{
    public class MergeSort
    {
        [DebuggerStepThrough]
        public static void Sort<T>(T[] array)
        {
            Sort(array, 0, array.Length, Comparer<T>.Default);
        }

        [DebuggerStepThrough]
        public static void Sort<T>(T[] array, int startIndex, int length)
        {
            Sort(array, startIndex, length, Comparer<T>.Default);
        }

        public static void Sort<T>(T[] array, int startIndex, int length, IComparer<T> comparer)
        {
            var buffer = new T[1 << LogBase2.Find(array.Length - 1)];

            for (int size = 1; size < length; size <<= 1)
            {
                int leftLimit = length - size;
                int leftStep = size << 1;
                for (int left = 0; left < leftLimit; left += leftStep)
                {
                    int right = left + size;
                    int rightSize = Math.Min(size, length - right);

                    if (comparer.Compare(array[right - 1], array[right]) <= 0)
                    {
                        continue;
                    }

                    Array.Copy(array, left, buffer, 0, size);
                    Merging.Merge(buffer, 0, size, array, right, rightSize, array, left, comparer);
                }
            }
        }
    }
}