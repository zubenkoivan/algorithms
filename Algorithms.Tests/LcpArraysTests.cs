using Algorithms.TextProcessing.LcpArrays.Kasai;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class LcpArraysTests
    {
        [Fact]
        public void Should_Create_LCP_Array_With_Kasai_Algorithm()
        {
            const string text = "abacaba";
            int[] suffixArray = { 6, 4, 0, 2, 5, 1, 3 };

            int[] lcpArray = new KasaiConstructor().Create(text, suffixArray);

            lcpArray.ShouldBeEquivalentTo(new[] { 1, 3, 1, 0, 2, 0 });
        }
    }
}