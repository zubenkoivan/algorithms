using Algorithms.Sorting;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms
{
    public class BurrowsWheelerInverseTransform
    {
        public string Result { get; }

        public BurrowsWheelerInverseTransform(string transformedText)
        {
            int[] firstToLastMap = RestoreFirstToLastMap(transformedText);
            var originalText = new char[transformedText.Length - 1];
            int j = firstToLastMap[0];

            for (int i = 0; i < originalText.Length; ++i)
            {
                j = firstToLastMap[j];
                originalText[i] = transformedText[j];
            }

            Result = new string(originalText);
        }

        private static int[] RestoreFirstToLastMap(string text)
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
