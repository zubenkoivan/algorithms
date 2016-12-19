namespace Algorithms.Trees.Cartesian
{
    public struct CartesianNode
    {
        public readonly int Parent;
        public readonly int Left;
        public readonly int Right;

        public CartesianNode(int parent) : this(parent, -1, -1)
        {
        }

        public CartesianNode(int parent, int left, int right)
        {
            Parent = parent;
            Left = left;
            Right = right;
        }

        public bool IsRoot => Parent == -1;

        public CartesianNode ChangeParent(int index)
        {
            return new CartesianNode(index, Left, Right);
        }

        public CartesianNode ChangeLeftChild(int index)
        {
            return new CartesianNode(Parent, index, Right);
        }

        public CartesianNode ChangeRightChild(int index)
        {
            return new CartesianNode(Parent, Left, index);
        }
    }
}