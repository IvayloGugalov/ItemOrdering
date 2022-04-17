using System.Linq;
using System.Net.Http;

using Microsoft.Net.Http.Headers;

namespace HttpClientExtensions
{
    public static class CookieExtensionMethods
    {
        /// <summary>
        /// Adds the specified cookie name returned from a response to the outgoing request
        /// </summary>
        /// <param name="path"></param>
        /// <param name="response"></param>
        /// <param name="httpMethodType"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static HttpRequestMessage SetCookieOnRequest(string path, HttpResponseMessage response, HttpMethod httpMethodType, string cookieName)
        {
            var request = new HttpRequestMessage(httpMethodType, path);
            if (!response.Headers.TryGetValues("Set-Cookie", out var values)) return request;

            var cookie = SetCookieHeaderValue.ParseList(values.ToList()).FirstOrDefault(x => x.Name == cookieName);
            if (cookie != null)
            {
                request.Headers.Add("Cookie", new CookieHeaderValue(cookie.Name, cookie.Value).ToString());
            }

            return request;
        }
    }
}
