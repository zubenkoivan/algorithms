using System;
using System.Collections.Generic;

namespace Algorithms.Sorting.ArrayMerging
{
    public static class Merging
    {
        public static void Merge<T>(T[] array1, int start1, int length1,
            T[] array2, int start2, int length2,
            T[] mergeArray, int mergeStart)
        {
            Merge(array1, start1, length1, array2, start2, length2, mergeArray, mergeStart, Comparer<T>.Default);
        }

        public static void Merge<T>(T[] array1, int start1, int length1,
            T[] array2, int start2, int length2,
            T[] mergeArray, int mergeStart,
            IComparer<T> comparer)
        {
            int end1 = start1 + length1 - 1;
            int end2 = start2 + length2 - 1;

            if (comparer.Compare(array1[end1], array2[start2]) <= 0)
            {
                Copy(array1, start1, mergeArray, mergeStart, length1);
                Copy(array2, start2, mergeArray, mergeStart + length1, length2);
                return;
            }

            if (comparer.Compare(array2[end2], array1[start1]) < 0)
            {
                Copy(array2, start2, mergeArray, mergeStart, length2);
                Copy(array1, start1, mergeArray, mergeStart + length2, length1);
                return;
            }

            MergeImpl(array1, start1, end1, array2, start2, end2, mergeArray, mergeStart, comparer);
        }

        private static void Copy<T>(T[] src, int srcIndex, T[] dst, int dstIndex, int length)
        {
            if (ReferenceEquals(src, dst) && srcIndex == dstIndex)
            {
                return;
            }

            Array.Copy(src, srcIndex, dst, dstIndex, length);
        }

        private static void MergeImpl<T>(T[] array1, int start1, int end1,
            T[] array2, int start2, int end2,
            T[] mergeArray, int mergeStart,
            IComparer<T> comparer)
        {
            while (start2 <= end2)
            {
                int lastLess = SearchLastLessOrEqual(array2[start2], array1, start1, end1, comparer);

                if (lastLess != -1)
                {
                    Cut(array1, ref start1, mergeArray, ref mergeStart, lastLess - start1 + 1);

                    if (lastLess == end1 || comparer.Compare(array2[end2], array1[start1]) < 0)
                    {
                        Cut(array2, ref start2, mergeArray, ref mergeStart, end2 - start2 + 1);
                        break;
                    }
                }

                mergeArray[mergeStart] = array2[start2];
                ++mergeStart;
                ++start2;
            }

            if (start1 <= end1)
            {
                Array.Copy(array1, start1, mergeArray, mergeStart, end1 - start1 + 1);
            }
        }

        private static int SearchLastLessOrEqual<T>(T element, T[] array, int start, int end, IComparer<T> comparer)
        {
            if (comparer.Compare(element, array[start]) < 0)
            {
                return -1;
            }

            if (comparer.Compare(array[end], element) <= 0)
            {
                return end;
            }

            int index = start;

            do
            {
                index = Math.Min(2 * index - start + 1, end);

                if (comparer.Compare(element, array[index]) < 0)
                {
                    return BinarySearchLastLessOrEqual(element, array, (index + start) / 2, index, comparer);
                }
            } while (index < end);

            return end;
        }

        private static int BinarySearchLastLessOrEqual<T>(T element, T[] array, int start, int end, IComparer<T> comparer)
        {
            if (end - start == 0)
            {
                return start;
            }

            while (end - start > 1)
            {
                int middle = (start + end) / 2;

                if (comparer.Compare(array[middle], element) <= 0)
                {
                    start = middle;
                }
                else
                {
                    end = middle;
                }
            }

            return start;
        }

        private static void Cut<T>(T[] src, ref int srcStart, T[] dst, ref int dstStart, int length)
        {
            Array.Copy(src, srcStart, dst, dstStart, length);
            srcStart += length;
            dstStart += length;
        }
    }
}