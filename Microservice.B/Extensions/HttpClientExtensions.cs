using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Microservice.B.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAs<T>(this HttpResponseMessage response)
        {
            //  Handling the erroneous responses
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error calling the API: {response.ReasonPhrase}");
            }

            var dataString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonSerializer.Deserialize<T>(dataString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
