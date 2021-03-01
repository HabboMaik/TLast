using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TLast.Models
{
    class SSOTokenModel
    {
        [JsonProperty("ssoToken")]
        public string SsoToken { get; set; }
    }
}
