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
            var array = new int[1000];
            var random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                array[i] = random.Next(10000);
            }

            int[] actual = RadixSort.Sort(array, x => x);
            int[] expected = array.OrderBy(x => x).ToArray();

            expected.ShouldBeEquivalentTo(actual);
        }
    }
}