using System.Collections.Generic;
using Algorithms.TextProcessing.PatternMatching.AhoCorasickAlgorithm;
using Algorithms.TextProcessing.PatternMatching.KnuthMorrisPrattAlgorithm;
using Algorithms.TextProcessing.PatternMatching.ZFunctionAlgorithm;
using FluentAssertions;
using Xunit;

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
            IEnumerable<int> actualIndexes = new ZFunction(pattern).IndexesIn(TestData.Text);

            actualIndexes.ShouldBeEquivalentTo(expectedIndexes);
        }

        [Theory, MemberData(nameof(TestInput))]
        public void Should_Find_Pattern_In_Text_With_Knuth_Morris_Pratt(string pattern, int[] expectedIndexes)
        {
            IEnumerable<int> actualIndexes = new PrefixFunction(pattern).IndexesIn(TestData.Text);

            actualIndexes.ShouldBeEquivalentTo(expectedIndexes);
        }

        [Fact]
        public void Should_Find_Patterns_In_Text_With_Aho_Corasick()
        {
            IEnumerable<PatternLocation> actualLocations = new Trie("not to be", "o be, t").LocationsIn(TestData.Text);

            actualLocations.ShouldBeEquivalentTo(new[] { new PatternLocation(10, 9), new PatternLocation(15, 7) });
        }

    }
}
