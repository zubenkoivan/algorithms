namespace Algorithms.Graphs.TopologicalSorting
{
    public static class TopologicalSort
    {
        public static int[] Sort(OrientedGraph graph)
        {
            var dfs = new TopologicalSortDFS(graph);

            for (int i = 0; i < graph.VerticesCount; ++i)
            {
                if (!dfs.IsWhite(i))
                {
                    continue;
                }

                dfs.DFS(i);

                if (dfs.HasCycle)
                {
                    return new int[0];
                }
            }

            return dfs.SortedVertices;
        }
    }
}