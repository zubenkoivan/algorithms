using Algorithms.TextProcessing.PatternMatching.LandauVishkinAlgorithm;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class ApproximatePatternMatchingTests
    {
        [Theory]
        [InlineData(1, "acxabcyza", "abac", 3, 3)]
        [InlineData(1, "acxabacyza", "abac", 3, 4)]
        [InlineData(3, "aixajnkyza", "abac", 0, 1)]
        public void Should_Find_Pattern(int maxDistance, string text, string pattern, int matchIndex, int matchLength)
        {
            PatternMatch actual = new ApproximateMatching(text, pattern).FindPattern(maxDistance);

            actual.ShouldBeEquivalentTo(new PatternMatch(matchIndex, matchLength));
        }

        [Fact]
        public void Should_Fail_To_Find_Pattern()
        {
            const string text = "acxabcyza";
            const string pattern = "iuy";

            PatternMatch actual = new ApproximateMatching(text, pattern).FindPattern(1);

            actual.ShouldBeEquivalentTo(null);
        }
    }
}