using Algorithms.TextProcessing.LongestIncreasingSubsequences;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class LisTests
    {
        [Fact]
        public void Should_Find_Lis()
        {
            const string text = "abacaba";

            string actualLis = new string(Lis<char>.Find(text.ToCharArray()));

            actualLis.ShouldBeEquivalentTo("abc");
        }
    }
}