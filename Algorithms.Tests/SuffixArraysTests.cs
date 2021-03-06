﻿using System;
using System.Linq;
using Algorithms.TextProcessing.Abstractions;
using Algorithms.TextProcessing.SuffixArrays;
using Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders;
using Algorithms.TextProcessing.SuffixArrays.KarpMillerRosenberg;
using FluentAssertions;
using Moq;
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

        [Fact]
        public void Should_Find_Index_Of_Pattern()
        {
            const string text = "abacaba";
            var suffixArrayConstructor = new Mock<SuffixArrayConstructor>();
            var lcpArrayConstructor = new Mock<LCPArrayConstructor>();

            suffixArrayConstructor.Setup(x => x.Construct(It.IsAny<string>()))
                .Returns(new[] { 6, 4, 0, 2, 5, 1, 3 });
            lcpArrayConstructor.Setup(x => x.Construct(It.IsAny<string>(), It.IsAny<int[]>()))
                .Returns(new[] { 1, 3, 1, 0, 2, 0 });

            bool actual = new SuffixArray(suffixArrayConstructor.Object, lcpArrayConstructor.Object, text).HasPattern("bacaba");
            actual.ShouldBeEquivalentTo(true);
        }

        [Theory, MemberData(nameof(Constructors))]
        public void Should_Create_Suffix_Array_Of_Short_Text(SuffixArrayConstructor constructor)
        {
            const string text = "abacaba";
            int[] actualSuffixArray = constructor.Construct(text);

            actualSuffixArray.ShouldBeEquivalentTo(ExpectedSuffixArray(text), config => config.WithStrictOrdering());
        }

        [Theory, MemberData(nameof(Constructors))]
        public void Should_Create_Suffix_Array_Of_Long_Text(SuffixArrayConstructor constructor)
        {
            const string text = TestData.Text;
            int[] actualSuffixArray = constructor.Construct(text);

            actualSuffixArray.ShouldBeEquivalentTo(ExpectedSuffixArray(text), config => config.WithStrictOrdering());
        }

        private static int[] ExpectedSuffixArray(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Select(i => new
                {
                    Index = i,
                    Suffix = text.Substring(i)
                })
                .OrderBy(x => x.Suffix, StringComparer.Ordinal)
                .Select(x => x.Index)
                .ToArray();
        }
    }
}