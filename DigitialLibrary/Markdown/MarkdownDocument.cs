using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitialLibrary.Markdown
{
    internal class MarkdownDocument
    {
        public Dictionary<string, string> Metadata { get; }

        public bool IsLibrary => Metadata.ContainsKey("tag") && Metadata["tag"] == "Library";

        public MarkdownDocument()
        {
            Metadata = new Dictionary<string, string>();
        }
    }
}
