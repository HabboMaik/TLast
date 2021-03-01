using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TLast.Models;

namespace TLast
{
    class Habbo
    {
        private readonly string _token;

        private IEnumerable<string> _cookies = null!;

        private HttpClient _client = null!;

        public string CurrentSSO { get; private set; }

        public Habbo(string token)
        {
            _token = token;
        }

        public async Task<bool> TryLoginAsync([NotNull] string email, [NotNull] string pass)
        {
            try
            {
                _client = new HttpClient();

                using var request = new HttpRequestMessage(new HttpMethod("POST"),
                                                           "https://www.habbo.com.br/api/public/authentication/login");

                request.Headers.TryAddWithoutValidation("Host", "www.habbo.com.br");

                request.Headers.TryAddWithoutValidation("Accept",
                                                        "text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5");

                request.Headers.TryAddWithoutValidation("User-Agent",
                                                        "Mozilla/5.0 (Android; U; pt-BR) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/33.1");

                request.Headers.TryAddWithoutValidation("x-flash-version", "33,1,1,98");
                request.Headers.TryAddWithoutValidation("Referer", "app:/HabboTablet.swf");
                request.Headers.TryAddWithoutValidation("X-Habbo-Device-Type", "android");

                request.Headers.TryAddWithoutValidation("x-habbo-api-deviceid",
                                                        "78cdfaa9e2f566145bce89a3977a9618b138978d3ef63caab1d7dbf9f104c4c:7758ef4bd09f61eb85dfdb706e996f83678508f0");

                request.Headers.TryAddWithoutValidation("X-Habbo-Device-ID",
                                                        "78cdfaa9e2f566145bce89a3977a9618b138978d3ef63caab1d7dbf9f104c4c:7758ef4bd09f61eb85dfdb706e996f83678508f0");

                request.Headers.TryAddWithoutValidation("Cookie",
                                                        "browser_token=s%3A7bsBmnhjEbx4do3gluFqLWkrmwg6iUi0-f6ntpyzlX8.9ElBU5hoagamqQPDMX4dw6WN3fzTuiarvqgsHTc8DPk");

                request.Content =
                    new
                        StringContent($"{{\"captchaToken\":\"{_token}\",\"email\":\"{email}\",\"password\":\"{pass}\"}}");

                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await _client.SendAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    _cookies = response.Headers.GetValues("set-cookie")
                                       .Append("browser_token=s%3A7bsBmnhjEbx4do3gluFqLWkrmwg6iUi0-f6ntpyzlX8.9ElBU5hoagamqQPDMX4dw6WN3fzTuiarvqgsHTc8DPk");

                    return true;
                }

                return false;
            }
            catch 
            {
                return false;
            }
        }

        public async Task<bool> TryGetSsoTokenAsync()
        {
            try
            {
                _client = new HttpClient();

                using var request =
                    new HttpRequestMessage(new HttpMethod("GET"), "https://www.habbo.com.br/api/ssotoken");

                request.Headers.TryAddWithoutValidation("Host", "www.habbo.com.br");

                request.Headers.TryAddWithoutValidation("Accept",
                                                        "text/xml, application/xml, application/xhtml+xml, text/html;q=0.9, text/plain;q=0.8, text/css, image/png, image/jpeg, image/gif;q=0.8, application/x-shockwave-flash, video/mp4;q=0.9, flv-application/octet-stream;q=0.8, video/x-flv;q=0.7, audio/mp4, application/futuresplash, */*;q=0.5");

                request.Headers.TryAddWithoutValidation("User-Agent",
                                                        "Mozilla/5.0 (Android; U; pt-BR) AppleWebKit/533.19.4 (KHTML, like Gecko) AdobeAIR/33.1");

                request.Headers.TryAddWithoutValidation("x-flash-version", "33,1,1,98");
                request.Headers.TryAddWithoutValidation("Referer", "app:/HabboTablet.swf");
                request.Headers.TryAddWithoutValidation("X-Habbo-Device-Type", "android");

                request.Headers.TryAddWithoutValidation("x-habbo-api-deviceid",
                                                        "78cdfaa9e2f566145bce89a3977a9618b138978d3ef63caab1d7dbf9f104c4c:7758ef4bd09f61eb85dfdb706e996f83678508f0");

                request.Headers.TryAddWithoutValidation("X-Habbo-Device-ID",
                                                        "78cdfaa9e2f566145bce89a3977a9618b138978d3ef63caab1d7dbf9f104c4c:7758ef4bd09f61eb85dfdb706e996f83678508f0");

                request.Headers.TryAddWithoutValidation("Cookie", _cookies);

                var response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var sso = JsonConvert.DeserializeObject<SSOTokenModel>(content).SsoToken; //{"ssoToken":"fcef80f9-51cb-414a-ba13-88d9235a2ae4-88314676"}

                    CurrentSSO = sso;

                    return true;
                }


                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
