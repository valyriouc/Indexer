using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitialLibrary.Markdown
{
    internal class MarkdownParser
    {
        public static MarkdownDocument Parse(string markdownFile)
        {
            if (string.IsNullOrEmpty(markdownFile))
            {
                throw new ArgumentNullException(
                    "Expected path to markdown file");
            }

            MarkdownDocument document = new MarkdownDocument();

            using Stream stream = File.OpenRead(markdownFile);

            using StreamReader reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                TryReadMetadata(reader, document);
            }

            return document;
        }

        private static bool TryReadMetadata(StreamReader reader, MarkdownDocument document)
        {
            string? metaStart = reader.ReadLine();

            if (string.IsNullOrEmpty(metaStart) || metaStart != "---")
            {
                return false;
            }

            while (true)
            {
                string? meta = reader.ReadLine();

                if (meta == "---")
                {
                    break;
                }
                
                if (string.IsNullOrEmpty(meta))
                {
                    throw new Exception("No empty lines in meta block allowed");
                }

                KeyValuePair<string, string> keyValue = ReadMetaLine(meta);

                document.Metadata.Add(keyValue.Key, keyValue.Value);
            }

            return true;

        }

        private static KeyValuePair<string, string> ReadMetaLine(string metaline)
        {

            if (!metaline.Contains(":"))
            {
                throw new Exception("Malformed metadata. Expected :");
            }

            string key = string.Empty;
            string value = string.Empty;

            bool readingKey = true;
            foreach (char sign in metaline)
            {
                if (sign == ':')
                {
                    readingKey = false;
                    continue;
                }

                if (char.IsWhiteSpace(sign))
                {
                    continue;
                }

                if (readingKey)
                {
                    key += sign;
                }
                else
                {
                    value += sign;
                }
            }

            return new KeyValuePair<string, string>(key, value);
        }

    }
}
