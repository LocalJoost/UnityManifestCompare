using Newtonsoft.Json;

namespace UnityManifestCompare.Data
{
    internal class Manifest
    {
        [JsonProperty("scopedRegistries")]
        public List<ScopedRegistry> ScopedRegistries { get; set; }

        [JsonProperty("dependencies")]
        public Dictionary<string,string> Dependencies { get; set; }
    }
}
