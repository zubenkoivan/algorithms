using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public class BinaryInsertionSort
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
            int endIndex = startIndex + length - 1;

            for (int i = startIndex + 1; i <= endIndex; ++i)
            {
                T element = array[i];
                int start = startIndex;
                int end = i - 1;

                if (comparer.Compare(array[end], element) <= 0)
                {
                    continue;
                }

                if (comparer.Compare(array[start], element) <= 0)
                {
                    while (end - start > 1)
                    {
                        int middle = (start + end) / 2;

                        if (comparer.Compare(element, array[middle]) < 0)
                        {
                            end = middle;
                        }
                        else
                        {
                            start = middle;
                        }
                    }

                    start = end;
                }

                Array.Copy(array, start, array, start + 1, i - start);
                array[start] = element;
            }
        }
    }
}
