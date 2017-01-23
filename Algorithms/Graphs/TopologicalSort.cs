namespace Algorithms.Graphs
{
    public class TopologicalSort
    {
        private readonly DirectedGraph graph;
        private readonly VerticeColors colors;

        private int verticeIndex;
        private readonly int[] verticesInFinishTimeOrder;

        private bool hasCycle;

        public TopologicalSort(DirectedGraph graph)
        {
            this.graph = graph;
            verticesInFinishTimeOrder = new int[graph.VerticesCount];
            colors = new VerticeColors(graph.VerticesCount);
            verticeIndex = graph.VerticesCount;
        }

        public int[] Sort()
        {
            var dfs = new TopologicalSort(graph);

            for (int i = 0; i < graph.VerticesCount; ++i)
            {
                if (colors[i] != VerticeColor.White)
                {
                    continue;
                }

                dfs.DFS(i);

                if (dfs.hasCycle)
                {
                    return new int[0];
                }
            }

            return dfs.verticesInFinishTimeOrder;
        }

        private void DFS(int vertice)
        {
            foreach (int edgeEnd in graph.OutgoingEdgeEnds(vertice))
            {
                if (hasCycle)
                {
                    break;
                }

                VerticeColor color = colors[edgeEnd];

                if (color == VerticeColor.White)
                {
                    DFS(edgeEnd);
                }

                if (color == VerticeColor.Gray)
                {
                    hasCycle = true;
                }
            }

            --verticeIndex;
            verticesInFinishTimeOrder[verticeIndex] = vertice;
        }
    }
}