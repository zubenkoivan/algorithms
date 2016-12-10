using System;
using System.Collections.Generic;

namespace Algorithms.SortedArraysMerging
{
    public static class SortedArrays
    {
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

            int mergedArrayEnd = start1 + length1;
            int array1Index = start1;
            int array1End = start1 + length1;
            int array2Index = start2;
            int array2End = start2 + length2;

            for (int i = start1 - length2; i < mergedArrayEnd; ++i)
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

                if (comparer.Compare(element1, element2) < 0)
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