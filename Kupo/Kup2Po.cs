using Yarhl.FileFormat;
using Yarhl.IO;
using Yarhl.Media.Text;

namespace Kupo
{
    public class Kupo : IConverter<BinaryFormat, Po>
    {
        private KUP Kup;
        public Po Convert(BinaryFormat source)
        {
            Kup = KUP.LoadStream(source.Stream);
            var po = new Po
            {
                Header = new PoHeader("Any game", "dummy@dummy.com", System.Threading.Thread.CurrentThread.CurrentCulture.Name),
            };

            var i = 0;
            foreach (var kupEntry in Kup.Entries)
            {
                po.Add(new PoEntry
                {
                    Original = kupEntry.OriginalText,
                    Translated = kupEntry.EditedText == kupEntry.OriginalText ? "" : kupEntry.EditedText,
                    Context = $"{i++}",
                    ExtractedComments = $"Speaker: {kupEntry.Name}\nMax Length: {kupEntry.MaxLength}"
                });
            }


            return po;
        }
    }
}
