using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Botticelli.Framework.Vk.API.Responses
{
    public class SessionDataResponse
    {
        [JsonPropertyName("server")]
        public string Server { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("ts")]
        public int Ts { get; set; }
    }
}
