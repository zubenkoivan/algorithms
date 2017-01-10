using System.Text;
using Algorithms.TextProcessing.SuffixArrays.KarkkainenSanders;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms
{
    public class BurrowsWheelerTransform
    {
        public string Result { get; }

        public BurrowsWheelerTransform(string text)
        {
            Result = Transform(text + char.MinValue);
        }

        private static string Transform(string text)
        {
            int[] suffixArray = new KarkkainenSandersConstructor().Create(text);
            var textBuilder = new StringBuilder(text.Length);

            for (int i = 0; i < suffixArray.Length; ++i)
            {
                int suffixIndex = suffixArray[i];
                int textIndex = suffixIndex == 0 ? (text.Length - 1) : (suffixIndex - 1);
                textBuilder.Append(text[textIndex]);
            }

            return textBuilder.ToString();
        }
    }
}
