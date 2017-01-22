using System;
using System.Collections.Generic;

namespace Algorithms.Graphs
{
    public class OrientedGraph
    {
        private readonly List<int>[] outEdges;

        public readonly int VerticesCount;

        public OrientedGraph(int verticesCount)
        {
            VerticesCount = verticesCount;
            outEdges = new List<int>[verticesCount];

            for (int i = 0; i < verticesCount; ++i)
            {
                outEdges[i] = new List<int>();
            }
        }

        public void AddEdge(int from, int to)
        {
            if (to < 0 || VerticesCount <= to)
            {
                throw new IndexOutOfRangeException($"Vertice '{nameof(to)}' is out of range");
            }

            outEdges[from].Add(to);
        }

        public IEnumerable<int> OutgoingEdgeEnds(int vertice)
        {
            List<int> edges = outEdges[vertice];

            for (int i = 0; i < edges.Count; ++i)
            {
                yield return edges[i];
            }
        }
    }
}