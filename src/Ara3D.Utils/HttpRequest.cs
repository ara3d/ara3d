using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Ara3D.Utils
{
    public static class HttpRequest
    {
        public static async Task<HttpResponseMessage> GetAsync(
            this Uri uri,
            CancellationToken token = default)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, token);
                response.EnsureSuccessStatusCode();
                return response;
            }
        }

        public static bool HttpStatusCodeIsSuccess(int statusCode)
        {
            // Make sure the status code exists among the standard HttpStatusCode values.
            if (!Enum.IsDefined(typeof(HttpStatusCode), statusCode))
            {
                return false;
            }

            return statusCode >= 200 && statusCode < 300;
        }
    }
}
