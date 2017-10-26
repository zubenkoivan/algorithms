using Algorithms.Sorting.ArrayMerging;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SortdedArraysMergingTests
    {
        [Fact]
        public void Sorted_Arrays_Merging()
        {
            int[] array1 = { 0, 0, 0, 1, 7, 8, 9, 11 };
            int[] array2 = { 3, 5, 6, 0, 0, 0, 0, 0 };

            Merging.Merge(array1, 3, 5, array2, 0, 3, array1, 0);

            int[] expected = { 1, 3, 5, 6, 7, 8, 9, 11 };

            array1.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }
    }
}