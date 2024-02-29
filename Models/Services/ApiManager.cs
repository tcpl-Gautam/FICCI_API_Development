using System.Net;

namespace FICCI_API.Models.Services
{
    public class ApiManager
    {
        private readonly string _api;
        private readonly string _authToken;

        public ApiManager(string api, string authToken = "")
        {
            _api = api;
            _authToken = authToken;
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(_authToken))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authToken}");
            }
            return client;
        }
        private async Task<Tuple<HttpStatusCode, string>> SendRequestAsync(HttpMethod method, HttpContent content = null)
        {
            using (var client = CreateHttpClient())
            {
                using (var response = await client.SendAsync(new HttpRequestMessage(method, _api) { Content = content }).ConfigureAwait(false))
                {
                    var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return new Tuple<HttpStatusCode, string>(response.StatusCode, responseContent);
                }
            }
        }
        public async Task<Tuple<HttpStatusCode, string>> PostFormData(IEnumerable<KeyValuePair<string, string>> formData)
        {
            var content = new FormUrlEncodedContent(formData);
            return await SendRequestAsync(HttpMethod.Post, content);
        }
    }
}
