using System.Linq;
using Algorithms.TextProcessing.SuffixArrays;
using Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders;
using Algorithms.TextProcessing.SuffixArrays.KarpMillerRosenberg;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class SuffixArraysTests
    {
        public static readonly object[][] Constructors =
        {
            new object[] { new KarpMillerRosenbergConstructor() },
            new object[] { new KarkkainenSandersConstructor() }
        };

        [Theory, MemberData(nameof(Constructors))]
        public void Should_Create_Suffix_Array(ISuffixArrayConstructor constructor)
        {
            const string text = TestData.Text;
            int[] actualSuffixArray = constructor.Create(text);

            actualSuffixArray.ShouldBeEquivalentTo(ExpectedSuffixArray(text));
        }

        private static int[] ExpectedSuffixArray(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Select(i => new
                {
                    Index = i,
                    Suffix = text.Substring(i)
                })
                .OrderBy(x => x.Suffix)
                .Select(x => x.Index)
                .ToArray();
        }
    }
}