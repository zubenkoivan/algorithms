using System.Collections.Generic;
using Algorithms.TextProcessing.LongestCommonSubsequences;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class LCSTests
    {
        [Fact]
        public void Should_Find_Lis()
        {
            char[] dna1 = "ACCGGTCGAGTGCGCGGAAGCCGGCCGAA".ToCharArray();
            char[] dna2 = "GTCGTTCGGAATGCCGTTGCTCTGTAAA".ToCharArray();

            char[] lcs = new HuntSzymanskiAlgorithm<char>(new CharComparer()).FindLCS(dna1, dna2);

            lcs.ShouldBeEquivalentTo("GTCGTCGGAAGCCGGCCGAA".ToCharArray());
        }

        public class CharComparer : IEqualityComparer<char>
        {
            public bool Equals(char x, char y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(char obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}