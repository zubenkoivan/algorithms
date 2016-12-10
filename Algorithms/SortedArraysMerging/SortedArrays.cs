using System;
using System.Collections.Generic;

namespace Algorithms.SortedArraysMerging
{
    public static class SortedArrays
    {
        private const int AllElementsGreater = -2;
        private const int AllElementsLess = -1;

        public static void MergeInPlace<T>(T[] array1, int start1, int length1,
            T[] array2, int start2, int length2)
            where T : IComparable<T>
        {
            Comparer<T> comparer = Comparer<T>.Create((x, y) => x.CompareTo(y));

            MergeInPlace(array1, start1, length1, array2, start2, length2, comparer);
        }

        public static void MergeInPlace<T>(T[] array1, int start1, int length1,
            T[] array2, int start2, int length2,
            IComparer<T> comparer)
        {
            if (array1.Length < length1 + length2)
            {
                throw new ArgumentException($"{nameof(array1)} will contain result, " +
                                            $"it must not be less than {nameof(length1)} + {nameof(length2)}");
            }

            if (start1 < length2)
            {
                Array.Copy(array1, start1, array1, length2, length1);
                start1 = length2;
            }

            int mergeStart = start1 - length2;
            int end1 = start1 + length1 - 1;
            int end2 = start2 + length2 - 1;

            if (length2 < length1)
            {
                MergeInPlace(array1, mergeStart, array1, start1, end1, array2, start2, end2, comparer);
            }
            else
            {
                MergeInPlace(array1, mergeStart, array2, start2, end2, array1, start1, end1, comparer);
            }
        }

        private static void MergeInPlace<T>(T[] mergeArray, int mergeStart,
            T[] array1, int start1, int end1,
            T[] array2, int start2, int end2,
            IComparer<T> comparer)
        {
            for (int i = start2; i <= end2; ++i)
            {
                int lastLess = FindLastLess(array2[i], array1, start1, end1, comparer);

                if (lastLess == AllElementsGreater)
                {
                    mergeArray[mergeStart] = array2[i];
                    ++mergeStart;
                    continue;
                }

                int length1;

                if (lastLess == AllElementsLess)
                {
                    length1 = end1 - start1 + 1;

                    if (length1 > 0)
                    {
                        Array.Copy(array1, start1, mergeArray, mergeStart, length1);
                        mergeStart += length1;
                        start1 += length1;
                    }

                    Array.Copy(array2, i, mergeArray, mergeStart, end2 - i + 1);
                    break;
                }

                length1 = lastLess - start1 + 1;
                Array.Copy(array1, start1, mergeArray, mergeStart, length1);
                mergeArray[mergeStart + length1] = array2[i];
                mergeStart += length1 + 1;
                start1 += length1;
            }

            if (start1 <= end1)
            {
                Array.Copy(array1, start1, mergeArray, mergeStart, end1 - start1 + 1);
            }
        }

        private static int FindLastLess<T>(T element, T[] array, int start, int end, IComparer<T> comparer)
        {
            if (comparer.Compare(element, array[start]) <= 0)
            {
                return AllElementsGreater;
            }

            for (int i = start + 1; i <= end; i = 2 * i - start + 1)
            {
                if (comparer.Compare(element, array[i]) > 0)
                {
                    continue;
                }

                return BinarySearchLastLess(element, array, (i + start) / 2, i, comparer);
            }

            return AllElementsLess;
        }

        private static int BinarySearchLastLess<T>(T element, T[] array, int start, int end, IComparer<T> comparer)
        {
            if (end - start == 0)
            {
                return start;
            }

            while (end - start > 1)
            {
                int middle = (start + end) / 2;

                if (comparer.Compare(element, array[middle]) <= 0)
                {
                    end = middle;
                }
                else
                {
                    start = middle;
                }
            }

            return comparer.Compare(element, array[start]) <= 0 ? start - 1 : start;
        }
    }
}