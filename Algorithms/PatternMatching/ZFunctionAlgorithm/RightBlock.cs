namespace Algorithms.PatternMatching.ZFunctionAlgorithm
{
    internal class RightBlock
    {
        private int rightBorder = -1;

        public int LeftBorder { get; private set; } = -1;

        public int NextIndex => rightBorder + 1;

        public int Length { get; private set; }

        public bool Covers(int index)
        {
            return rightBorder > index;
        }

        public void Update(int newLeftBorder, int newLength)
        {
            if (rightBorder <= newLeftBorder + newLength - 1)
            {
                LeftBorder = newLeftBorder;
                rightBorder = newLeftBorder + newLength - 1;
                Length = newLength;
            }
        }

        public int RightPartLength(int middleIndex)
        {
            return rightBorder - middleIndex + 1;
        }
    }
}
