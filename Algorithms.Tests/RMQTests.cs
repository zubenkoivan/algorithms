using System.Linq;
using Algorithms.RangeMinimumQuery;
using Algorithms.RangeMinimumQuery.FarachColtonBender;
using FluentAssertions;
using Xunit;

namespace Algorithms.Tests
{
    public class RMQTests
    {
        public static readonly int[] Array =
        {
            49, 58, 53, 37, 77, 79, 93, 87, 28, 37, 77, 79
        };

        public static readonly int[] ArrayPlusMinus1BlockSize2 =
        {
            42, 43, 42, 43, 42, 41, 42, 43, 42, 43, 44, 45, 46, 45, 44, 45, 46, 45, 46, 47, 46, 47, 46, 47, 48, 47
        };

        public static readonly int[] ArrayPlusMinus1BlockSize4 =
        {
            42, 43, 42, 43, 42, 41, 42, 43, 42, 43, 44, 45, 46, 45, 44, 45, 46, 45, 46, 47, 46, 47, 46, 47, 48, 47, 46,
            45, 46, 45, 46, 45, 44, 45, 46, 47, 46, 45, 44, 45, 44, 43, 42, 41, 40, 41, 42, 41, 40, 39, 40, 41, 42, 43,
            42, 41, 40, 39, 38, 37, 38, 39, 40, 39, 38, 37, 38, 37, 36, 37, 36, 37, 38, 39, 40, 41, 40, 39, 38, 39, 38,
            37, 38, 39, 40, 41, 42, 43, 42, 43, 44, 45, 46, 45, 44, 45, 46, 45, 46, 47, 46, 47, 46, 47, 48, 47, 46, 45,
            46, 45, 46, 45, 44, 45, 46, 47, 46, 45, 44, 45, 44, 43, 42, 41, 40, 41, 42, 41, 40, 39, 40, 41, 42, 43, 42,
            41, 40, 39, 38, 37, 38, 39, 40, 39, 38, 37, 38, 37, 36, 37, 36, 37, 38, 39, 40, 41, 40, 39, 38, 39, 38, 37,
            38, 39, 40, 41
        };

        public static readonly object[][] ArraysPlusMinus1 =
        {
            new object[] { ArrayPlusMinus1BlockSize2 },
            new object[] { ArrayPlusMinus1BlockSize4 },
        };

        public static TestResult RunTest(RMQ rmq, int[] array)
        {
            TestResult testResult = TestResult.Success;

            for (int i = 0; i < array.Length; ++i)
            {
                for (int j = i; j < array.Length; ++j)
                {
                    int expected = array.Where((x, index) => index >= i && index <= j).Min();

                    testResult.Combine(TestResult.RunTest(expected, rmq[i, j], $"For {i},{j}:"));
                }
            }

            return testResult;
        }

        [Fact]
        public void Should_Find_RMQ()
        {
            var rmq = new SparseTableRMQ(Array);

            TestResult testResult = RunTest(rmq, Array);

            testResult.ShouldBeEquivalentTo(TestResult.Success);
        }

        [Theory, MemberData(nameof(ArraysPlusMinus1))]
        public void Should_Find_Plus_Minus_1_RMQ(int[] array)
        {
            var rmq = new PlusMinus1RMQ(array);

            TestResult testResult = RunTest(rmq, array);

            testResult.ShouldBeEquivalentTo(TestResult.Success);
        }
    }
}