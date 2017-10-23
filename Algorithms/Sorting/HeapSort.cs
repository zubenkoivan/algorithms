using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Algorithms.Sorting
{
    public class HeapSort
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
            ValidateArgs(array, startIndex, length, comparer);

            int endIndex = startIndex + length - 1;

            CreateHeap(array, startIndex, endIndex, comparer);
            SortHeap(array, startIndex, endIndex, comparer);
        }

        private static void ValidateArgs<T>(T[] array, int startIndex, int length, IComparer<T> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (startIndex < 0 || startIndex >= array.Length)
            {
                throw new IndexOutOfRangeException(
                    $"{nameof(startIndex)} must be in range [0,{nameof(array)}.Length-1]");
            }

            if (length <= 0 || startIndex + length > array.Length)
            {
                throw new ArgumentException(
                    $"{nameof(length)} must be in range [1,{nameof(array)}.Length-{nameof(startIndex)}]",
                    nameof(length));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }
        }

        private static void CreateHeap<T>(T[] array, int startIndex, int endIndex, IComparer<T> comparer)
        {
            for (int i = (endIndex - 1) / 2; i >= startIndex; --i)
            {
                Heapify(array, i, endIndex, comparer);
            }
        }

        private static void SortHeap<T>(T[] array, int startIndex, int endIndex, IComparer<T> comparer)
        {
            for (int i = endIndex; i > startIndex; --i)
            {
                T tmp = array[startIndex];
                array[startIndex] = array[i];
                array[i] = tmp;
                Heapify(array, startIndex, i - 1, comparer);
            }
        }

        private static void Heapify<T>(T[] array, int parent, int endIndex, IComparer<T> comparer)
        {
            while (true)
            {
                int leftChild = (parent << 1) + 1;
                int rightChild = leftChild + 1;
                int largest = parent;

                if (leftChild <= endIndex && comparer.Compare(array[largest], array[leftChild]) < 0)
                {
                    largest = leftChild;
                }

                if (rightChild <= endIndex && comparer.Compare(array[largest], array[rightChild]) < 0)
                {
                    largest = rightChild;
                }

                if (largest == parent)
                {
                    break;
                }

                T tmp = array[parent];
                array[parent] = array[largest];
                array[largest] = tmp;
                parent = largest;
            }
        }
    }
}