using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public class InsertionSort
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
                int insertion = i;
                T element = array[i];

                while (insertion > startIndex && comparer.Compare(array[insertion - 1], element) > 0)
                {
                    array[insertion] = array[insertion - 1];
                    --insertion;
                }

                array[insertion] = element;
            }
        }
    }
}