using System.Collections.Generic;
using Algorithms.TextProcessing.PatternMatching.AhoCorasickAlgorithm;
using FluentAssertions;
using Xunit;

using ZFunctionPattern = Algorithms.TextProcessing.PatternMatching.ZFunctionAlgorithm.Pattern;
using KnuthMorrisPrattPattern = Algorithms.TextProcessing.PatternMatching.KnuthMorrisPrattAlgorithm.Pattern;

namespace Algorithms.Tests
{
    public class PatternMatchingTests
    {
        public static readonly object[][] TestInput =
        {
            new object[] { "not to be", new[] { 10 } },
            new object[] { "o be, t", new[] { 15 } }
        };

        [Theory, MemberData(nameof(TestInput))]
        public void Should_Find_Pattern_In_Text_With_Z_Function(string pattern, int[] expectedIndexes)
        {
            IEnumerable<int> actualIndexes = new ZFunctionPattern(pattern).IndexesIn(TestData.Text);

            actualIndexes.ShouldBeEquivalentTo(expectedIndexes);
        }

        [Theory, MemberData(nameof(TestInput))]
        public void Should_Find_Pattern_In_Text_With_Knuth_Morris_Pratt(string pattern, int[] expectedIndexes)
        {
            IEnumerable<int> actualIndexes = new KnuthMorrisPrattPattern(pattern).IndexesIn(TestData.Text);

            actualIndexes.ShouldBeEquivalentTo(expectedIndexes);
        }

        [Fact]
        public void Should_Find_Patterns_In_Text_With_Aho_Corasick()
        {
            IEnumerable<PatternLocation> actualLocations = new PatternsCollection("not to be", "o be, t").LocationsIn(TestData.Text);

            actualLocations.ShouldBeEquivalentTo(new[] { new PatternLocation(10, 9), new PatternLocation(15, 7) });
        }

    }
}
