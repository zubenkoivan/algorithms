using Algorithms.RangeMinimumQuery.Abstractions;

namespace Algorithms.RangeMinimumQuery
{
    public class RMQThroughLCAConstructor : RMQConstructor
    {
        public override RMQ Construct(int[] array)
        {
            return new RMQThroughLCA(array);
        }
    }
}