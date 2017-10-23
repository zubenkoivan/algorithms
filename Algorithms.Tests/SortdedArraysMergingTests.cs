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
            int[] array1 = { 3, 5, 6, 0, 0, 0, 0, 0 };
            int[] array2 = { 1, 7, 8, 9, 11 };

            Merging.MergeInPlace(array1, 0, 3, array2, 0, 5);

            int[] expected = { 1, 3, 5, 6, 7, 8, 9, 11 };

            array1.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }

        [Fact]
        public void Sorted_Arrays_Merging2()
        {
            int[] array1 = {0, 0, 0, 0, 0, 0, 0, 0, 2609, 4877, 4929, 6754, 6951, 7708, 9176, 9366};
            int[] array2 = { 11, 208, 2228, 2326, 2340, 2918, 3491, 5385 };

            Merging.MergeInPlace(array1, 8, 8, array2, 0, 8);

            int[] expected = { 1, 3, 5, 6, 7, 8, 9, 11 };

            array1.ShouldBeEquivalentTo(expected, config => config.WithStrictOrdering());
        }
    }
}