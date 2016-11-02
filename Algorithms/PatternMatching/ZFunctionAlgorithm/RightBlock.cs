namespace Algorithms.PatternMatching.ZFunctionAlgorithm
{
    internal class RightBlock
    {
        private int leftBorder = -1;
        private int rightBorder = -1;
        private int length;

        public int LeftBorder => leftBorder;

        public int NextIndex => rightBorder + 1;

        public int Length => length;

        public bool Covers(int index)
        {
            return rightBorder > index;
        }

        public void Update(int newLeftBorder, int newLength)
        {
            if (rightBorder <= newLeftBorder + newLength - 1)
            {
                leftBorder = newLeftBorder;
                rightBorder = newLeftBorder + newLength - 1;
                length = newLength;
            }
        }

        public int RightPartLength(int middleIndex)
        {
            return rightBorder - middleIndex + 1;
        }
    }
}
