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
            int[] array1 = { 3, 5, 6, 0, 0, 0, 0, 0 };
            int[] array2 = { 1, 7, 8, 9, 11 };

            SortedArrays.MergeInPlace(array1, 0, 3, array2, 0, 5);

            int[] expected = { 1, 3, 5, 6, 7, 8, 9, 11 };

            array1.ShouldBeEquivalentTo(expected);
        }
    }
}