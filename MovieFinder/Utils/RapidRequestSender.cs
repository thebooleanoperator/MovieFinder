using System;
using System.Net.Http;
using System.Web;

namespace MovieFinder.Utils
{
    public class RapidRequestSender
    {
        private static string imdbRapidApiUrl = "https://movie-database-imdb-alternative.p.rapidapi.com/";

        public static HttpRequestMessage ImdbIdsRapidRequest(string title, string year)
        {
            string longurl = imdbRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["page"] = "1";
            query["type"] = "movie";
            query["s"] = title;
            query["y"] = year;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }

        public static HttpRequestMessage ImdbInfoRapidRequest(string imdbId, string year)
        {
            string longurl = imdbRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["type"] = "movie";
            query["plot"] = "full";
            query["i"] = imdbId;
            query["y"] = year;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }
    }
}
