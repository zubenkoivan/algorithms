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
            int[] array1 = { 0, 0, 0, 0, 1, 4, 5, 9 };
            int[] array2 = { 3, 6, 8 };

            SortedArrays.Merge(array1, array2, (i1, i2) => i1.CompareTo(i2));

            int[] expected = { 0, 1, 3, 4, 5, 6, 8, 9 };

            array1.ShouldBeEquivalentTo(expected);
        }
    }
}