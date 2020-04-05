using System;
using System.Net.Http;
using System.Web;

namespace MovieFinder.Utils
{
    public class RapidRequestSender
    {
        private static string imdbRapidApiUrl = "https://movie-database-imdb-alternative.p.rapidapi.com/";
        private static string utellyRapidApiUrl = "https://utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com/lookup";
        private static string idRapidUrl = "https://imdb-internet-movie-database-unofficial.p.rapidapi.com/search";

        /// <summary>
        /// This is used to get an array of JSON ImdbIds from a title string.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HttpRequestMessage ImdbIdsRapidRequest(string title, int? year)
        {
            string longurl = imdbRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["page"] = "1";
            query["type"] = "movie";
            query["y"] = year.ToString();
            query["s"] = title;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }

        /// <summary>
        /// Gets a list of Ids by movie title from Rapid Api Imdb GET endpoint.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HttpRequestMessage IdsRapidRequest(string title)
        {
            string longurl = idRapidUrl + $"/{title}";
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "imdb-internet-movie-database-unofficial.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }

        /// <summary>
        /// Used to get an array of JSON Movies with complete movie info.
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static HttpRequestMessage ImdbInfoRapidRequest(string imdbId, string year)
        {
            string longurl = imdbRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["type"] = "movie";
            query["plot"] = "short";
            query["i"] = imdbId;
            query["y"] = year;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }

        /// <summary>
        /// Used to get a Movie with all info. Overrides ImdbInfoRapidRequest by changing signature.
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static HttpRequestMessage ImdbInfoRapidRequest(string imdbId)
        {
            string longurl = imdbRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["r"] = "json";
            query["type"] = "movie";
            query["plot"] = "short";
            query["i"] = imdbId;
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "movie-database-imdb-alternative.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }

        /// <summary>
        /// Gets an array of movies streaming data from Rapid Api Utelly GET endpoint.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HttpRequestMessage UtellyRapidRequest(string title)
        {
            string longurl = utellyRapidApiUrl;
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["term"] = title;
            query["country"] = "us";
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, longurl);

            request.Headers.Add("x-rapidapi-host", "utelly-tv-shows-and-movies-availability-v1.p.rapidapi.com");
            request.Headers.Add("x-rapidapi-key", "98148972b0mshcf5fd6487ff6f4ap1b7554jsn9f54713ce432");

            return request;
        }
    }
}
