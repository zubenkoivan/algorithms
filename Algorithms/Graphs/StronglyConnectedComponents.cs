using System.Collections.Generic;

namespace Algorithms.Graphs
{
    public class StronglyConnectedComponents
    {
        private readonly DirectedGraph graph;
        private readonly VerticeColors colors;
        private DirectedGraph invertedGraph;

        private int verticeIndex;
        private readonly int[] verticesInFinishTimeOrder;

        private List<List<int>> components;

        public StronglyConnectedComponents(DirectedGraph graph)
        {
            this.graph = graph;
            colors = new VerticeColors(graph.VerticesCount);
            verticesInFinishTimeOrder = new int[graph.VerticesCount];
            verticeIndex = graph.VerticesCount;
        }

        public int[][] Find()
        {
            ForwardDFS();
            BackwardDFS();

            var result = new int[components.Count][];

            for (int i = 0; i < result.Length; ++i)
            {
                result[i] = components[i].ToArray();
            }

            return result;
        }

        private void ForwardDFS()
        {
            for (int i = 0; i < graph.VerticesCount; ++i)
            {
                if (colors[i] == VerticeColor.White)
                {
                    ForwardDFS(i);
                }
            }
        }

        private void ForwardDFS(int vertice)
        {
            colors[vertice] = VerticeColor.Black;

            foreach (int edgeEnd in graph.OutgoingEdgeEnds(vertice))
            {
                if (colors[edgeEnd] == VerticeColor.White)
                {
                    ForwardDFS(edgeEnd);
                }
            }

            --verticeIndex;
            verticesInFinishTimeOrder[verticeIndex] = vertice;
        }

        private void BackwardDFS()
        {
            colors.Reset();
            invertedGraph = graph.Invert();
            components = new List<List<int>>();

            for (int i = 0; i < verticesInFinishTimeOrder.Length; ++i)
            {
                if (colors[i] == VerticeColor.White)
                {
                    var component = new List<int>();

                    components.Add(component);
                    BackwardDFS(i, component);
                }
            }
        }

        private void BackwardDFS(int vertice, List<int> component)
        {
            colors[vertice] = VerticeColor.Black;
            component.Add(vertice);

            foreach (int edgeEnd in invertedGraph.OutgoingEdgeEnds(vertice))
            {
                if (colors[edgeEnd] == VerticeColor.White)
                {
                    BackwardDFS(edgeEnd, component);
                }
            }
        }
    }
}