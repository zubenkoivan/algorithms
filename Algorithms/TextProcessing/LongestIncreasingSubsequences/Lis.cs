using System;

namespace Algorithms.TextProcessing.LongestIncreasingSubsequences
{
    public class Lis<T> where T : IComparable<T>
    {
        public static T[] Find(T[] input)
        {
            var minLasts = new int[input.Length];
            var resultChain = new int[input.Length];
            int lisLength = 1;

            minLasts[0] = 0;
            resultChain[0] = -1;

            for (int i = 1; i < input.Length; ++i)
            {
                T current = input[i];
                int index = FindFirstGreaterOrEqual(input, current, minLasts, lisLength);
                T greaterOrEqual = input[minLasts[index]];

                if (index == lisLength || current.CompareTo(greaterOrEqual) < 0)
                {
                    minLasts[index] = i;
                    resultChain[i] = index == 0 ? -1 : minLasts[index - 1];
                }

                if (index == lisLength)
                {
                    ++lisLength;
                }
            }

            return ExtractResult(input, resultChain, minLasts[lisLength - 1], lisLength);
        }

        private static int FindFirstGreaterOrEqual(T[] input, T element, int[] minLasts, int lisLength)
        {
            int start = 0;
            int end = lisLength - 1;

            if (lisLength == 0)
            {
                return lisLength;
            }

            if (lisLength == 1)
            {
                return element.CompareTo(input[minLasts[0]]) <= 0 ? 0 : lisLength;
            }

            if (element.CompareTo(input[minLasts[0]]) <= 0)
            {
                return 0;
            }

            if (element.CompareTo(input[minLasts[end]]) > 0)
            {
                return lisLength;
            }

            while (end - start > 1)
            {
                int middle = (start + end) / 2;
                int cmp = element.CompareTo(input[minLasts[middle]]);

                if (cmp < 0)
                {
                    end = middle;
                    continue;
                }

                if (cmp > 0)
                {
                    start = middle;
                    continue;
                }

                if (cmp == 0)
                {
                    return middle;
                }
            }

            return element.CompareTo(input[minLasts[start]]) > 0 && element.CompareTo(input[minLasts[end]]) <= 0
                ? end
                : lisLength;
        }

        private static T[] ExtractResult(T[] input, int[] resultChain, int lastIndex, int lisLength)
        {
            var lis = new T[lisLength];
            int currentIndex = lastIndex;

            for (int i = lisLength - 1; i >= 0; --i)
            {
                lis[i] = input[currentIndex];
                currentIndex = resultChain[currentIndex];
            }

            return lis;
        }
    }
}