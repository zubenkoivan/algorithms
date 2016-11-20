using System;
using System.Linq;
using Algorithms.Sorting;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SortingTests
    {
        [Fact]
        public void Radix_Sort()
        {
            int[] array = GenerateRandomArray();
            int[] expected = array.OrderBy(x => x).ToArray();

            RadixSort.Sort(array, x => x);

            expected.ShouldBeEquivalentTo(array);
        }

        private static int[] GenerateRandomArray()
        {
            var array = new int[1000];
            var random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                array[i] = random.Next(10000);
            }

            return array;
        }
    }
}