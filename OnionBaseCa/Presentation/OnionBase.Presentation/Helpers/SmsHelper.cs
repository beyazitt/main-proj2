using Newtonsoft.Json;
using OnionBase.Presentation.Interfaces;
using Sinch.ServerSdk;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace OnionBase.Presentation.Helpers
{
    public class SmsHelper : ISmsHelper
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        //public SmsHelper(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}
        //public async Task<bool> SendSms(string message, string target)
        //{
        //    var httpClient = _httpClientFactory.CreateClient();
        //    string url = string.Format("***REDACTED***", message, target);
        //    var httpRequest = new HttpRequestMessage(HttpMethod.Get, url) { };
        //    var httpResponse = await httpClient.SendAsync(httpRequest);
        //    return httpResponse.IsSuccessStatusCode;
        //}


        private readonly IHttpClientFactory _clientFactory;

        public SmsHelper(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<bool> SendSms(string message, string phoneNumber)
        {
            var url = "https://sms.api.sinch.com/xms/v1/c5e94770dd0848e5b8cb77b43bf13bbc/batches";

            var sms = new
            {
                from = "447520652428",
                to = new List<string> { phoneNumber },
                body = message
            };

            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                "a491f8c73f154583853857997e249141");

            var content = new StringContent(JsonConvert.SerializeObject(sms), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

    }
}
