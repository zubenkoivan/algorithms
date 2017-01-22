namespace Algorithms.Graphs.Cycles
{
    public static class AcyclicityTest
    {
        public static bool HasCycles(OrientedGraph graph)
        {
            var colors = new VerticeColors(graph.VerticesCount);

            for (int i = 0; i < colors.Length; ++i)
            {
                if (colors[i] == VerticeColor.White)
                {
                    if (DFS(graph, i, colors))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool DFS(OrientedGraph graph, int vertice, VerticeColors colors)
        {
            colors[vertice] = VerticeColor.Gray;

            foreach (int edgeEnd in graph.OutgoingEdgeEnds(vertice))
            {
                if (colors[edgeEnd] == VerticeColor.White && DFS(graph, vertice, colors)
                    || colors[edgeEnd] == VerticeColor.Gray)
                {
                    return true;
                }
            }

            colors[vertice] = VerticeColor.Black;

            return false;
        }
    }
}