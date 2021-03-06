﻿using System;

namespace Algorithms.TextProcessing.SuffixTrees.UkkonenAlgorithm
{
    public class SuffixTree
    {
        private readonly Node root;

        public SuffixTree(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException(nameof(text));
            }

            root = new Node();
            root.SaveSuffixLink(root);
            CreateSuffixTree(text);
        }

        private void CreateSuffixTree(string text)
        {
            var currentVertice = new Vertice(root);

            for (int i = 0; i < text.Length; i++)
            {
                Node previousNode = null;

                while (true)
                {
                    if (currentVertice.IsLeaf(i))
                    {
                        currentVertice = currentVertice.SuffixLink(text);
                        continue;
                    }

                    Vertice nextVertice = currentVertice.GetNext(text[i]);

                    if (nextVertice != null)
                    {
                        previousNode?.SaveSuffixLink(currentVertice.ExplicitVertice);
                        currentVertice = nextVertice;
                        break;
                    }

                    nextVertice = currentVertice.SuffixLink(text);
                    Node node = currentVertice.AddExplicitVertice(text, i);
                    previousNode?.SaveSuffixLink(node);
                    previousNode = node;
                    currentVertice = nextVertice;
                }
            }
        }

        public bool HasPattern(string pattern)
        {
            var firstEdge = root.GetEdge(pattern[0]);

            if (firstEdge == null)
            {
                return false;
            }

            Vertice vertice = new Vertice(firstEdge, 1);

            for (int i = 1; i < pattern.Length && vertice != null; ++i)
            {
                vertice = vertice.GetNext(pattern[i]);
            }

            return vertice != null;
        }
    }
}