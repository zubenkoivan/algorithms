using Algorithms.TextProcessing.SuffixArrays;
using Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders;
using Algorithms.TextProcessing.SuffixArrays.KarpMillerRosenberg;
using Xunit;

namespace Algorithms.Tests
{
    public class SuffixArraysTests
    {
        [Fact]
        public void Should_Check_Text_Has_Pattern_With_Karp_Miller_Rosenberg_Suffix_Array()
        {
            var text = new SuffixArray(new KarpMillerRosenbergConstructor(), TestData.Text);

            //text.HasPattern("not to be").Should().BeTrue();
        }

        [Fact]
        public void Should_Check_Text_Has_Pattern_With_Karkkainen_Sanders_Suffix_Array()
        {
            //var text = new SuffixArray(new KarkkainenSandersConstructor(), "abacaba");
            var text = new SuffixArray(new KarkkainenSandersConstructor(), TestData.Text);

            //text.HasPattern("not to be").Should().BeTrue();
        }
    }
}