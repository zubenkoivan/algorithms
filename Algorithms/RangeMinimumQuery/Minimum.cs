namespace Algorithms.RangeMinimumQuery
{
    public struct Minimum
    {
        public readonly int Index;
        public readonly int Value;

        public Minimum(int index, int value)
        {
            Index = index;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Index}]: {Value}";
        }

        public static Minimum Min(Minimum min1, Minimum min2)
        {
            return min2.Value < min1.Value ? min2 : min1;
        }
    }
}