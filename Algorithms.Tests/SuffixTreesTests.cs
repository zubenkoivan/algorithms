using Algorithms.TextProcessing.SuffixTrees.UkkonenAlgorithm;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SuffixTreesTests
    {
        [Fact]
        public void Should_Check_Text_Has_Pattern_With_Suffix_Tree()
        {
            var text = new SuffixTree(TestData.Text);

            text.HasPattern("not to be").Should().BeTrue();
        }
    }
}