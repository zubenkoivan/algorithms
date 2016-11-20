namespace Algorithms.PatternMatching.SuffixArrays.KarkkainenSanders
{
    public class KarkkainenSandersConstructor : ISuffixArrayConstructor
    {
        public int[] Create(string text)
        {
            return null;
            //var triples = new Triple[text.Length];

            //for (int i = 0; i < triples.Length - 2; ++i)
            //{
            //    triples[i] = new Triple(i, i + 2);
            //}

            //triples[triples.Length - 2] = new Triple(triples.Length - 2, text.Length);
            //triples[triples.Length - 1] = new Triple(triples.Length - 1, text.Length);

            //CreateSuffixArray(text, triples);
        }

        //private static int[] CreateSuffixArray(string text, Triple[] triples)
        //{
        //    if (triples.Length < 3)
        //    {
        //        return CreateSimpleSuffixArray(text, triples);
        //    }

        //    var newTriples = new Triple[triples.Length * 2 / 3 + triples.Length % 3];

        //    int index = 0;

        //    for (int i = 0; i < triples.Length; i += 2)
        //    {
        //        newTriples[index] = triples[i];
        //        ++index;
        //    }

        //    for (int i = 1; i < triples.Length; i += 2)
        //    {
        //        newTriples[index] = triples[i];
        //        ++index;
        //    }

        //    int[] smallSuffixArray = CreateSuffixArray(text, triples);
        //}

        //private static int[] CreateSimpleSuffixArray(string text, Triple[] triples)
        //{ }
    }
}