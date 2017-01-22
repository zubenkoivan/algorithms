namespace Algorithms.Graphs.Cycles
{
    public static class CycleSearcher
    {
        public static int[] Find(OrientedGraph graph)
        {
            var dfs = new CycleSearcherDFS(graph);
            int[] cycle = null;

            for (int i = 0; i < graph.VerticesCount; ++i)
            {
                if (dfs.IsWhite(i) && (cycle = dfs.DFS(i)) != null)
                {
                    break;
                }
            }

            return cycle ?? new int[0];
        }
    }
}