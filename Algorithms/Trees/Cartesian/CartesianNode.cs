namespace Algorithms.Trees.Cartesian
{
    public struct CartesianNode
    {
        public readonly int Parent;
        public readonly int Left;
        public readonly int Right;

        public bool IsRoot => Parent == -1;
        public bool HasLeft => Left != -1;
        public bool HasRight => Right != -1;
        public bool HasRightChild(int rightChild) => rightChild != -1 && Right == rightChild;
        public bool HasChild(int child) => child != -1 && (Left == child || Right == child);

        public CartesianNode(int parent, int left = -1, int right = -1)
        {
            Parent = parent;
            Left = left;
            Right = right;
        }

        public CartesianNode ChangeParent(int newParent)
        {
            return new CartesianNode(newParent, Left, Right);
        }

        public CartesianNode ChangeLeftChild(int newLeftChild)
        {
            return new CartesianNode(Parent, newLeftChild, Right);
        }

        public CartesianNode ChangeRightChild(int newRightChild)
        {
            return new CartesianNode(Parent, Left, newRightChild);
        }
    }
}