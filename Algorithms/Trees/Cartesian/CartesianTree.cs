namespace Algorithms.Trees.Cartesian
{
    public class CartesianTree
    {
        public static CartesianNode[] Create(int[] priorities, out int root)
        {
            var tree = new CartesianNode[priorities.Length];
            root = 0;
            tree[root] = new CartesianNode(-1);

            for (int i = 1; i < priorities.Length; ++i)
            {
                int currentPriority = priorities[i];
                int parent = i - 1;

                while (parent != -1 && currentPriority < priorities[parent])
                {
                    parent = tree[parent].Parent;
                }

                if (parent == -1)
                {
                    tree[root] = tree[root].ChangeParent(i);
                    tree[i] = new CartesianNode(parent, root, -1);
                    root = i;
                    continue;
                }

                int parentRight = tree[parent].Right;
                tree[i] = new CartesianNode(parent, parentRight, -1);
                tree[parent] = tree[parent].ChangeRightChild(i);

                if (parentRight != -1)
                {
                    tree[parentRight] = tree[parentRight].ChangeParent(i);
                }
            }

            return tree;
        }
    }
}