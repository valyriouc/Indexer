using DigitialLibrary.Markdown;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DigitialLibrary
{
    internal class Indexer
    {
        public string Basepath { get; init; }

        public Indexer(string basepath)
        {
            if (!Directory.Exists(basepath))
            {
                throw new DirectoryNotFoundException(
                    $"The directory {basepath} does not exists on the disk!");
            }

            Basepath = basepath;
        }

        public void Start()
        {
            string readmePath = Path.Combine(Basepath, "README.md");

            if (!File.Exists(readmePath))
            {
                throw new FileNotFoundException(
                    $"File README.md not found in specified directory");
            }

            MarkdownDocument document = MarkdownParser.Parse(readmePath);

            /////////// FOR TESTING REASONS /////////////////////////////////
            //foreach (KeyValuePair<string, string> meta in document.Metadata) 
            //{ 
            //    Console.WriteLine($"{meta.Key}: {meta.Value}");    
            //}

            if (!document.IsLibrary)
            {
                Console.WriteLine("This directory is not marked as library");
                return;
            }

            foreach (string path in Directory.GetDirectories(Basepath))
            { 
                IndexDirectory(path);
            }
        }

        private void IndexDirectory(string path)
        {
            Console.WriteLine($"Working on directory {path}...");

            List<IndexNode> nodes = new List<IndexNode>();

            using SHA256 hasher = SHA256.Create();

            foreach (string item in Directory.EnumerateFileSystemEntries(path))
            {
                if (Directory.Exists(item))
                {
                    IndexDirectory(item);
                    continue;
                }

                // TODO: Exclude this files from being mapped
                if (Path.GetExtension(item) == ".json" || Path.GetExtension(item) == ".md")
                {
                    continue;
                }

                string filename = Path.GetFileNameWithoutExtension(item);
                byte[] hash = hasher.ComputeHash(File.ReadAllBytes(item));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                IndexNode node = new IndexNode()
                {
                    Fullpath = item,
                    Title = filename,
                    Id = builder.ToString()
                };

                nodes.Add(node);
            }

            string[] splits = path.Split('\\');
            string dirname = splits[splits.Length - 1];

            using Stream outputStream = File.Create(Path.Combine(path, $"{dirname}.json"));

            JsonSerializer.Serialize(outputStream, nodes);
        }
    }
}
