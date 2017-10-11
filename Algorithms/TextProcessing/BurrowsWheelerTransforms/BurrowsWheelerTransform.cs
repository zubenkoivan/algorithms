using System.Text;
using Algorithms.TextProcessing.Abstractions;

namespace Algorithms.TextProcessing.BurrowsWheelerTransforms
{
    public class BurrowsWheelerTransform : TextTransformer
    {
        private readonly SuffixArrayConstructor suffixArrayConstructor;

        public BurrowsWheelerTransform(SuffixArrayConstructor suffixArrayConstructor)
        {
            this.suffixArrayConstructor = suffixArrayConstructor;
        }

        public override string Transform(string text)
        {
            text = text + char.MinValue;
            int[] suffixArray = suffixArrayConstructor.Construct(text);
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
