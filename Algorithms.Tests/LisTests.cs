﻿using Algorithms.TextProcessing.LongestIncreasingSubsequences;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class LISTests
    {
        [Fact]
        public void Should_Find_Lis()
        {
            const string text = "abacaba";

            string actualLis = new string(LIS<char>.Find(text.ToCharArray()));

            actualLis.ShouldBeEquivalentTo("abc");
        }
    }
}