using System;
using Algorithms.BitOperations;

namespace Algorithms.RangeMinimumQuery
{
    public class SparseTableRMQ : RMQ
    {
        private readonly int[][] sparseTable;

        public SparseTableRMQ(int[] array)
        {
            int arrayLength = array.Length;
            sparseTable = new int[arrayLength][];

            for (int i = 0; i < sparseTable.Length; ++i)
            {
                var sparseArray = new int[LogBase2.Find(arrayLength - i) + 1];
                sparseArray[0] = array[i];
                sparseTable[i] = sparseArray;
            }

            FillSparseTable(sparseTable);
        }

        private static void FillSparseTable(int[][] sparseTable)
        {
            int maxPower = sparseTable[0].Length;

            for (int power = 1; power < maxPower; ++power)
            {
                for (int row = 0; row < sparseTable.Length; ++row)
                {
                    int[] sparseArray = sparseTable[row];

                    if (power >= sparseArray.Length)
                    {
                        break;
                    }

                    int previousPower = power - 1;
                    int min1 = sparseArray[previousPower];
                    int min2Row = row + (int) Math.Pow(2.0, previousPower);
                    int min2 = sparseTable[min2Row][previousPower];

                    sparseArray[power] = Math.Min(min1, min2);
                }
            }
        }

        public override int this[int i, int j]
        {
            get
            {
                if (i > j)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                return i == j ? sparseTable[i][0] : FindRMQ(i, j);
            }
        }

        private int FindRMQ(int i, int j)
        {
            int length = j - i + 1;
            int logBase2 = LogBase2.Find(length);
            int power = (int) Math.Pow(2.0, logBase2);

            int min1 = sparseTable[i][logBase2];

            if (power == length)
            {
                return min1;
            }

            int min2 = sparseTable[j - power + 1][logBase2];

            return Math.Min(min1, min2);
        }
    }
}
