using Algorithms.RangeMinimumQuery.Abstractions;

namespace Algorithms.RangeMinimumQuery
{
    public class SparseTableRMQConstructor : RMQConstructor
    {
        public override RMQ Construct(int[] array)
        {
            return new SparseTableRMQ(array);
        }
    }
}