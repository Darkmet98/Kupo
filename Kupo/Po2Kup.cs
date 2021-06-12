using System.IO;
using System.Reflection;
using Yarhl.FileFormat;
using Yarhl.IO;
using Yarhl.Media.Text;
using TextWriter = Yarhl.IO.TextWriter;

namespace Kupo
{
    public class Po2Kup : IConverter<Po, BinaryFormat>
    {
        public KUP Kup { get; set; }
        public BinaryFormat Convert(Po source)
        {
            var sr = new StringReplacer();
            sr.GenerateFontMap(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
                               Path.DirectorySeparatorChar + "dictionary.txt");
            for (int i = 0; i < Kup.Count; i++)
            {
                var entry = source.Entries[i];
                if (string.IsNullOrWhiteSpace(entry.Translated))
                    continue;

                Kup.Entries[i].EditedText = sr.GetModified(entry.Translated);
            }

            var textWriter = new TextWriter(new DataStream());
            textWriter.Write(Kup.SaveText());

            return new BinaryFormat(textWriter.Stream);
        }
    }
}
