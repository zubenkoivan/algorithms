using Algorithms.Sorting;
using Algorithms.TextProcessing.Abstractions;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms
{
    public class BurrowsWheelerInverseTransform : TextTransformer
    {
        public override string Transform(string transformedText)
        {
            int[] firstChars = GetFirstChars(transformedText);
            var originalText = new char[transformedText.Length - 1];

            for (int i = 0, j = firstChars[0]; i < originalText.Length; ++i)
            {
                j = firstChars[j];
                originalText[i] = transformedText[j];
            }

            return new string(originalText);
        }

        private static int[] GetFirstChars(string text)
        {
            var map = new int[text.Length];

            for (int i = 0; i < map.Length; ++i)
            {
                map[i] = i;
            }

            RadixSort.Sort(map, i => text[i]);

            return map;
        }
    }
}
