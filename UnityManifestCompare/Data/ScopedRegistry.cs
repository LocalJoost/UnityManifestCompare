using Newtonsoft.Json;

namespace UnityManifestCompare.Data
{
    internal class ScopedRegistry
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
    }
}
