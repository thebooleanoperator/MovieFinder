
using MovieFinder.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;

namespace MovieFinder.Utils
{
    public class DataReader
    {
        public static List<string> ReadMovies(string file)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(file);
                var parsedJson = JArray.Parse(json);
                var test = parsedJson.ToObject<Movies[]>();

                var movieTitles = new List<string>();
                foreach(var movie in test)
                {
                    movieTitles.Add(movie.Title);
                }

                return movieTitles; 
            }
        }
    }
}
