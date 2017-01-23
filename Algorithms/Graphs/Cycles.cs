namespace Algorithms.Graphs
{
    internal class Cycles
    {
        private readonly DirectedGraph graph;
        private readonly int[] enterTimes;
        private readonly VerticeColors colors;

        private int enterTime;

        public Cycles(DirectedGraph graph)
        {
            this.graph = graph;
            enterTimes = new int[graph.VerticesCount];
            colors = new VerticeColors(graph.VerticesCount);
        }

        public int[] Find()
        {
            var dfs = new Cycles(graph);
            int[] cycle = null;

            for (int i = 0; i < graph.VerticesCount; ++i)
            {
                if (colors[i] == VerticeColor.White && (cycle = dfs.DFS(i)) != null)
                {
                    break;
                }
            }

            return cycle ?? new int[0];
        }

        private int[] DFS(int vertice)
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