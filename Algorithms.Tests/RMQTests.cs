using Algorithms.RangeMinimumQuery;
using Algorithms.RangeMinimumQuery.FarachColtonBender;
using Algorithms.RangeMinimumQuery.RMQToLCA;
using Xunit;

namespace Algorithms.Tests
{
    public class RMQTests
    {
        public static readonly int[] ArraySmall =
        {
            49, 58, 53, 37, 77, 79, 93, 87, 28, 37, 77, 79
        };

        public static readonly int[] ArrayLarge =
        {
            49, 58, 53, 37, 77, 79, 93, 87, 28, 37, 77, 79, 15, 68, 16, 26, 64, 27, 55, 80, 13, 20, 48, 64, 59, 48, 67,
            64, 80, 64, 41, 76, 22, 17, 56, 56, 69, 30, 68, 33, 92, 26, 77, 82, 92, 37, 90, 51, 96, 68, 90, 72, 39, 22,
            50, 92, 37, 74, 92, 89, 14, 20, 53, 31, 30, 63, 59, 25, 52, 85, 94, 73, 62, 66, 96, 50, 43, 73, 51, 65, 73,
            89, 99, 94, 77, 11, 60, 26, 64, 94, 47, 28, 54, 43, 26, 17, 35, 97, 25, 22, 81, 75, 11, 57, 89, 21, 70, 51,
            36, 83, 49, 51, 92, 89, 14, 20, 53, 31, 30, 63, 59, 25, 52, 85, 94, 73, 62, 66, 96, 50, 43, 73, 51, 65, 73,
            89, 99, 94, 77, 11, 60, 26, 64, 94, 47, 28, 54, 43, 26, 17, 35, 97, 25, 22, 81, 75, 11, 57, 89, 21, 70, 51
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

        public static TestResult RunTest(RMQ rmq, int[] array)
        {
            TestResult testResult = TestResult.Success;

            for (int i = 0; i < array.Length; ++i)
            {
                var expected = new Minimum(i, array[i]);

                for (int j = i; j < array.Length; ++j)
                {
                    expected = array[j] < expected.Value ? new Minimum(j, array[j]) : expected;

                    testResult.Combine(TestResult.RunTest(expected, () => rmq[i, j], $"For {i},{j}"));
                }
            }

            return testResult;
        }

        [Fact]
        public void Should_Find_RMQ_Sparse_Table()
        {
            var rmq = new SparseTableRMQ(ArrayLarge);

            TestResult testResult = RunTest(rmq, ArrayLarge);

            testResult.AssertSuccess();
        }

        [Fact]
        public void Should_Find_RMQ_Complex()
        {
            var rmq = new ComplexRMQ(ArrayLarge);

            TestResult testResult = RunTest(rmq, ArrayLarge);

            testResult.AssertSuccess();
        }

        [Fact]
        public void Should_Find_Plus_Minus_1_RMQ_Block_2()
        {
            var rmq = new PlusMinus1RMQ(ArrayPlusMinus1BlockSize2);

            TestResult testResult = RunTest(rmq, ArrayPlusMinus1BlockSize2);

            testResult.AssertSuccess();
        }

        [Fact]
        public void Should_Find_Plus_Minus_1_RMQ_Block_4()
        {
            var rmq = new PlusMinus1RMQ(ArrayPlusMinus1BlockSize4);

            TestResult testResult = RunTest(rmq, ArrayPlusMinus1BlockSize4);

            testResult.AssertSuccess();
        }
    }
}