using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
                    Array.Copy(array, left, buffer, 0, size);
                    Array.Copy(array, right, buffer, size, rightSize);
                    Merging.MergeInPlace(array, right, rightSize, buffer, 0, size, comparer);
                    if (!IsSorted(array, left, size << 1, comparer))
                    {
                        string a = string.Join(",", buffer.Take(size));
                        string b = string.Join(",", buffer.Skip(size).Take(size));
                        string r = string.Join(",", array.Skip(left).Take(size << 1));
                    }
                }
            }
        }

        private static bool IsSorted<T>(T[] array, int start, int length, IComparer<T> comparer)
        {
            for (int i = start + 1; i < start + length; i++)
            {
                if (comparer.Compare(array[i - 1], array[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}