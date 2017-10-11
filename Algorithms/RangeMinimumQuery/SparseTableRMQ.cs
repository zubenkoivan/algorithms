using System;
using Algorithms.BitOperations;
using Algorithms.RangeMinimumQuery.Abstractions;

namespace Algorithms.RangeMinimumQuery
{
    public class SparseTableRMQ : RMQ
    {
        private readonly Minimum[][] sparseTable;

        public SparseTableRMQ(int[] array)
        {
            int arrayLength = array.Length;
            sparseTable = new Minimum[arrayLength][];

            for (int i = 0; i < sparseTable.Length; ++i)
            {
                var sparseArray = new Minimum[LogBase2.Find(arrayLength - i) + 1];
                sparseArray[0] = new Minimum(i, array[i]);
                sparseTable[i] = sparseArray;
            }

            FillSparseTable(sparseTable);
        }

        private static void FillSparseTable(Minimum[][] sparseTable)
        {
            int maxPower = sparseTable[0].Length;

            for (int power = 1; power < maxPower; ++power)
            {
                for (int row = 0; row < sparseTable.Length; ++row)
                {
                    Minimum[] sparseArray = sparseTable[row];

                    if (power >= sparseArray.Length)
                    {
                        break;
                    }

                    int previousPower = power - 1;
                    Minimum min1 = sparseArray[previousPower];
                    int min2Row = row + (int) Math.Pow(2.0, previousPower);
                    Minimum min2 = sparseTable[min2Row][previousPower];

                    sparseArray[power] = Minimum.Min(min1, min2);
                }
            }
        }

        public override Minimum this[int i, int j]
        {
            get
            {
                if (i > j || j >= sparseTable.Length)
                {
                    throw new ArgumentOutOfRangeException(nameof(j));
                }

                return i == j ? sparseTable[i][0] : FindMin(i, j);
            }
        }

        private Minimum FindMin(int i, int j)
        {
            int length = j - i + 1;
            int logBase2 = LogBase2.Find(length);
            int power = (int) Math.Pow(2.0, logBase2);

            Minimum min1 = sparseTable[i][logBase2];

            if (power == length)
            {
                return min1;
            }

            Minimum min2 = sparseTable[j - power + 1][logBase2];

            return Minimum.Min(min1, min2);
        }
    }
}
