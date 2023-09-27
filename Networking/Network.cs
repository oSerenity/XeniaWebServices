using System.Net.Sockets;
using System.Net;

namespace XeniaWebServices.Networking
{
    public class Network
    {
        private IHttpClientFactory httpClientFactory;

        public Network(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }
        public async Task<string> GetPublicIpAddressAsync()
        {
            using (var httpClient = httpClientFactory.CreateClient())
            {
                try
                {
                    var response = await httpClient.GetAsync("https://api.ipify.org/");
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                catch (HttpRequestException)
                {
                    // Handle request exception
                    return "";
                }
            }
        }

        public async Task<string> UpdateOrFetchIpAddress(string ipv4)
        {
            if (ipv4 == "127.0.0.1" || ipv4.StartsWith("192.168"))
            {
                // Hi me! Who are you?
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://api.ipify.org/");
                    ipv4 = response.Trim(); // Assuming the response is a single IP address.
                }
            }

            return ipv4;
        }
    }
}
