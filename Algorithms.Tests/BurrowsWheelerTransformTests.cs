using Algorithms.TextProcessing.BurrowsWheelerTransforms;
using Algorithms.TextProcessing.BurrowsWheelerTransforms.PatternMatching;
using Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class BurrowsWheelerTransformTests
    {
        [Fact]
        public void Should_Transform_Text()
        {
            const string expected = "nootr,e,stee\0h  hbbuttt o Tti n oieao  s q";

            string actual = new BurrowsWheelerTransform(new KarkkainenSandersConstructor()).Transform(TestData.Text);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Fact]
        public void Should_Restore_Text()
        {
            const string text = "nootr,e,stee\0h  hbbuttt o Tti n oieao  s q";

            string actual = new BurrowsWheelerInverseTransform().Transform(text);

            actual.ShouldBeEquivalentTo(TestData.Text);
        }

        [Fact]
        public void Should_Find_Pattern1()
        {
            const string text = "nootr,e,stee\0h  hbbuttt o Tti n oieao  s q";

            int actual = new BurrowsWheelerPatternMatcher(text).PatternCount("be");

            actual.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void Should_Find_Pattern2()
        {
            const string text = "nootr,e,stee\0h  hbbuttt o Tti n oieao  s q";

            int actual = new BurrowsWheelerPatternMatcher(text).PatternCount("that");

            actual.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public void Should_Find_Pattern3()
        {
            const string text = "nooooooooootttttrrrrr,,,,,eeeee,,,,,sssssttttteeeeeeeeeennnn\0hhhhh          hhhhhbbbbbbbbbbuuuuuttttttttttttttt     ooooo     TTTTTtttttiiiii     nnnnn     oooooiiiiieeeeeaaaaaooooo          sssss     qqqqq";

            int actual = new BurrowsWheelerPatternMatcher(text).PatternCount("that");

            actual.ShouldBeEquivalentTo(5);
        }

        [Fact]
        public void Should_Not_Find_Pattern()
        {
            const string text = "nootr,e,stee\0h  hbbuttt o Tti n oieao  s q";

            int actual = new BurrowsWheelerPatternMatcher(text).PatternCount("beq");

            actual.ShouldBeEquivalentTo(0);
        }
    }
}
