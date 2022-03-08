using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace HttpClientExtensions
{
    public static class HttpClientExtensionMethods
    {
        #region GET Extension Methods

        /// <summary>
        /// Sends a GET request to the specified Uri and returns the deserialized response content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <returns>T object from response.Content.ReadFromJsonAsync</returns>
        public static async Task<T> GetDeserializedJsonResult<T>(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<T>();

            return result;
        }

        /// <summary>
        /// Sends a GET request to the specified Uri and ensures the response message is 404 (NotFound).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAndEnsureNotFound(this HttpClient client, string requestUri)
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureNotFound();

            return response;
        }

        #endregion

        #region PUT Extension Methods

        /// <summary>
        /// Sends a PUT request to the specified Uri and ensures the response message is 404 (NotFound).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<T> PutAndReceiveDeserializedJsonResult<T>(this HttpClient client, string requestUri, StringContent content)
        {
            var response = await client.PutAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            var responseToSend = await response.Content.ReadFromJsonAsync<T>();

            return responseToSend;
        }

        /// <summary>
        /// Sends a PUT request to the specified Uri and ensures the response message is 404 (NotFound).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> PutAndEnsureNotFound(this HttpClient client, string requestUri, HttpContent content)
        {
            var response = await client.PutAsync(requestUri, content);
            response.EnsureNotFound();

            return response;
        }

        #endregion

        #region POST Extension Methods

        /// <summary>
        /// Sends a POST request to the specified Uri and deserializes the response content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns>T object from response.Content.ReadFromJsonAsync</returns>
        public static async Task<T> PostAndReceiveResult<T>(this HttpClient client, string requestUri, HttpContent content = null)
        {
            HttpResponseMessage response;
            if (content == null)
            {
                response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri));
            }
            else
            {
                response = await client.PostAsync(requestUri, content);
            }

            response.EnsureSuccessStatusCode();

            var responseToSend = await response.Content.ReadFromJsonAsync<T>();

            return responseToSend;
        }

        /// <summary>
        /// Sends a POST request to the specified Uri.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns>HttpResponseMessage</returns>
        public static async Task<HttpResponseMessage> PostAndReceiveMessage(this HttpClient client, string requestUri, HttpContent content = null)
        {
            HttpResponseMessage response;
            if (content == null)
            {
                response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri));
            }
            else
            {
                response = await client.PostAsync(requestUri, content);
            }

            return response;
        }

        #endregion

        #region DELETE Extension Methods

        /// <summary>
        /// Sends a DELETE request to the specified Uri and ensures the response message is 404 (NotFound).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAndEnsureNotFound(this HttpClient client, string requestUri)
        {
            var response = await client.DeleteAsync(requestUri);
            response.EnsureNotFound();

            return response;
        }

        /// <summary>
        /// Sends a DELETE request to the specified Uri and ensures the response message is 204 (NoContent).
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> DeleteAndEnsureNoContent(this HttpClient client, string requestUri)
        {
            var response = await client.DeleteAsync(requestUri);
            response.EnsureNoContent();

            return response;
        }

        #endregion

        #region HttpResponseMessage status codes

        /// <summary>
        /// Evaluates that the HttpResponseMessage is 404 (NotFound) status code.
        /// </summary>
        /// <param name="response"></param>
        public static void EnsureNotFound(this HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.NotFound)
            {
                throw new HttpRequestException($"Expected 404 Not Found but was {response.StatusCode}.");
            }
        }

        /// <summary>
        /// Evaluates that the HttpResponseMessage is 204 (NoContent) status code.
        /// </summary>
        /// <param name="response"></param>
        public static void EnsureNoContent(this HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new HttpRequestException($"Expected 204 No Content but was {response.StatusCode}.");
            }
        }

        #endregion
    }
}
