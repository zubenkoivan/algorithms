using Algorithms.SortedArraysMerging;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SortdedArraysMergingTests
    {
        [Fact]
        public void Sorted_Arrays_Merging()
        {
            int[] array1 = { 1, 4, 5, 9, 11, 0, 0, 0 };
            int[] array2 = { 3, 6, 8 };

            SortedArrays.MergeInPlace(array1, 0, 5, array2, 0, 3);

            int[] expected = { 1, 3, 4, 5, 6, 8, 9, 11 };

            array1.ShouldBeEquivalentTo(expected);
        }
    }
}