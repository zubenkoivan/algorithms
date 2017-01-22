namespace Algorithms.Graphs.TopologicalSorting
{
    internal class TopologicalSortDFS
    {
        private readonly OrientedGraph graph;
        private readonly VerticeColors colors;

        private int index;

        public readonly int[] SortedVertices;

        public bool HasCycle { get; private set; }

        public TopologicalSortDFS(OrientedGraph graph)
        {
            this.graph = graph;
            SortedVertices = new int[graph.VerticesCount];
            colors = new VerticeColors(graph.VerticesCount);
            index = graph.VerticesCount;
        }

        public bool IsWhite(int vertice)
        {
            return colors[vertice] == VerticeColor.White;
        }

        public void DFS(int vertice)
        {
            foreach (int edgeEnd in graph.OutgoingEdgeEnds(vertice))
            {
                if (HasCycle)
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
                    HasCycle = true;
                }
            }

            --index;
            SortedVertices[index] = vertice;
        }
    }
}