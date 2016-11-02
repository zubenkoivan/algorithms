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
            int newRightBorder = newLeftBorder + newLength - 1;

            if (rightBorder <= newRightBorder)
            {
                LeftBorder = newLeftBorder;
                rightBorder = newRightBorder;
                Length = newLength;
            }
        }

        public int RightPartLength(int middleIndex)
        {
            return rightBorder - middleIndex + 1;
        }
    }
}
