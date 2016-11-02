using System.Collections.Generic;
using Algorithms.PatternMatching.ZFunctionAlgorithm;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class PatternMatching
    {
        public const string Text = "To be, or not to be, that is the question";

        public static readonly object[][] TestInput =
        {
            new object[] { "not to be", new[] { 10 } },
            new object[] { "o be, t", new[] { 15 } }
        };

        [Theory, MemberData(nameof(TestInput))]
        public void Should_Find_Pattern_In_Text_With_Z_Function(string pattern, int[] expectedIndexes)
        {
            IEnumerable<int> actualIndexes = new Pattern(pattern).IndexesIn(Text);

            actualIndexes.ShouldBeEquivalentTo(expectedIndexes);
        }
    }
}
