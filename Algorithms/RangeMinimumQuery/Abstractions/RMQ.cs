namespace Algorithms.RangeMinimumQuery.Abstractions
{
    public abstract class RMQ
    {
        public abstract Minimum this[int i, int j] { get; }
    }
}