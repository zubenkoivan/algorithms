namespace Algorithms.Compression.Huffman
{
    internal struct TreeNode
    {
        public readonly int Frequency;
        public readonly int Left;
        public readonly int Right;
        public readonly byte Value;

        public TreeNode(byte value, int frequency)
        {
            Value = value;
            Frequency = frequency;
            Left = -1;
            Right = -1;
        }

        public TreeNode(int frequency, int left, int right)
        {
            Value = 0;
            Frequency = frequency;
            Left = left;
            Right = right;
        }

        public bool IsLeaf => Left == -1;

        public override string ToString()
        {
            return IsLeaf ? $"{(char)Value}: {Frequency}" : $"{Left},{Right}: {Frequency}";
        }
    }
}