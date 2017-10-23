using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms.Sorting.Merging
{
    public static class Merging
    {
        [DebuggerStepThrough]
        public static void MergeInPlace<T>(T[] array1, int start1, int length1,
            T[] array2, int start2, int length2)
        {
            MergeInPlace(array1, start1, length1, array2, start2, length2, Comparer<T>.Default);
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

            if (comparer.Compare(array1[end1], array2[start2]) <= 0)
            {
                Array.Copy(array1, start1, array1, mergeStart, length1);
                Array.Copy(array2, start2, array1, mergeStart + length1, length2);
                return;
            }

            if (comparer.Compare(array1[start1], array2[end2]) >= 0)
            {
                Array.Copy(array2, start2, array1, mergeStart, length2);
                return;
            }

            MergeInPlaceImpl(array1, mergeStart, array1, start1, end1, array2, start2, end2, comparer);
        }

        private static void MergeInPlaceImpl<T>(T[] mergeArray, int mergeStart,
            T[] array1, int start1, int end1,
            T[] array2, int start2, int end2,
            IComparer<T> comparer)
        {
            for (; start2 <= end2; ++start2)
            {
                if (end1 - start1 < end2 - start2)
                {
                    Swap(ref array1, ref array2);
                    Swap(ref start1, ref start2);
                    Swap(ref end1, ref end2);
                }

                int lastLess = SearchLastLess(array2[start2], array1, start1, end1, comparer);

                if (lastLess != -1)
                {
                    int length1 = lastLess - start1 + 1;
                    Array.Copy(array1, start1, mergeArray, mergeStart, length1);
                    mergeStart += length1;
                    start1 += length1;

                    if (lastLess == end1 || comparer.Compare(array1[start1], array2[end2]) >= 0)
                    {
                        int length2 = end2 - start2 + 1;
                        Array.Copy(array2, start2, mergeArray, mergeStart, length2);
                        mergeStart += length2;
                        break;
                    }
                }

                mergeArray[mergeStart] = array2[start2];
                ++mergeStart;
            }

            if (start1 <= end1)
            {
                Array.Copy(array1, start1, mergeArray, mergeStart, end1 - start1 + 1);
            }
        }

        private static void Swap<T>(ref T arg1, ref T arg2)
        {
            T tmp = arg1;
            arg1 = arg2;
            arg2 = tmp;
        }

        private static int SearchLastLess<T>(T element, T[] array, int startIndex, int endIndex, IComparer<T> comparer)
        {
            if (comparer.Compare(array[startIndex], element) >= 0)
            {
                return -1;
            }

            int index = startIndex;

            do
            {
                index = Math.Min(2 * index - startIndex + 1, endIndex);

                if (comparer.Compare(array[index], element) >= 0)
                {
                    return BinarySearchLastLess(element, array, (index + startIndex) / 2, index, comparer);
                }
            } while (index < endIndex);

            return endIndex;
        }

        private static int BinarySearchLastLess<T>(T element, T[] array, int startIndex, int endIndex, IComparer<T> comparer)
        {
            if (endIndex - startIndex == 0)
            {
                return startIndex;
            }

            while (endIndex - startIndex > 1)
            {
                int middle = (startIndex + endIndex) / 2;

                if (comparer.Compare(array[middle], element) >= 0) // array[middle] >= element
                {
                    endIndex = middle - 1;
                }
                else
                {
                    startIndex = middle + 1;
                }
            }

            return comparer.Compare(array[startIndex], element) >= 0 ? startIndex - 1 : startIndex;
        }
    }
}