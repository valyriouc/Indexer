using System.Text.Json.Serialization;

namespace DigitialLibrary
{
    internal class IndexNode
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("fullpath")]
        public string Fullpath { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "Not read";

        [JsonPropertyName("categories")]
        public List<string> Categories { get; set; } = new List<string>();

        [JsonPropertyName("title")]
        public string Title { get; set; }
    }
}
