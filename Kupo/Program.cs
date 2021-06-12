using System;
using System.IO;
using Yarhl.FileSystem;
using Yarhl.Media.Text;

namespace Kupo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"Kupo - A simple Kup to Po and Po to Kup converter by Darkmet98. Version: 1.0");
            Console.WriteLine(@"Thanks to Pleonex for the Yarhl Libraries and Kuriimu for Kup Format.");

            if (args.Length == 0)
                Info();
            else if (args.Length == 1)
            {
                if (!File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
                    SingleFile(args[0]);
                else
                    Info();
            }
            else
                Info();
        }

        private static void SingleFile(string file)
        {
            if (file.Contains(".po"))
            {
                Console.WriteLine($"Importing {file}...");
                var fileOri = file.Replace(".po", "");
                if (!File.Exists(fileOri))
                    throw new Exception("The original kup file doesn't exist");


                var node = NodeFactory.FromFile(file);

                node.TransformWith(new Binary2Po()).TransformWith(new Po2Kup()
                {
                    Kup = KUP.Load(fileOri)
                }).Stream.WriteTo(fileOri.Replace(".kup", "_new.kup"));
            }
            else if (file.Contains(".kup"))
            {
                Console.WriteLine($"Exporting {file}...");
                var node = NodeFactory.FromFile(file);
                node.TransformWith(new Kupo()).TransformWith(new Po2Binary()).Stream.WriteTo(file+".po");
            }
            else
                Info();
        }

        private static void Info()
        {
            Console.WriteLine(@"Export: Kupo 'file.kup'");
            Console.WriteLine(@"Import: Kupo 'file.po'");
        }
    }
}
