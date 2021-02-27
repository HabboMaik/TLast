using Newtonsoft.Json;

namespace TLast.Models
{
    internal class ProductVersionModel
    {
        [JsonProperty("productVersion")]
        public string ProductVersion { get; set; }
    }
}