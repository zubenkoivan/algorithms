using System;

namespace Algorithms.SortedArraysMerging
{
    public static class SortedArrays
    {
        public static void Merge<T>(T[] array1, T[] array2, Func<T, T, int> compare)
        {
            Merge(array1, array2.Length, array1.Length - array2.Length,
                array2, 0, array2.Length,
                compare);
        }

        public static void Merge<T>(T[] array1, int array1Start, int length1,
            T[] array2, int array2Start, int length2,
            Func<T, T, int> compare)
        {
            if (array1.Length < length1 + length2)
            {
                throw new ArgumentException($"{nameof(array1)} will contain result, it must not be less than {nameof(length1)} + {nameof(length2)}");
            }

            if (array1Start < length2)
            {
                throw new ArgumentException($"{nameof(length2)} must not be greater than {nameof(array1Start)}");
            }

            int mergedArrayEnd = array1Start + length1;
            int array1Index = array1Start;
            int array1End = array1Start + length1;
            int array2Index = array2Start;
            int array2End = array2Start + length2;

            for (int i = array1Start - length2; i < mergedArrayEnd; ++i)
            {
                if (array2Index == array2End)
                {
                    break;
                }

                if (array1Index == array1End)
                {
                    Array.Copy(array2, array2Index, array1, i, mergedArrayEnd - i);
                    break;
                }

                T element1 = array1[array1Index];
                T element2 = array2[array2Index];

                if (compare(element1, element2) < 0)
                {
                    Swap(array1, array1Index, i);
                    ++array1Index;
                }
                else
                {
                    array1[i] = element2;
                    ++array2Index;
                }
            }
        }

        private static void Swap<T>(T[] array, int index1, int index2)
        {
            T tmp = array[index1];
            array[index1] = array[index2];
            array[index2] = tmp;
        }
    }
}