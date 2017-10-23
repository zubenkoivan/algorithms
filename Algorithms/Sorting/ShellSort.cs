using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public class ShellSort
    {
        private static readonly int[] Gaps = {701, 301, 132, 57, 23, 10, 4, 1};

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

            foreach (int gap in Gaps)
            {
                for (int i = startIndex + gap; i <= endIndex; i += 1)
                {
                    int insertion = i;
                    T element = array[i];

                    while (insertion >= gap && comparer.Compare(array[insertion - gap], element) > 0)
                    {
                        array[insertion] = array[insertion - gap];
                        insertion -= gap;
                    }

                    array[insertion] = element;
                }
            }
        }
    }
}