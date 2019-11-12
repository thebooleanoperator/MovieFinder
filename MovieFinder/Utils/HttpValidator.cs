using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieFinder.Utils
{
    public class HttpValidator
    {
        public static async Task<JObject> ValidateAndParseResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Status: {response.StatusCode}. Response did not sucessfully resolve.");
            }
            //Read the string asynchronosouly from response. 
            var responseBodyAsText = await response.Content.ReadAsStringAsync();
            //Parse the string into an object that contains an array of objects. 
            var parsedJson = JObject.Parse(responseBodyAsText);

            //We need to check if the response came back false.
            var movieFound = parsedJson["Response"].Value<bool>();
            if (!movieFound)
            {
                throw new ArgumentException($"Status: {response.StatusCode}. Movie not found.");
            }

            return parsedJson;
        }
    }
}
