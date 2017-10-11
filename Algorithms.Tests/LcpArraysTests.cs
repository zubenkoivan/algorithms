using Algorithms.TextProcessing.LCPArrays;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class LCPArraysTests
    {
        [Fact]
        public void Should_Create_LCP_Array_With_Kasai_Algorithm()
        {
            const string text = "abacaba";
            int[] suffixArray = { 6, 4, 0, 2, 5, 1, 3 };

            int[] lcpArray = new KasaiConstructor().Construct(text, suffixArray);

            lcpArray.ShouldBeEquivalentTo(new[] { 1, 3, 1, 0, 2, 0 });
        }
    }
}