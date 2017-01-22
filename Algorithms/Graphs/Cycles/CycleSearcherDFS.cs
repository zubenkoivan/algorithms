namespace Algorithms.Graphs.Cycles
{
    internal class CycleSearcherDFS
    {
        private readonly OrientedGraph graph;
        private readonly int[] enterTimes;
        private readonly VerticeColors colors;

        private int enterTime;

        public CycleSearcherDFS(OrientedGraph graph)
        {
            this.graph = graph;
            enterTimes = new int[graph.VerticesCount];
            colors = new VerticeColors(graph.VerticesCount);
        }

        public bool IsWhite(int vertice)
        {
            return colors[vertice] == VerticeColor.White;
        }

        public int[] DFS(int vertice)
        {
            ++enterTime;
            enterTimes[vertice] = enterTime;

            foreach (int edgeEnd in graph.OutgoingEdgeEnds(vertice))
            {
                VerticeColor color = colors[edgeEnd];

                if (color == VerticeColor.White)
                {
                    int[] cycle = DFS(edgeEnd);

                    if (cycle != null)
                    {
                        return ContinueCycle(cycle, vertice);
                    }
                }

                if (color == VerticeColor.Gray)
                {
                    return StartCycle(edgeEnd, vertice);
                }
            }

            return null;
        }

        private int[] StartCycle(int cycleStart, int cycleEnd)
        {
            var cycle = new int[enterTimes[cycleEnd] - enterTimes[cycleStart] + 1];

            cycle[0] = cycleStart;
            cycle[cycle.Length - 2] = cycleEnd;
            cycle[cycle.Length - 1] = cycleStart;

            return cycle;
        }

        private int[] ContinueCycle(int[] cycle, int vertice)
        {
            int index = enterTimes[vertice] - cycle[0];

            if (index > 0)
            {
                cycle[index] = vertice;
            }

            return cycle;
        }
    }
}