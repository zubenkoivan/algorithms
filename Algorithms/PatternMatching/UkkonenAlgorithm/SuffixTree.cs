namespace Algorithms.PatternMatching.UkkonenAlgorithm
{
    internal class SuffixTree
    {
        private readonly Node root;

        public SuffixTree(string text)
        {
            root = new Node(text);
            CreateSuffixTree(text);
        }

        private void CreateSuffixTree(string text)
        {
            var currentVertice = new Vertice(root, 0);

            for (int i = 0; i < text.Length; i++)
            {
                while (true)
                {
                    if (currentVertice.IsLeaf)
                    {
                        currentVertice = currentVertice.SuffixLink;
                        continue;
                    }

                    Vertice? vertice = currentVertice.GetNext(text[i]);

                    if (vertice != null)
                    {
                        currentVertice = vertice.Value;
                        break;
                    }

                    Vertice suffixLink = currentVertice.SuffixLink;
                    currentVertice.AddExplicit(text[i], i);
                    currentVertice = suffixLink;
                }
            }
        }
    }
}