namespace Algorithms.PatternMatching.AhoCorasickAlgorithm
{
    public struct PatternLocation
    {
        public readonly int Index;
        public readonly int Length;

        public PatternLocation(int index, int length)
        {
            Index = index;
            Length = length;
        }

        public override string ToString()
        {
            return $"index: {Index}, length: {Length}";
        }
    }
}
